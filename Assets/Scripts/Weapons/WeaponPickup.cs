using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {

    [SerializeField]
    WeaponBase.WeaponType _weaponType;
    [SerializeField]
    AudioClip _audioClipPickup = null;

    public AudioClip AudioClipPickup
    {
        get
        {
            return _audioClipPickup;
        }
    }

    public WeaponBase.WeaponType weaponType
    {
        get
        {
            return _weaponType;
        }
        private set
        {
            _weaponType = value;

        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider collider)
    {
        Inventory.Instance.OnWeaponPickup(this);
        gameObject.SetActive(false);
    }
}
