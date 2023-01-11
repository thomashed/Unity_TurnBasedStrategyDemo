using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    public static UnitManager Instance { get; private set; }

    public List<Unit> UnitList { get; private set; }
    public List<Unit> FriendlyUnitList { get; private set; }
    public List<Unit> EnemyUnitList { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Trying to create more than one instance of {this}!");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        UnitList = new List<Unit>();
        FriendlyUnitList = new List<Unit>();    
        EnemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.AnyUnitSpawned += Unit_OnAnyUnitSpawned;        
        Unit.AnyUnitDead += Unit_OnAnyUnitDead;        
    }



    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unitToTrack = sender as Unit;
        UnitList.Add(unitToTrack);

        if (unitToTrack.IsEnemy)
        {
            EnemyUnitList.Add(unitToTrack);
        }
        else if (!unitToTrack.IsEnemy)
        {
            FriendlyUnitList.Add(unitToTrack);  
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unitToTrack = sender as Unit;
        UnitList.Remove(unitToTrack);

        if (unitToTrack.IsEnemy)
        {
            EnemyUnitList.Remove(unitToTrack);
        }
        else if (!unitToTrack.IsEnemy) 
        {
            FriendlyUnitList.Remove(unitToTrack);
        }
    }

}
