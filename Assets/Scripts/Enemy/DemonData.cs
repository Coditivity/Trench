using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonData : MonoBehaviour {

    private static DemonData s_instance = null;
    public static DemonData Instance
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
    public int maxHealth = 80;
    /// <summary>
    /// The health value at/below which the overkill animation will be played
    /// </summary>
    public float overkillHealth = -11;
    public AudioClip audioClipHit = null;
    public AudioClip audioClipDeath = null;

    void Awake()
    {
        s_instance = this;
    }


    void Start()
    {

    }
	
}
