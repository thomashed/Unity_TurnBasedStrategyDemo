using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private GridSystem grid;

    private void Awake()
    {
        grid = new GridSystem(10, 10,  2f);
        

    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        print(grid.GetGridPosition(MouseWorld.GetMousePosition()));
    }
}
