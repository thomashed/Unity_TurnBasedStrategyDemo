using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolBeans.Grid
{
    public class GridObject
    {
        private GridSystem gridSystem; // what GridSystem created this object
        private GridPosition gridPosition; // which gridPos does this object belong to
        private List<Unit> UnitList;

        public GridObject(GridSystem gridSystem, GridPosition gridPosition)
        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
            this.UnitList = new List<Unit>();   
        }

        public void AddUnit(Unit unit) 
        { 
            UnitList.Add(unit); 
        }

        public List<Unit> GetUnitList()
        {
            return UnitList;
        }

        public void RemoveUnit(Unit unit)
        {
            UnitList.Remove(unit);
        }

        public override string ToString()
        {
            var unitstring = "";

            foreach (var unit in UnitList)
            {
                unitstring += unit.ToString() + "\n";
            }

            return $"{gridPosition}\n{unitstring}"; 
        }

        public bool HasUnit()
        {
            return UnitList.Count > 0;
        }

        public Unit GetUnit()
        {
            if (!HasUnit()) return null;
            return UnitList[0]; 
        }

    }

}

