using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedValue<T> : RuntimeValue<T>
{
    public T scriptableObjectValue;
}
