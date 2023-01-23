using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator = null; 
    [SerializeField] private Transform bulletProjectilePrefab = null;
    [SerializeField] private Transform shootPointTransform = null;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;

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
        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.SwordActionStarted += SwordAction_SwordActionStarted;
            swordAction.SwordActionCompleted += SwordAction_SwordActionCompleted;
        }
    }

    private void Start()
    {
        EquipRifle();
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

    private void EquipSword()
    {
        rifleTransform.gameObject.SetActive(false);
        swordTransform.gameObject.SetActive(true);
    }

    private void EquipRifle()
    {
        rifleTransform.gameObject.SetActive(true);
        swordTransform.gameObject.SetActive(false);
    }

    private void SwordAction_SwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }

    private void SwordAction_SwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();
        animator.SetTrigger("SwordSlash");
    }

}
