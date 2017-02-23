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
        _prevPosition = transform.position;
	}



    public Vector3 GetPlayerMoveDirection()
    {
        return (transform.position - _prevPosition).normalized;
        
    }

    /// <summary>
    /// Returns the player's velocity in the previous frame
    /// </summary>
    /// <returns></returns>
    public Vector3 GetInstantaenousVelocity()
    {
        //get the direction of movement
        //float vel = dir.magnitude;// * Mathf.Sign(Vector3.Dot(transform.right, dir)); //return the signed velocity magnitude
        //return vel;
        if(_lastTimeDelta == 0)
        {
            return Vector3.zero;
        }
        //   Debug.LogError("td>>" + _lastTimeDelta);
        return _lastVelocity / _lastTimeDelta;
       // return transform.position - _prevPosition;
        
    }

    /// <summary>
    /// Stores the last position of the player
    /// </summary>
    Vector3 _prevPosition = Vector3.zero;

    Vector3 _lastVelocity = Vector3.zero;
    float _lastTimeDelta = 1f;
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
        Vector3 dir = (transform.position - _prevPosition).normalized;
        float vel = (transform.position - _prevPosition).magnitude * Mathf.Sign(Vector3.Dot(transform.right, dir));

        _lastVelocity = (transform.position - _prevPosition);
        _lastTimeDelta = Time.deltaTime;
        _prevPosition = transform.position;
        
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
        PlayerAttack.Instance.OnPrimaryAttackEnd();
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
