using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolBeans.Grid
{
    public class GridObject
    {
        private GridSystem gridSystem; // what GridSystem created this object
        private GridPosition gridPosition; // which gridPos does this object belong to
        public Unit Unit { get; set; }

        public GridObject(GridSystem gridSystem, GridPosition gridPosition)
        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
        }

        public override string ToString()
        {
            return $"{gridPosition}\n{Unit}"; 
        }

    }

}

