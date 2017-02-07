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
    [SerializeField]
    private WeaponPickup _firstWeaponPickup = null;

    void Awake()
    {
        s_instance = this;
    }
    /// <summary>
    /// Current weapon used by the player
    /// </summary>
    WeaponBase _activeWeapon = null;
    public WeaponBase ActiveWeapon
    {
        get
        {
            return _activeWeapon;
        }
    }

	// Use this for initialization
	void Start () {
        _activeWeapon = WeaponsManager.Instance.GetWeapon(WeaponBase.WeaponType.Pistol); //set pistol as the active weapon
        Inventory.Instance.AddWeaponToInventory(_firstWeaponPickup, false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!_isWeaponBeingSwitched)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.Instance.IsWeaponInInventory(WeaponBase.WeaponType.Pistol))
            {
                if (_activeWeapon.weaponType != WeaponBase.WeaponType.Pistol)
                {
                    ChangeActiveWeapon(WeaponBase.WeaponType.Pistol);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && Inventory.Instance.IsWeaponInInventory(WeaponBase.WeaponType.Shotgun))
            {
                if (_activeWeapon.weaponType != WeaponBase.WeaponType.Shotgun)
                {
                    ChangeActiveWeapon(WeaponBase.WeaponType.Shotgun);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && Inventory.Instance.IsWeaponInInventory(WeaponBase.WeaponType.MachineGun))
            {
                if (_activeWeapon.weaponType != WeaponBase.WeaponType.MachineGun )
                {
                    ChangeActiveWeapon(WeaponBase.WeaponType.MachineGun);
                }
            }
        }
    }

    /// <summary>
    /// Called when player picks up/hits a weapon
    /// </summary>
    /// <param name="weapon"></param>
    public void OnWeaponPickup(WeaponPickup weapon)
    {
        SoundManager.Instance.PlayWeaponPickup(weapon.AudioClipPickup);
        ChangeActiveWeapon(weapon.weaponType);

    }


    int _prevDelayedCallWeaponChange = -1;
    bool _isWeaponBeingSwitched = false;
    public bool IsWeaponBeingSwitched
    {
        get
        {
            return _isWeaponBeingSwitched;
        }
    }
    /// <summary>
    /// Function to make the player switch to another weapon
    /// </summary>
    private void ChangeActiveWeapon(WeaponBase.WeaponType weaponType)
    {
        

        _isWeaponBeingSwitched = true;
        AnimatorStates.Set(AnimatorStates.AnimationParameter.Unwield, _activeWeapon.weaponType);
        SoundManager.Instance.PlayWeaponChange(_activeWeapon.audioClipUnwield);
        if(_prevDelayedCallWeaponChange!=-1)
        {
            DelayedCaller.Instance.RemoveDelayedCall(_prevDelayedCallWeaponChange);
        }
        _prevDelayedCallWeaponChange = DelayedCaller.Instance.AddDelayedCall(() =>
        {
            _isWeaponBeingSwitched = false;
            _weaponSprites[(int)_activeWeapon.weaponType - 1].SetActive(false);
            _activeWeapon = WeaponsManager.Instance.GetWeapon(weaponType);
            _weaponSprites[(int)weaponType - 1].SetActive(true);
            SoundManager.Instance.PlayWeaponChange(_activeWeapon.audioClipWield);
        }, _weaponUnwieldTime);

    }

    void OnDestroy()
    {
        s_instance = null;
    }
}
