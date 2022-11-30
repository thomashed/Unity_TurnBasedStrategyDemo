using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjcetPrefab = null;

    private GridSystem grid;

    private void Awake()
    {
        grid = new GridSystem(10, 10,  2f);
        grid.CreateDebugObjects(gridDebugObjcetPrefab);

    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        print(grid.GetGridPosition(MouseWorld.GetMousePosition()));
    }
}
