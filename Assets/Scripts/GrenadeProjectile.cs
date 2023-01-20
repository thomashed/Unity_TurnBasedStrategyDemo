using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler AnyGrenadeExploded;

    [SerializeField] private Transform grenadeExplodeVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimation;

    private Vector3 targetPosition;
    private int damageEffect = 30;
    private Action onGrenadeBehaviourComplete;
    
    private float totalDistance;
    private Vector3 positionXZ; 


    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);  
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;
        float moveSpeed =15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance; // invert

        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimation.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(targetPosition, positionXZ) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            var colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (var collider in colliderArray)
            {
                if (!collider.TryGetComponent<Unit>( out Unit targetUnit)) continue;
                targetUnit.Damage(damageEffect);
            }

            OnAnyGrenadeExploded();
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
            onGrenadeBehaviourComplete();
        }
    }

    private void OnAnyGrenadeExploded()
    {
        AnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
    }

}
