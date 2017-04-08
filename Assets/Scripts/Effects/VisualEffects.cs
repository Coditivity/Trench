using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffects : MonoBehaviour {


   /* [SerializeField]
    private GameObject smokeEffectPosition = null;
    [SerializeField]
    private GameObject muzzleFlashPosition = null;*/
    [SerializeField]
    private Animator _mainCameraAnimator = null;

	// Use this for initialization
	void Start () {
        PlayerAttack.Instance.OnPrimaryAttackHitListener.AddListener(OnPrimaryAttackHitCallBack);
        PlayerAttack.Instance.OnPrimaryAttackListener.AddListener(OnPrimaryAttackCallBack);
        PlayerAttack.Instance.OnPrimaryAttackEndListener.AddListener(OnPrimaryAttackEndCallBack);
    }



    GameObject prevSmoke = null;
    /// <summary>
    /// Called when the player attacks
    /// </summary>
    private void OnPrimaryAttackCallBack()
    {

        AnimatorStates.Set(PlayerMain.Instance.ActiveWeapon.cameraShakeAnimationParameter, _mainCameraAnimator);

        GameObject smoke = VisualEffectsManager.Instance.GetSmokePoolObject();
       
        prevSmoke = smoke;
        // smoke.SetActive(true);
        //  smoke.transform.position = smokeEffectPosition.transform.position;
        //  smoke.GetComponent<ParticleSystem>().Play();
        WeaponBase activeWeapon = PlayerMain.Instance.ActiveWeapon;
        /*    GameObject smokeInstance = Instantiate(smoke);
            smokeInstance.SetActive(true);
            smokeInstance.transform.parent = activeWeapon.smokeSpawnPoint.transform.parent;
            smokeInstance.transform.position = activeWeapon.smokeSpawnPoint.transform.position;*/
        smoke.SetActive(true);
        ///smoke.transform.parent = activeWeapon.smokeSpawnPoint.transform.parent;
        smoke.transform.position = activeWeapon.smokeSpawnPoint.transform.position;
        ParticleSystem ps = smoke.GetComponent<ParticleSystem>();
        //ps.gameObject.SetActive(true);
        ps.Play();
        
        DelayedCaller.Instance.AddDelayedCall(() => { ps.Stop(); /*ps.Clear();*/ },  ps.main.duration/160f);
        //Destroy(smokeInstance, ps.main.duration/20);


        GameObject muzzleFlash = VisualEffectsManager.Instance.GetMuzzleFlashObject();
        muzzleFlash.SetActive(true);
        muzzleFlash.transform.parent = activeWeapon.muzzleFlashSpawnPoint.transform.parent;
        muzzleFlash.transform.position = activeWeapon.muzzleFlashSpawnPoint.transform.position;

        muzzleFlash.GetComponent<ParticleSystem>().Play();



    }

   

    /// <summary>
    /// CallBack received the when the player attack hits something
    /// </summary>
    void OnPrimaryAttackHitCallBack(AttackHitData attackHitData)
    {
        GameObject decal = VisualEffectsManager.Instance.GetDecalSprite(attackHitData.raycastHit.collider.gameObject);
        if(decal!=null)
        {
            decal.SetActive(true);
            decal.transform.forward = attackHitData.raycastHit.normal;
            decal.transform.position = attackHitData.raycastHit.point + decal.transform.forward * .01f;
            decal.GetComponent<ParticleSystem>().Play();
        }
      /*  GameObject testSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        testSphere.transform.localScale = Vector3.one * .05f;
        testSphere.transform.position = bulletRayCastHit.point;*/

      

    }


    private void OnPrimaryAttackEndCallBack()
    {
        if (PlayerMain.Instance.ActiveWeapon.weaponType == WeaponBase.WeaponType.MachineGun) {
            AnimatorStates.UnSet(PlayerMain.Instance.ActiveWeapon.cameraShakeAnimationParameter, _mainCameraAnimator);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
    void LateUpdate()
    {

    }

    void OnDestroy()
    {
        if (PlayerAttack.Instance != null)
        {
            PlayerAttack.Instance.OnPrimaryAttackHitListener.RemoveListener(OnPrimaryAttackHitCallBack);
            PlayerAttack.Instance.OnPrimaryAttackListener.RemoveListener(OnPrimaryAttackCallBack);
            PlayerAttack.Instance.OnPrimaryAttackEndListener.RemoveListener(OnPrimaryAttackEndCallBack);
        }
    }
}
