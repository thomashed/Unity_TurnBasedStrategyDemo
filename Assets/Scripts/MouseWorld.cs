using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    /// <summary>
    /// mouse click register
    /// publish event that primary input was clicked
    /// include customEventArgs where in worldSpace we clicked
    /// cursorobject goes where mouseclicked
    /// </summary>

    //public static event EventHandler<EventArgsPrimaryInput> InputPrimaryClicked;

    [SerializeField] private LayerMask layerMask = 0;

    void Update()
    {
        //RegisterInput();
        //DebugCursorFollower();
    }

    //private void DebugCursorFollower()
    //{
    //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return; // TODO: DRY
    //    transform.position = hit.point;
    //}

    //private void RegisterInput()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
    //        {
    //            OnInputPrimaryClicked(hit.point);
    //        }
    //    }
    //}

    //private void OnInputPrimaryClicked(Vector3 pointClicked)
    //{
    //    InputPrimaryClicked?.Invoke(this, new EventArgsPrimaryInput(pointClicked));
    //}

    public static Vector3 GetPosition()
    {
        var ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        var isHit = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
        return hit.point;
    }
}
