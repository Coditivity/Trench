using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private static Inventory s_instance = null; //Singleton instance of the class
    public static Inventory Instance
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

    
    public class WeaponInventoryData
    {
        public WeaponBase.WeaponType weaponType;
        /// <summary>
        /// Determines weather the weapon is actually listed in the inventory. Set to false to remove the weapon from Inventory.
        ///  
        /// </summary>
        public bool isWeaponInInventory = false;
    }
    /// <summary>
    /// Max number of unique weapon types in the game. 
    /// </summary>
    private const int MaximumUniqueWeaponTypes = 20;
    WeaponInventoryData[] _weaponDatas = null;
    void Awake()
    {
        s_instance = this;
        _weaponDatas = new WeaponInventoryData[MaximumUniqueWeaponTypes];
        for(int i=0;i<MaximumUniqueWeaponTypes;i++)
        {
            _weaponDatas[i] = new WeaponInventoryData();
        }
    }

    private bool[] _weaponAvailability;
	// Use this for initialization
	void Start () {
        _weaponAvailability = new bool[Enum.GetNames(typeof(WeaponBase.WeaponType)).Length + 1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnWeaponPickup(WeaponPickup weaponPickup)
    {
        AddWeaponToInventory(weaponPickup, true);
    }

    
    public bool IsWeaponInInventory(WeaponBase.WeaponType weaponType)
    {
        return _weaponAvailability[(int)weaponType];
    }

    /// <summary>
    /// Add a weapon to inventory if a weapon of that type is not present already
    /// </summary>
    /// <param name="weapon"></param>
    public void AddWeaponToInventory(WeaponPickup weaponPickup, bool notifyPlayer)
    {
        bool isWeaponAlreadyAdded = false; //is a weapon with the same type already in the inventory
        int firstFreeInventoryWeaponDataIndex = -1; //represents the index of the first weaponinventoryData object in the array which has isWeaponInInventory set to false
        for(int i=0;i<MaximumUniqueWeaponTypes;i++)
        {
            if(_weaponDatas[i].weaponType == weaponPickup.weaponType && _weaponDatas[i].isWeaponInInventory) //if there is already a weapon of the same type
            {
                isWeaponAlreadyAdded = true; 
                break; //no need to do anything. Break out of loop
            }
            if(!_weaponDatas[i].isWeaponInInventory)
            {
                firstFreeInventoryWeaponDataIndex = i;
            }
        }

        if(!isWeaponAlreadyAdded && firstFreeInventoryWeaponDataIndex ==-1) //there is no weapon of this type in the inventory and the inventory doesn't have free space to add the new weapon type
        {
            Debug.LogError("Inventory is full. Can't add the new weapon with type " + weaponPickup.weaponType);
        }
        else if(!isWeaponAlreadyAdded) //if a weapon of this type has not been added yet
        {
            _weaponDatas[firstFreeInventoryWeaponDataIndex].weaponType = weaponPickup.weaponType;
            _weaponDatas[firstFreeInventoryWeaponDataIndex].isWeaponInInventory = true;
            _weaponAvailability[(int)weaponPickup.weaponType] = true;
            if (notifyPlayer)
            {
                PlayerMain.Instance.OnWeaponPickup(weaponPickup);
            }
          //  Debug.LogError("Adding " + weaponPickup.weaponType + " to inventory");
        }
        else
        {
          //  Debug.LogError("Not adding " + weaponPickup.weaponType + " to inventory becuase it is already present");
        }        
    }

    public WeaponInventoryData GetWeaponData(int index)
    {
        return _weaponDatas[index];
    }

    void OnDestroy()
    {
        s_instance = null;
    }
}
