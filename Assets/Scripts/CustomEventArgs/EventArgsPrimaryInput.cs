using System;
using UnityEngine;

public class EventArgsPrimaryInput : EventArgs
{
    public Vector3 InputHitPosition;

    public EventArgsPrimaryInput(Vector3 inputHitPosition)
    {
        InputHitPosition = inputHitPosition;
    }
}
