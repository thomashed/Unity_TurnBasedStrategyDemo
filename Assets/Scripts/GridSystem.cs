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
        public Vector3 GetWorldPosition(int x, int z) 
        {
            var worldPosition = new Vector3(x, 0, z) * cellSize;
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

        public void CreateDebugObjects(Transform debugPrefab)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GameObject.Instantiate(debugPrefab, GetWorldPosition(x,z), Quaternion.identity);
                }
            }
        }

    }
}


