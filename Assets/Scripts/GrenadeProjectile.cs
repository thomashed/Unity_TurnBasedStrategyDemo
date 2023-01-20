using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{

    private Vector3 targetPosition;
    private int damageEffect = 30;
    private Action onGrenadeBehaviourComplete; 

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float moveSpeed =15f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    
        float reachedTargetDistance = .2f;
        if (Vector3.Distance(targetPosition, transform.position) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            var colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (var collider in colliderArray)
            {
                if (!collider.TryGetComponent<Unit>( out Unit targetUnit)) continue;
                targetUnit.Damage(damageEffect);
            }

            Destroy(gameObject);
            onGrenadeBehaviourComplete();
        }
    }

}
