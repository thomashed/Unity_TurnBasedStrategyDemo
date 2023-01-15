using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{

    public GridPosition GridPosition { get; private set; }
    public int Gcost { get; set; }
    public int Hcost { get; set; }
    public int Fcost { get; private set; }
    // we need to keep track of where we came from in order to go back 
    public PathNode CameFromPathNode { get; set; }

    public PathNode(GridPosition gridPosition)
    {
        this.GridPosition = gridPosition;
    }

    public void CalculateFCost()
    {
        Fcost = Gcost + Hcost;
    }

    public override string ToString()
    {
        return GridPosition.ToString();
    }

    public void ResetCameFromPathNode()
    {
        CameFromPathNode = null;
    }

}
