using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour {

    private static WeaponsManager s_instance = null;
    public static WeaponsManager Instance
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
        int weaponTypeCount = Enum.GetNames(typeof(WeaponBase.WeaponType)).Length; //get the number of different weapon types
        _indexedWeapons = new WeaponBase[weaponTypeCount + 1];

        foreach (WeaponBase weaponBase in _weapons)
        {
            int index = (int)weaponBase.weaponType; //convert enum to rank index

            if (_indexedWeapons[index] != null)
            {
                Debug.LogError("There are more than one weapon of the same type in weaponsManager. This is not allowed. Create a new weapon type if you want to add a weapon with a different property");
            }
            _indexedWeapons[index] = weaponBase;
        }
    }


    [SerializeField]
    private WeaponBase[] _weapons;

    /// <summary>
    /// All the weapons in _weapons are put into _indexed weapons so that they can easily be fetched using rank index
    /// </summary>
    private WeaponBase[] _indexedWeapons = null;
	// Use this for initialization
	void Start () {
        foreach(WeaponBase wb in _weapons)
        {
            wb.Init();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public WeaponBase GetWeapon(WeaponBase.WeaponType weaponType)
    {
        int index = (int)weaponType;
        if(_indexedWeapons[index] == null)
        {
            Debug.LogError("A weapon of type >>" + weaponType + "<< is not present in weaponsManager. Please create one.");
            return null;
        }
        return _indexedWeapons[index];
    }

    void OnDestroy()
    {
        s_instance = null;
    }
}
