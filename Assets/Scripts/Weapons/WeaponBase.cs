using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponBase { 

    /// <summary>
    /// Enum for all the weapon types. Integer values represent the ranks
    /// </summary>
    public enum WeaponType
    {
        Pistol=1,
        Shotgun=2,
        MachineGun=3
    }
    
    /// <summary>
    /// Type of this weapon
    /// </summary>
    public WeaponType weaponType = WeaponType.Pistol;
    /// <summary>
    /// Represents number of shots per second
    /// </summary>
    public float fireRate = 3f;

    /// <summary>
    /// Amount of damage done by the weapon per projectile. Multiply this with projectile per shot to get the effective damage per shot
    /// </summary>
    public int damage = 1;
    /// <summary>
    /// Returns the time taken for a shot to complete
    /// </summary>
    public float fireCoolDown
    {
        get
        {
            return 1f / fireRate;
        }
    }
    public float spread = .2f;
    public int projectilePerShot = 1;
    public int ammoCapacity = 10;
    /// <summary>
    /// If enabled, will fire weapon automatically if fire button is held
    /// </summary>
    public bool autoFire = true;
    public float swayIntensity = 1;
    public GameObject[] shellObjects = null;
    public GameObject shellEjectionPoint;
    /// <summary>
    /// The direction in which the shell is ejected is calculated from shellEjectionPoint and this
    /// </summary>
    public GameObject shellEjectionTowardsDirection;
    public float shellEjectionForceMagnitudeMin;
    public float shellEjectionForceMagnitudeMax;
    public float shellEjectDelay = 0;
    public GameObject smokeSpawnPoint;
    public GameObject muzzleFlashSpawnPoint;
    public AudioClip audioClipWield;
    public AudioClip audioClipUnwield;
    public AudioClip audioClipFire;
    public AudioSource audioSourceFire;
    public int shellPoolSize = 50;
    public LList<Shell> shellPool;
    public AnimatorStates.AnimationParameter cameraShakeAnimationParameter;

    public class Shell
    {
        public GameObject gameObject;
        public Rigidbody rigidBody;
        public Shell() { }
        public Shell(GameObject gameObject)
        {
            this.gameObject = gameObject;
            rigidBody = gameObject.GetComponent<Rigidbody>(); //cache the gameobject's rigidbidy for faster access
        
        }
    }

    const string NoShellErrorMessage = "No shell eject prefab was given for the weapon ";
    public void Init()
    {

        shellPool = new LList<Shell>(shellPoolSize);
        if (shellObjects == null )
        {
            Debug.LogError(NoShellErrorMessage + weaponType);
            return;
        }
        if(shellObjects.Length<=0)
        {
            Debug.LogError(NoShellErrorMessage + weaponType);
            return;
        }
        for (int i=0;i<shellPoolSize;i++)
        {
            int randomIndex = Random.Range(0, shellObjects.Length);
            GameObject shellObjectInstance = GameObject.Instantiate(shellObjects[randomIndex]);
            shellObjectInstance.SetActive(false);
            Shell shell = new Shell(shellObjectInstance);
            shellPool.AddNext(shell);
        }
    }
}
