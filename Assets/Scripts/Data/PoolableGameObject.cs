using UnityEngine;

public class PoolableGameObject {
    public GameObject gameObject = null;

    public PoolableGameObject()
    {

    }

    public PoolableGameObject(GameObject gameobject)
    {
        this.gameObject = gameobject;
    }
}
