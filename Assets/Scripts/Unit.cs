using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] private float stoppingDistance = 0.5f;
    private float movingSpeed = 5f;
    [SerializeField] private Animator unitAnimator = null;

    private Vector3 targetDest = Vector3.zero;
    private float rotationSpeed = 15f;

    private void Awake()
    {
        MouseWorld.InputPrimaryClicked += InputPrimaryClicked;        
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetDest) < stoppingDistance) 
        {
            unitAnimator.SetBool("IsWalking", false);
            return;
        }
        else
        {
            MoveUnit();
        }
        
    }

    private void MoveUnit()
    {
        unitAnimator.SetBool("IsWalking", true);
        var newPos = (targetDest - transform.position).normalized;
        //transform.forward += newPos * rotationSpeed;
        transform.forward = Vector3.Lerp(transform.forward, newPos, Time.deltaTime * rotationSpeed);
        transform.position += newPos * movingSpeed * Time.deltaTime;
    }

    private void InputPrimaryClicked(object sender, EventArgsPrimaryInput e)
    {
        targetDest = e.InputHitPosition;
    }

}
