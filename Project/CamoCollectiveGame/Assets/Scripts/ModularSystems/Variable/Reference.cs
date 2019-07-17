
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Reference<TType, TValue> where TValue : Variable<TType>
{
    [SerializeField]
    private bool useConstant = true;
    [SerializeField]
    private TType constant;
    [SerializeField]
    private TValue variable = null;
    public TType Value
    {
        get
        {
            return (useConstant) ? constant : variable.value;
        }
    }
}
