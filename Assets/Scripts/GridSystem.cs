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

        public GridSystem(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Debug.DrawLine(GetWorldPosition(x,z), GetWorldPosition(x, z) + Vector3.right * 0.4f, Color.green, Mathf.Infinity);
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

    }
}


