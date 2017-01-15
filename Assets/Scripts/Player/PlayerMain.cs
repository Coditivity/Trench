using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

public class PlayerMain : MonoBehaviour {

    private static PlayerMain s_instance = null;
    public static PlayerMain Instance
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
    GameObject[] _weaponSprites = null;
    [SerializeField]
    [Tooltip("Time taken to unwield a weapon")]
    private float _weaponUnwieldTime = .3f;

    void Awake()
    {
        s_instance = this;
    }
    /// <summary>
    /// Current weapon used by the player
    /// </summary>
    WeaponBase _activeWeapon = null;
	// Use this for initialization
	void Start () {
        _activeWeapon = WeaponsManager.Instance.GetWeapon(WeaponBase.WeaponType.Pistol);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Called when player picks up/hits a weapon
    /// </summary>
    /// <param name="weapon"></param>
    public void OnWeaponPickup(WeaponPickup weapon)
    {
        //_activeWeapon = weapon; //set the current weapon as the new weapon
     //   if((int)_activeWeapon.weaponType < (int)weapon.weaponType) //if rank of the picked up weapon is greater than that of the current weapon
        {
            AnimatorStates.Set(AnimatorStates.AnimationParameter.Unwield, _activeWeapon.weaponType);
            DelayedCaller.Instance.AddDelayedCall(() =>
            {
                _weaponSprites[(int)_activeWeapon.weaponType - 1].SetActive(false);
                _activeWeapon = WeaponsManager.Instance.GetWeapon(weapon.weaponType);
                _weaponSprites[(int)weapon.weaponType - 1].SetActive(true);
            }, _weaponUnwieldTime);
                     
        }
    }


    void OnDestroy()
    {
        s_instance = null;
    }
}
