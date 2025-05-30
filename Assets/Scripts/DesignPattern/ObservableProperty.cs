using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObservableProperty<T>
{
    [SerializeField] private T _value;
    public T Value
    {
        get => _value;
        set
        {
            if (_value.Equals(value)) return;
            _value = value;
            Notify();
        }
    }
    private UnityEvent<T> _onValueChanged = new();
    private UnityEvent _onValueChangedNoParam = new();

    public ObservableProperty(T value = default)
    {
        _value = value;
    }

    public void Subscribe(UnityAction<T> action)
    {
        _onValueChanged.AddListener(action);
    }

    public void Subscribe(UnityAction action)
    {
        _onValueChangedNoParam.AddListener(action);
    }

    public void Unsubscribe(UnityAction<T> action)
    {
        _onValueChanged.RemoveListener(action);
    }

    public void Unsubscribe(UnityAction action)
    {
        _onValueChangedNoParam.RemoveListener(action);
    }

    public void UnsbscribeAll()
    {
        _onValueChanged.RemoveAllListeners();
        _onValueChangedNoParam.RemoveAllListeners();
    }

    private void Notify()
    {
        _onValueChanged?.Invoke(Value);
        _onValueChangedNoParam?.Invoke();
    }
}
