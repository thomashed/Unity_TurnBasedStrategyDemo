using CoolBeans.Grid;
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
    private GridPosition gridPosition;

    private void Awake()
    {
        targetDest = transform.position;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this); // place the Unit on the levelGrid
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
            CheckUnitMovement();
        }

        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition); // unit changed to a new gridPosition, we update grid 
            gridPosition = newGridPosition;
        }

    }

    private void CheckUnitMovement()
    {
        unitAnimator.SetBool("IsWalking", true);
        var newPos = (targetDest - transform.position).normalized;
        //transform.forward += newPos * rotationSpeed;
        transform.forward = Vector3.Lerp(transform.forward, newPos, Time.deltaTime * rotationSpeed);
        transform.position += newPos * movingSpeed * Time.deltaTime;
    }


    public void Move(Vector3 targetDest)
    {
        this.targetDest = targetDest;   
    }



}
