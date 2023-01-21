using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    // will listen to events on sceneObstacles that were destroyd, and then 
    // notify PathFinding to update the PathNode to IsWalkable=true
    
    private void Start()
    {
        // subscribe to crate's event
        // call function on PathFinding to update IsWalkable
        DestructableCrate.AnyDestroyed += DestructableCrate_OnAnyDestroyed;
    }

    private void DestructableCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructableCrate crateTransformPosition = sender as DestructableCrate;
        PathFinding.Instance.SetIsWalkableGridPosition(crateTransformPosition.GridPosition, true);
    }

}
