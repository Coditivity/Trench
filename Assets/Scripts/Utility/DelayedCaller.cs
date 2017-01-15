﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedCaller : MonoBehaviour {

    private static DelayedCaller s_instance = null;
    public static DelayedCaller Instance
    {
        get
        {
            return s_instance;
        }
        private set
        {
            s_instance = value;
        }
    }

    private class DelayedCall
    {
        public float _startTime = -1;
        float _delay = 0;
        UnityAction _functionToCall = null;
        public bool isRunning = false;
      
        public void Init(UnityAction functionToCall, float delay)
        {
            _startTime = Time.time;
            _delay = delay;
            _functionToCall = functionToCall;
            isRunning = true;
        }   
        
        public bool bHasDelayedLongEnough
        {
            get
            {
                return (Mathf.Abs(Time.time - _startTime) >= _delay);
            }
        }     

        public void InvokeAndDeactivate()
        {
            isRunning = false;
            _functionToCall.Invoke();
            _functionToCall = null;

        }
    }

    [SerializeField]
    int _bufferSize = 10;
    private DelayedCall[] _delayedCalls;

    void Awake()
    {
        s_instance = this;
    }
    
	// Use this for initialization
	void Start () {
		_delayedCalls = new DelayedCall[_bufferSize];
        for(int i=0;i<_bufferSize;i++)
        {
            _delayedCalls[i] = new DelayedCall();
            _delayedCalls[i].isRunning = false;
        }
    }
    public void AddDelayedCall(UnityAction action, float delay)
    {
        for (int i=0;i<_bufferSize;i++)
        {
            if(!_delayedCalls[i].isRunning)
            {
                _delayedCalls[i].Init(action, delay);
                return;
            }
        }
        Debug.LogError("Buffer is full. Consider increasing the buffer size");
    }

    private void UpdateAndInvokeCalls()
    {
        for(int i=0;i<_bufferSize;i++)
        {
            if(_delayedCalls[i].isRunning)
            {
                if (_delayedCalls[i].bHasDelayedLongEnough)
                {
                    _delayedCalls[i].InvokeAndDeactivate();
                }
            }
        }
    }
    // Update is called once per frame
    void Update () {
        UpdateAndInvokeCalls();
	}
}