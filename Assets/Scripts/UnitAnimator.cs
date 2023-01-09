using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator = null; 
    [SerializeField] private Transform bulletProjectilePrefab = null;
    [SerializeField] private Transform shootPointTransform = null;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction)) // still is an internal dependency, as it's all on the unit GO
        {
            moveAction.StartMoving += MoveAction_StartMoving;
            moveAction.StopMoving += MoveAction_StopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.StartShooting += ShootAction_OnStartShooting;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void MoveAction_StartMoving(object sender, EventArgs e) 
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_StopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void ShootAction_OnStartShooting(object sender, ShootAction.StartShootingEventArgs e)
    {
        animator.SetTrigger("Shoot");
        var bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        var bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        // make sure the height of the bullet stays the same while moving
        var targetWorldPosition = e.targetUnit.GetWorldPosition();
        targetWorldPosition.y = shootPointTransform.transform.position.y;
        bulletProjectile.Setup(targetWorldPosition);
    }

   
}
