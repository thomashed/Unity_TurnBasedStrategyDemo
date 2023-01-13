using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolBeans.Grid
{
    public class GridSystem<TGridObject>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        private float cellSize;
        private TGridObject[,] gridObjects; // 2d array for storing gridObjects

        public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
        {
            this.Width = width;
            this.Height = height;
            this.cellSize = cellSize;
            this.gridObjects = new TGridObject[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    var gridPos = new GridPosition(x,z);
                    var gridObject = createGridObject(this, gridPos); // use Func to instantiate GridObject
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

        public TGridObject GetGridObject(GridPosition gridPosition)
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

            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Height; z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    var debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                    debugTransform.parent = debugObjectsParent.transform; // avoid bloating the hierachy with debugObjects
                    debugTransform.name = $"DebugObject_{x},{z}";
                    var gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                    gridDebugObject.SetGridObject(GetGridObject(gridPosition) as GridObject); // we explicitly cast this for now
                 }
            }
#endif
        }

        public bool IsValidGridPosition(GridPosition gridPosition)
        {
            return gridPosition.X >= 0 && 
                   gridPosition.Z >= 0 && 
                   gridPosition.X < Width && 
                   gridPosition.Z < Height;
        }
        

    }
}


