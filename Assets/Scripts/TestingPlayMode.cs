using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPlayMode : MonoBehaviour 
{
    [SerializeField] private Unit unit = null;


    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            print("TESTING: ");
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            print($"TESTING: Mouse grid position: {mouseGridPosition}");

            GridPosition startGridPosition = new GridPosition(0,0);

            List<GridPosition> gridPositionList = PathFinding.Instance.FindPath(startGridPosition, mouseGridPosition);

            for (int i = 0; i < gridPositionList.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
                    Color.magenta,
                    10f
                );   
            }
        }
    }

}
