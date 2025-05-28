using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject<T> : MonoBehaviour where T : PooledObject<T>
{
    public ObjectPool<T> ObjPool { get; private set; }

    public void PooledInit(ObjectPool<T> objPool)
    {
        ObjPool = objPool;
    }

    public void ReturnPool()
    {
        ObjPool.PushPool(this as T);
    }
}
