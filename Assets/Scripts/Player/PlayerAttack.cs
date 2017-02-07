using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour {
    private static PlayerAttack s_instance = null;
    public static PlayerAttack Instance
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
    private GameObject _crossHairObject = null;
    void Awake()
    {
        s_instance = this;
    }

    RectTransform _crossHairRectTransform = null;
	// Use this for initialization
	void Start () {
        InputControls.Instance.OnPrimaryAttackStartEventListener.AddListener(OnPrimaryAttackStart);
        InputControls.Instance.OnPrimaryAttackEndEventListener.AddListener(OnPrimaryAttackEnd);
        _crossHairRectTransform = _crossHairObject.GetComponent<RectTransform>();
        if ((_crossHairRectTransform.anchorMax - _crossHairRectTransform.anchorMin).magnitude > .01f)
        {
            Debug.LogError("The crosshair sprite or it's parent is set to stretch in the UI. It shouldn't be");
        }
        
    }



    /// <summary>
    /// Returns the time elapsed between the last primary attack and the current time stamp
    /// </summary>
    private float _PrimaryAttackProgress
    {
        get
        {
            return Mathf.Abs(Time.time - _timeAtPrimaryAttack);
        }
    }

    bool _bIsPrimaryAttackKeyHeld = false;
    /// <summary>
    /// Time stamp at the last primary attack
    /// </summary>
    float _timeAtPrimaryAttack = -1;
    /// <summary>
    /// Will be triggered whenever the left mouse button is clicked
    /// </summary>
    private void OnPrimaryAttackStart()
    {
        if(_PrimaryAttackProgress <= PlayerMain.Instance.ActiveWeapon.fireCoolDown && _timeAtPrimaryAttack!=-1)
        {
            return;
        }
        if(!PlayerMain.Instance.IsWeaponBeingSwitched)
        {
            _bIsPrimaryAttackKeyHeld = true;
            AnimatorStates.Set(AnimatorStates.AnimationParameter.Fire, PlayerMain.Instance.ActiveWeapon.weaponType);
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        _timeAtPrimaryAttack = Time.time; 
        RayCastBullet(); //ray cast and fire bullet hit event
        WeaponBase activePlayerWeapon = PlayerMain.Instance.ActiveWeapon;

        WeaponBase.Shell shell = activePlayerWeapon.shellPool.getNext();// GameObject.Instantiate(activePlayerWeapon.shellObject);
      
        if(shell == null)
        {
            return;
        }
        shell.gameObject.SetActive(true);
        Vector3 shellDirection = (activePlayerWeapon.shellEjectionTowardsDirection.transform.position
            - activePlayerWeapon.shellEjectionPoint.transform.position).normalized;
        shell.gameObject.transform.position = activePlayerWeapon.shellEjectionPoint.transform.position;
        shell.rigidBody.AddForce
            (shellDirection * activePlayerWeapon.shellEjectionForceMagnitude);
        
        
    }

    /// <summary>
    /// Event that will be fired when the bullet hits something
    /// </summary>
    public class RayHitEvent : UnityEvent<RaycastHit> { }
    private RayHitEvent _onRayHitListener = new RayHitEvent();
    public RayHitEvent OnRayHitListener
    {
        get
        {
            return _onRayHitListener;
        }
        private set
        {
            _onRayHitListener = value;
        }
    }
    



    

    RaycastHit bulletRayCastHit;
    /// <summary>
    /// Check if the bullet collides with something, and if it does fire an event
    /// </summary>
    private void RayCastBullet()
    {
        
        if((_crossHairRectTransform.anchorMax - _crossHairRectTransform.anchorMin).magnitude > .01f)
        {
            Debug.LogError("The crosshair sprite or it's parent is set to stretch in the UI. It shouldn't be");
        }
        float cursorX = _crossHairRectTransform.anchorMax.x * Screen.width
            + _crossHairRectTransform.anchoredPosition.x;
        float cursorY = _crossHairRectTransform.anchorMax.y * Screen.height
            + _crossHairRectTransform.anchoredPosition.y;
        Vector3 cursorScreenPosition = new Vector3(cursorX, cursorY, 0);
        Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);
        if (Physics.Raycast(ray, out bulletRayCastHit))
        {
            
            OnRayHitListener.Invoke(bulletRayCastHit);
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.localScale = Vector3.one * .2f;
            g.transform.position = bulletRayCastHit.point;
        }
    }

    /// <summary>
    /// If the primary attack key was held, this will be called for all auto-attack weapons. If the fire cooldown has been reached, trigger the fire event
    /// </summary>
    private void OnPrimaryAttackHeld()
    {
        if (_PrimaryAttackProgress <= PlayerMain.Instance.ActiveWeapon.fireCoolDown && _timeAtPrimaryAttack != -1)
        {
            return;
        }
        FireWeapon();

    }

    private void OnPrimaryAttackEnd()
    {
        _bIsPrimaryAttackKeyHeld = false;
        AnimatorStates.Set(AnimatorStates.AnimationParameter.FireStop, PlayerMain.Instance.ActiveWeapon.weaponType);
    }

	// Update is called once per frame
	void Update () {
		if(_bIsPrimaryAttackKeyHeld && PlayerMain.Instance.ActiveWeapon.autoFire)
        {
            OnPrimaryAttackHeld();
        }
	}

    void OnDestroy()
    {
        s_instance = null;
    }
}
