using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    [SerializeField] private TrailRenderer trailRenderer = null;
    [SerializeField] private Transform bulletVfxPrefab = null;

    private Vector3 targetPosition;
    private float moveSpeed = 150f;

    private void Update()
    {
        if (targetPosition == null) return;
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        var distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        var distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        // if distance to target is greater after moving, we overShot the target
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition; // might as well fix the position so user wont see the overshooting
            trailRenderer.transform.parent = null; // un-parent the Trail, so its destroyed independently
            Destroy(gameObject);
            Instantiate(bulletVfxPrefab, targetPosition, Quaternion.identity);
        }

    }

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }


}
