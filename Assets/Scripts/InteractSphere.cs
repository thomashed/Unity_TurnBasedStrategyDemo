using CoolBeans.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{


    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private MeshRenderer meshRenderer;

    private GridPosition gridPosition;
    private bool isGreen = false;
    private bool isActive = false;
    private float timer = 0f;
    private Action onInteractionComplete;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        SetColourGreen();
    }

    private void Update()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
        }
    
    }

    public void SetColourGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }

    public void SetColourRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;

        if (isGreen)
        {
            SetColourRed();
        }
        else
        {
            SetColourGreen();
        }
    }
}
