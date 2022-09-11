using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SerializedValue<T> : RuntimeValue<T>
{
    public T StaticValue;

    private void Awake()
    {
        InitializeRuntimeValue();
    }

    private void InitializeRuntimeValue()
    {
        runtimeValue = StaticValue;
    }
}
