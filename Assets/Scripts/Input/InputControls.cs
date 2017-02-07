using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputControls : MonoBehaviour {

    private static InputControls s_instance = null;
    public static InputControls Instance
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

    /// <summary>
    /// The event that will be fired when primary attack button goes down
    /// </summary>
    private UnityEvent _onPrimaryAttackStartEvent = new UnityEvent();
    public UnityEvent OnPrimaryAttackStartEventListener
    {
        get
        {
            return _onPrimaryAttackStartEvent;
        }
        private set
        {
            _onPrimaryAttackStartEvent = value;
        }
    }

    /// <summary>
    /// The event that will be fired when primary attack button goes up
    /// </summary>
    private UnityEvent _onPrimaryAttackEndEvent = new UnityEvent();
    public UnityEvent OnPrimaryAttackEndEventListener
    {
        get
        {
            return _onPrimaryAttackEndEvent;
        }
        private set
        {
            _onPrimaryAttackEndEvent = value;
        }
    }

    void Awake()
    {
        s_instance = this;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            _onPrimaryAttackStartEvent.Invoke();
        }
        if(Input.GetMouseButtonUp(0))
        {
            _onPrimaryAttackEndEvent.Invoke();
        }
	}

    void OnDestroy()
    {
        s_instance = null;
    }
}
