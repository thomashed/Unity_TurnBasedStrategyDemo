using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{

    [SerializeField] private Transform ragdollRootBone = null;
    
    public void Setup(Transform originalRootBone)
    {
        MathAllChildTransform(originalRootBone, ragdollRootBone); // match original's bones, to avoid T-pose
        ApplyExplosionToRagDoll(ragdollRootBone, 900f, transform.position, 100f); // apply force at ragdoll's position
    }

    private void MathAllChildTransform(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            var cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MathAllChildTransform(child, cloneChild); // we need to go multiple levels down, we use recursion
            }
        }
    }

    private void ApplyExplosionToRagDoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            // if we have rigidBody, means we should apply explosion effect
            if (child.gameObject.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody)) 
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyExplosionToRagDoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }

}
