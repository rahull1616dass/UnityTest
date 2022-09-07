using System.Diagnostics.Tracing;
using UnityEngine;

public class RuntimeValue<T> : ScriptableObject, IValue<T>
{
    protected T runtimeValue;

    public event IValue<T>.ValueEvent ValueChanged;

    public T Value
    {
        get => runtimeValue;
        set
        {
            SetValueWithSource(null, value);
        }
    }

    public bool SetValueWithSource(EventSource source, T newValue)
    {
        var oldValue = runtimeValue;
        runtimeValue = newValue;
        InvokeValueChanged(source, oldValue, newValue);
        return true;
    }

    public void SetValueWithoutNotify(T newVal)
    {
        runtimeValue = newVal;
    }

    private void InvokeValueChanged(EventSource source, T oldValue, T newValue)
    {
        ValueChanged?.Invoke(source, oldValue, newValue);
    }


}
