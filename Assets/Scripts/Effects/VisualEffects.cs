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
	

    
    /// <summary>
    /// Called when the player attacks
    /// </summary>
    private void OnPrimaryAttackCallBack()
    {

        AnimatorStates.Set(PlayerMain.Instance.ActiveWeapon.cameraShakeAnimationParameter, _mainCameraAnimator);

        GameObject smoke = VisualEffectsManager.Instance.GetSmokePoolObject();
        // smoke.SetActive(true);
        //  smoke.transform.position = smokeEffectPosition.transform.position;
        //  smoke.GetComponent<ParticleSystem>().Play();
        WeaponBase activeWeapon = PlayerMain.Instance.ActiveWeapon;
        GameObject smokeInstance = Instantiate(smoke);
        smokeInstance.SetActive(true);
        smoke.transform.parent = activeWeapon.smokeSpawnPoint.transform.parent;
        smoke.transform.position = activeWeapon.smokeSpawnPoint.transform.position;
        ParticleSystem ps = smoke.GetComponent<ParticleSystem>();
        ps.Play();
        Destroy(smokeInstance, ps.main.duration);


        GameObject muzzleFlash = VisualEffectsManager.Instance.GetMuzzleFlashObject();
        muzzleFlash.SetActive(true);
        muzzleFlash.transform.parent = activeWeapon.muzzleFlashSpawnPoint.transform.parent;
        muzzleFlash.transform.position = activeWeapon.muzzleFlashSpawnPoint.transform.position;

        muzzleFlash.GetComponent<ParticleSystem>().Play();



    }

    /// <summary>
    /// CallBack received the when the player attack hits something
    /// </summary>
    void OnPrimaryAttackHitCallBack(RaycastHit bulletRayCastHit)
    {
        GameObject decal = VisualEffectsManager.Instance.GetDecalSprite(bulletRayCastHit.collider.gameObject);
        if(decal!=null)
        {
            decal.SetActive(true);
            decal.transform.forward = bulletRayCastHit.normal;
            decal.transform.position = bulletRayCastHit.point + decal.transform.forward * .01f;
            decal.GetComponent<ParticleSystem>().Play();
        }

      

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
