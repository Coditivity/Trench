using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectsManager : MonoBehaviour {


    private static VisualEffectsManager s_instance = null;
    public static VisualEffectsManager Instance
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

    void Awake()
    {
        s_instance = this;
    }
    /// <summary>
    /// Holds the tag-decalSprite data pair
    /// </summary>
    [System.Serializable]
    public class DecalData
    {
        public string gameObjectTag;
        public GameObject[] decalSprites;
    }

    
    [SerializeField]
    private DecalData[] _decalDatas = null;    
    [SerializeField]
    private int _decalPoolSize = 20;

    /// <summary>
    /// Source gameObject/prefab that will be used to spawn muzzleFlash instances
    /// </summary>
    [SerializeField]
    private GameObject _muzzleFlashSource = null;
    [SerializeField]
    private int _muzzleFlashPoolSize = 20;

    /// <summary>
    /// Source gameObject/prefab that will be used to spawn smoke instances
    /// </summary>
    [SerializeField]
    private GameObject _smokeSource = null;
    [SerializeField]
    private int _smokePoolSize = 20;




    GameObjectPool _decalPool = null;
    GameObjectPool _muzzleFlashPool = null;
    GameObjectPool _smokePool = null;

    private const string MuzzleFlashTag = "mf";
    private const string SmokeTag = "s";

    int _indexOffset = 0;
	// Use this for initialization
	void Start () {



        _decalPool = new GameObjectPool(_decalPoolSize);
        foreach(DecalData dd in _decalDatas)
        {
            _decalPool.AddTag(dd.gameObjectTag, dd.decalSprites[0]);
        }
        _muzzleFlashPool = new GameObjectPool(_muzzleFlashPoolSize);       
        _muzzleFlashPool.AddTag(MuzzleFlashTag, _muzzleFlashSource);
        _smokePool = new GameObjectPool(_smokePoolSize);
        _smokePool.AddTag(SmokeTag, _smokeSource);




    }
	

    /// <summary>
    /// Returns a randomly selected decal sprite for the given object
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public GameObject GetDecalSprite(GameObject gameObject)
    {
        //int random = Random.Range(0, _decalSpritesInstanceIdIndexed[gameObject.GetInstanceID()].Length);
        //return _decalSpritesPoolInstanceIdIndexed[gameObject.GetInstanceID()].getNext();
        return _decalPool.GetGameObjectFromPool(gameObject.tag);
    }

    public GameObject GetSmokePoolObject()
    {
        return _smokePool.GetGameObjectFromPool(SmokeTag);
    }

    public GameObject GetMuzzleFlashObject()
    {
        return _muzzleFlashPool.GetGameObjectFromPool(MuzzleFlashTag);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
