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
    public float spread = .2f;
    public int projectilePerShot = 1;
    public int ammoCapacity = 10;
    /// <summary>
    /// If enabled, will fire weapon automatically if fire button is held
    /// </summary>
    public bool autoFire = true;
    public float swayIntensity = 1;
    public AudioClip audioClipWield;
    public AudioClip audioClipUnwield;

    
}
