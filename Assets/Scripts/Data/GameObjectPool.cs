using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool  {

    Dictionary<string, LList<GameObject>> _poolDictionary = new Dictionary<string, LList<GameObject>>(1);

    int _poolSize = 0;
    public GameObjectPool(int poolSize)
    {
        _poolSize = poolSize;
    }

    public GameObjectPool(string[] tags, int poolSize, GameObject[] sourceObjects)
    {
        _poolSize = poolSize;
        foreach(string tag in tags)
        {
            LList<GameObject> pool = new LList<GameObject>(poolSize);
            for(int i=0;i<poolSize;i++)
            {
                GameObject g = GameObject.Instantiate(sourceObjects[i]);
                g.SetActive(false);
                pool.AddNext(g);
            }
            _poolDictionary.Add(tag, pool);
        }
    }

    public void AddTag(string tag, GameObject sourceObject)
    {
        LList<GameObject> pool = new LList<GameObject>(_poolSize);
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject g = GameObject.Instantiate(sourceObject);
            g.SetActive(false);
            pool.AddNext(g);
        }
        _poolDictionary.Add(tag, pool);
    }


    public GameObject GetGameObjectFromPool(string tag)
    {
        LList<GameObject> pool = null;
        _poolDictionary.TryGetValue(tag, out pool);
        if(pool == null)
        {
      //      Debug.LogError("No pool was created for the tag >>" + tag + "<<. You must create one");
            return null;
        }
        return pool.getNext();
    }

	
}
