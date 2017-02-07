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
    public GameObject shellObject = null;
    public GameObject shellEjectionPoint;
    /// <summary>
    /// The direction in which the shell is ejected is calculated from shellEjectionPoint and this
    /// </summary>
    public GameObject shellEjectionTowardsDirection;
    public float shellEjectionForceMagnitude;
    public AudioClip audioClipWield;
    public AudioClip audioClipUnwield;
    public int shellPoolSize = 50;
    public LList<Shell> shellPool;

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

    public void Init()
    {

        shellPool = new LList<Shell>(shellPoolSize);
        if (shellObject == null)
        {
            return;
        }
        for (int i=0;i<shellPoolSize;i++)
        {
            
            GameObject shellObjectInstance = GameObject.Instantiate(shellObject);
            shellObjectInstance.SetActive(false);
            Shell shell = new Shell(shellObjectInstance);
            shellPool.AddNext(shell);
        }
    }
}
