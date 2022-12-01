using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolBeans.Grid
{
    public class GridSystem
    {
        private int width;
        private int height;
        private float cellSize;
        private GridObject[,] gridObjects; // 2d array for storing gridObjects

        public GridSystem(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.gridObjects = new GridObject[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    var gridPos = new GridPosition(x,z);
                    var gridObject = new GridObject(this, gridPos);
                    gridObjects[x, z] = gridObject;
                }
            }

        }

        // convert from worldPos to gridPos
        public Vector3 GetWorldPosition(GridPosition gridPosition) 
        {
            var worldPosition = new Vector3(gridPosition.X, 0, gridPosition.Z) * cellSize;
            worldPosition.y = 0.1f;
            return worldPosition;
        }

        public GridPosition GetGridPosition(Vector3 worldPos) 
        {
            return new GridPosition(
                Mathf.RoundToInt(worldPos.x / cellSize), 
                Mathf.RoundToInt(worldPos.z / cellSize)
                );
        }

        public GridObject GetGridObject(GridPosition gridPosition)
        {
            return gridObjects[gridPosition.X, gridPosition.Z];
        }

        public void CreateDebugObjects(Transform debugPrefab)
        {
#if UNITY_EDITOR
            var debugObjectsParent = GameObject.Find("Testing");

            if (debugObjectsParent == null)
            {
                debugObjectsParent = new GameObject("Testing");
            }

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    var debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                    debugTransform.parent = debugObjectsParent.transform; // avoid bloating the hierachy with debugObjects
                    debugTransform.name = $"DebugObject_{x},{z}";
                    var gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                    gridDebugObject.SetGridObject(GetGridObject(gridPosition));
                }
            }
#endif
        }
        

    }
}


