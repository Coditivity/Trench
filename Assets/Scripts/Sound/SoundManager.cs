using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    AudioSource _audioSourcePickup = null;
    [SerializeField]
    AudioSource _audioSourceWeaponChange = null;


    private static SoundManager s_instance = null;
    public static SoundManager Instance
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

    void Awake()
    {
        s_instance = this;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayWeaponChange(AudioClip audioClip)
    {
        
        _audioSourceWeaponChange.clip = audioClip;
        _audioSourceWeaponChange.Play();
    }

    public void PlayWeaponPickup(AudioClip audioClip)
    {
        _audioSourcePickup.clip = audioClip;
        _audioSourcePickup.Play();
    }

    void OnDestroy()
    {
        s_instance = null;
    }
}
