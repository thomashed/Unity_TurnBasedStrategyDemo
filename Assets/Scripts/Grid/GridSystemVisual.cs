using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolBeans.Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        [SerializeField] private Transform gridSystemVisualPrefab = null;
        [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList = null;
        private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

        public static GridSystemVisual Instance;

        // custom struct to hold together which enum fits with which material 
        [Serializable]
        public struct GridVisualTypeMaterial 
        {
            public GridVisualType gridVisualType;
            public Material material;
        }

        public enum GridVisualType 
        {
            White, 
            Blue,
            Red,
            Yellow,
            RedSoft
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("GridSystemVisual: trying to create more than one instance!");
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.Width, LevelGrid.Instance.Height];
            for (int x = 0; x < LevelGrid.Instance.Width; x++)
            {
                for (int z = 0; z < LevelGrid.Instance.Height; z++)
                {
                    var gridPosition = new GridPosition(x,z);
                    var gridSystemVisualSingleTransform = Instantiate(gridSystemVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                    gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
                }
            }

            UnitActionSystem.Instance.SelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            LevelGrid.Instance.AnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
            UpdateGridVisual();
        }

        private void Update()
        {
            
        }

        private void UpdateGridVisual() 
        {
            HideAllGridPosition();
            var selectedAction = UnitActionSystem.Instance.SelectedAction;
            var selectedUnit = UnitActionSystem.Instance.SelectedUnit;
            if (selectedUnit == null) return;
            if (selectedAction == null) return;
            var gridPositionList = selectedAction.GetValidActionGridPositionList();
            
            // find out what the action is, so we can choose the right colour for the gridVisual
            GridVisualType gridVisualType;
            switch (selectedAction)
            {
                default:
                case MoveAction moveAction:
                    gridVisualType = GridVisualType.White;
                    break;
                case ShootAction shootAction:
                    gridVisualType = GridVisualType.Red;
                    ShowGridPositionRange(selectedUnit.GridPosition, shootAction.MaxShootDistance, GridVisualType.RedSoft);
                    break;
                case SpinAction spinAction:
                    gridVisualType = GridVisualType.Blue;
                    break;
            }

            ShowGridPositionList(gridPositionList, gridVisualType);
        }

        public void HideAllGridPosition() 
        {
            foreach (var gridSystemVisualSingle in gridSystemVisualSingleArray)
            {
                gridSystemVisualSingle.Hide();
            }
        }

        // will show range of current action. Separate method call, as range is different material than the action itself, e.g. shootAction
        // range is a redSoft, whereas the gridPosition where there's a valid target, is solid red
        private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
        {
            List<GridPosition> gridPositionList = new List<GridPosition>();

            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {
                    GridPosition testGridPosition = gridPosition + new GridPosition(x,z);

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                    if (testDistance > range) continue; // to avoid showing range in a square

                    gridPositionList.Add(testGridPosition); 
                }
            }

            ShowGridPositionList(gridPositionList, GridVisualType.RedSoft);
        }

        public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
        {
            var colourForGridPosition = GetGridVisualTypeMaterial(gridVisualType);
            foreach (var gridPosition in gridPositionList)
            {
                gridSystemVisualSingleArray[gridPosition.X, gridPosition.Z].Show(colourForGridPosition);
            }
        }

        private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
        {
            foreach (var availableVisualType in gridVisualTypeMaterialList)
            {
                if (availableVisualType.gridVisualType == gridVisualType)
                {
                    return availableVisualType.material;
                }
            }
            Debug.Log("Couldn't find material for: " + gridVisualType);
            return null;
        }

        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) 
        {
            UpdateGridVisual();
        }

        private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
        {
            UpdateGridVisual();
        }

    }
}
