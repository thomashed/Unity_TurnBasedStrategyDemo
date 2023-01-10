using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolBeans.Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        [SerializeField] private Transform gridSystemVisualPrefab = null;
        private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

        public static GridSystemVisual Instance;

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
            if (selectedAction == null) return;
            var gridPositionList = selectedAction.GetValidActionGridPositionList();
            ShowGridPositionList(gridPositionList);
        }

        public void HideAllGridPosition() 
        {
            foreach (var gridSystemVisualSingle in gridSystemVisualSingleArray)
            {
                gridSystemVisualSingle.Hide();
            }
        }

        public void ShowGridPositionList(List<GridPosition> gridPositionList)
        {
            foreach (var gridPosition in gridPositionList)
            {
                gridSystemVisualSingleArray[gridPosition.X, gridPosition.Z].Show();
            }
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
