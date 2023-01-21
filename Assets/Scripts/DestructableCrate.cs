using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{

    public static event EventHandler AnyDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;

    public GridPosition GridPosition { get; private set; }

    private void Start()
    {
        GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        var crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);

        Destroy(gameObject);
        OnAnyDestroyed();
    }

    private void OnAnyDestroyed()
    {
        AnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            // if we have rigidBody, means we should apply explosion effect
            if (child.gameObject.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }

}
