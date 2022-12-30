using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    protected Unit unit;
    protected bool isActive = false;

    protected virtual void Awake()
    {
        this.unit = GetComponent<Unit>();
    }




}
