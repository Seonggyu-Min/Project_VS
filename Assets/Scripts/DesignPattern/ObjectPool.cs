using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : PooledObject<T>
{
    private Stack<T> _stack;
    private T _targetPrefab;
    private GameObject _poolObject;

    public ObjectPool(Transform parent, T targetPrefab, int initSize = 5)
    {
        Init(parent, targetPrefab, initSize);
    }

    private void Init(Transform parent, T targetPrefab, int initSize)
    {
        _stack = new Stack<T>(initSize);
        _targetPrefab = targetPrefab;
        _poolObject = new GameObject($"{targetPrefab.name} Pool");
        _poolObject.transform.SetParent(parent);

        for (int i = 0; i < initSize; i++)
            CreatePooledObject();
    }

    private void CreatePooledObject()
    {
        T obj = GameObject.Instantiate(_targetPrefab);
        obj.PooledInit(this);
        PushPool(obj);
    }

    public T PopPool()
    {
        if (_stack.Count == 0)
            CreatePooledObject();

        T obj = _stack.Pop();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void PushPool(T target)
    {
        target.transform.SetParent(_poolObject.transform);
        target.gameObject.SetActive(false);
        _stack.Push(target);
    }
}
