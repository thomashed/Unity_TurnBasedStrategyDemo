using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{

    [SerializeField] private Transform ragdollPrefab = null;
    [SerializeField] private Transform originalRootbone = null;
    
    private HealthSystem healthSystem;
    private Unit unit;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>(); // inner dependency, internal meaning scope of this prefab
        unit = GetComponent<Unit>();

        healthSystem.Dead += HealthSystem_OnDead;
    }

    private void OnDestroy()
    {
        healthSystem.Dead -= HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        var ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        var unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootbone);

    }

}
