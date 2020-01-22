using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Poolitem : MonoBehaviour
{
    ObjectPool pool;
    public ObjectPool ObjectPool { set { pool = value; } }

    protected abstract void Reset();

    public virtual void Spawn(Vector3 position, Quaternion rotation, Transform parent)
    {
        Reset();
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = rotation;
        transform.parent = parent;
    }

    public virtual void Return()
    {
        pool.AddItem(this);
    }
}
