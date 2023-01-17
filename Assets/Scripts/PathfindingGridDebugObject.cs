using CoolBeans.Grid;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{

    [SerializeField] private TextMeshPro gCostText = null;
    [SerializeField] private TextMeshPro hCostText = null;
    [SerializeField] private TextMeshPro fCostText = null;
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer = null;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject; 
    }

    protected override void Update()
    {
        base.Update();

        gCostText.text = pathNode.Gcost.ToString();
        hCostText.text = pathNode.Hcost.ToString();
        fCostText.text = pathNode.Fcost.ToString();

        isWalkableSpriteRenderer.color = pathNode.IsWalkable ? Color.green : Color.red;
    }



}
