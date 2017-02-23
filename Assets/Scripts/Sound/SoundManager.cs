using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


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

    [SerializeField]
    AudioSource _audioSourcePickup = null;
    [SerializeField]
    AudioSource _audioSourceWeaponChange = null;
    [SerializeField]
    EjectedShellAudioData[] _ejectedShellAudioDatas = null;
    

    [System.Serializable]
    class EjectedShellAudioData
    {
        public WeaponBase.WeaponType weaponType;
        public AudioClip audioClip;
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

    public void PlayAudio(AudioSource audioSource, AudioClip audioClip)
    {
      //  audioSource.clip = audioClip;
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayEjectedShellHitAudio(AudioSource audioSource, WeaponBase.WeaponType weaponType)
    {
        audioSource.PlayOneShot(_ejectedShellAudioDatas[((int)weaponType - 1)].audioClip);
    }

    void OnDestroy()
    {
        s_instance = null;
    }
}
