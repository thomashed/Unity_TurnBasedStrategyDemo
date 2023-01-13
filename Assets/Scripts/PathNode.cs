using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{

    private GridPosition gridPosition;
    public int Gcost { get; private set; }
    public int Hcost { get; private set; }
    public int Fcost { get; private set; }
    // we need to keep track of where we came from in order to go back 
    private PathNode cameFromPathNode;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

}
