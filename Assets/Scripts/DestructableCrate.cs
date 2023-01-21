using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{

    public static event EventHandler AnyDestroyed;

    public GridPosition GridPosition { get; private set; }

    private void Start()
    {
        GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        Destroy(gameObject);
        OnAnyDestroyed();
    }

    private void OnAnyDestroyed()
    {
        AnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

}
