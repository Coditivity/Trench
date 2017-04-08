using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Characters.FirstPerson;

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
    [SerializeField]
    private FirstPersonController _firstPersonController = null;

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
            AnimatorStates.ResetTrigger(AnimatorStates.AnimationParameter.FireStop, PlayerMain.Instance.ActiveWeapon.weaponType);
            AnimatorStates.Set(AnimatorStates.AnimationParameter.Fire, PlayerMain.Instance.ActiveWeapon.weaponType);

            /*   WeaponBase activePlayerWeapon = PlayerMain.Instance.ActiveWeapon;
               SoundManager.Instance.PlayAudio(activePlayerWeapon.audioSourceFire, activePlayerWeapon.audioClipFire);
               for (int i=0;i<PlayerMain.Instance.ActiveWeapon.projectilePerShot;i++) {
                   FireWeapon();
               }*/
            OnPrimaryAttackHeld();
        }
       
    }

    private void FireWeapon(bool bLastProjectile)
    {
       // _cameraAnimator.SetTrigger("Fire");
        _timeAtPrimaryAttack = Time.time; 
        RayCastBullet(bLastProjectile); //ray cast and fire bullet hit event
     
        //Debug.LogError(PlayerMain.Instance.GetInstantaenousVelocity());
      //  Debug.LogError((activePlayerWeapon.shellEjectionForceMagnitude + PlayerMain.Instance.GetInstantaenousVelocity() * 600));
       // shell.rigidBody.velocity += PlayerMain.Instance.GetInstantaenousVelocity();
        
        
    }


    private UnityEvent _onPrimaryAttackListener = new UnityEvent();  
    public UnityEvent OnPrimaryAttackListener
    {
        get
        {
            return _onPrimaryAttackListener;
           
        }
        private set
        {
            _onPrimaryAttackListener = value;
        }
    }


    
    /// <summary>
    /// Event that will be fired when the bullet hits something
    /// </summary>
    public class PrimaryAttackHitEvent : UnityEvent<AttackHitData> { }
    private PrimaryAttackHitEvent _onRayHitListener = new PrimaryAttackHitEvent();
    public PrimaryAttackHitEvent OnPrimaryAttackHitListener
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
    private void RayCastBullet(bool bLastProjectile)
    {
        
        if((_crossHairRectTransform.anchorMax - _crossHairRectTransform.anchorMin).magnitude > .01f)
        {
            Debug.LogError("The crosshair sprite or it's parent is set to stretch in the UI. It shouldn't be");
        }
        float cursorX = _crossHairRectTransform.anchorMax.x * Screen.width
            + _crossHairRectTransform.anchoredPosition.x;
        float cursorY = _crossHairRectTransform.anchorMax.y * Screen.height
            + _crossHairRectTransform.anchoredPosition.y;
        float spreadX = Random.Range(-PlayerMain.Instance.ActiveWeapon.spread, PlayerMain.Instance.ActiveWeapon.spread);//get a random spread value limited by the weapon's max spread
        float spreadY = Random.Range(-PlayerMain.Instance.ActiveWeapon.spread, PlayerMain.Instance.ActiveWeapon.spread);//get a random spread value limited by the weapon's max spread
 
        
        Vector3 cursorScreenPosition = new Vector3(cursorX, cursorY, 0) //add the spread to the cursor position to create inaccuracy
            + new Vector3(spreadX, spreadY, 0);
        Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);
        if (Physics.Raycast(ray, out bulletRayCastHit))
        {
            
            OnPrimaryAttackHitListener.Invoke(new AttackHitData(bulletRayCastHit, PlayerMain.Instance.ActiveWeapon.damage, bLastProjectile));
         /*   GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.localScale = Vector3.one * .2f;
            g.transform.position = bulletRayCastHit.point;*/
          //  Debug.LogError(VisualEffectsManager.Instance.GetDecalSprite(bulletRayCastHit.collider.gameObject));
        }
    }


    private void EjectShell()
    {
        WeaponBase activePlayerWeapon = PlayerMain.Instance.ActiveWeapon;
        WeaponBase.Shell shell = activePlayerWeapon.shellPool.getNext();// GameObject.Instantiate(activePlayerWeapon.shellObject);
        shell.gameObject.SetActive(true);
        float ejectMagnitude = Random.Range(activePlayerWeapon.shellEjectionForceMagnitudeMin
            , activePlayerWeapon.shellEjectionForceMagnitudeMax);
        Vector3 shellDirection = (activePlayerWeapon.shellEjectionTowardsDirection.transform.position
            - activePlayerWeapon.shellEjectionPoint.transform.position).normalized
            * ejectMagnitude;
        // + PlayerMain.Instance.GetPlayerMoveDirection();
        shell.gameObject.transform.position = activePlayerWeapon.shellEjectionPoint.transform.position;
        //shell.gameObject.transform.parent = _ammoShellParent.transform.parent;
        /*  shell.rigidBody.AddForce
              (shellDirection
              
              + PlayerMain.Instance.GetInstantaenousVelocity() * 700
              )
              );*/

        Vector3 shellVelocity = shellDirection
            + (_firstPersonController.GetInstantaneousVelocity());


        //   * activePlayerWeapon.shellEjectionForceMagnitude ;

        shell.rigidBody.velocity = shellVelocity;

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
        WeaponBase activePlayerWeapon = PlayerMain.Instance.ActiveWeapon;
        SoundManager.Instance.PlayAudio(activePlayerWeapon.audioSourceFire, activePlayerWeapon.audioClipFire);

        _onPrimaryAttackListener.Invoke();
        


       
        for (int i = 0; i < PlayerMain.Instance.ActiveWeapon.projectilePerShot; i++)
        {
            bool bLastProjectile = (i == PlayerMain.Instance.ActiveWeapon.projectilePerShot - 1) ? true : false;
            FireWeapon(bLastProjectile);
        }
        DelayedCaller.Instance.AddDelayedCall(EjectShell, activePlayerWeapon.shellEjectDelay);
    

    }


    private UnityEvent _onPrimaryAttackEndListener = new UnityEvent();
    public UnityEvent OnPrimaryAttackEndListener
    {
        get
        {
            return _onPrimaryAttackEndListener;
        }
        private set
        {
            _onPrimaryAttackEndListener = value;
        }
    }
    public void OnPrimaryAttackEnd()
    {
        _bIsPrimaryAttackKeyHeld = false;
        AnimatorStates.Set(AnimatorStates.AnimationParameter.FireStop, PlayerMain.Instance.ActiveWeapon.weaponType);
        AnimatorStates.ResetTrigger(AnimatorStates.AnimationParameter.Fire, PlayerMain.Instance.ActiveWeapon.weaponType);
        _onPrimaryAttackEndListener.Invoke();
    }
    
    

	// Update is called once per frame
	void LateUpdate () {
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
