using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectedShellCollisionSound : MonoBehaviour {

    [SerializeField]
    WeaponBase.WeaponType weaponType;
    [SerializeField]
    private AudioSource _audioSource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlayEjectedShellHitAudio(_audioSource, weaponType);
    }
}
