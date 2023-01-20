using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{

    private void Start()
    {
        ShootAction.AnyStartShooting += ShootAction_AnyStartShooting;
        GrenadeProjectile.AnyGrenadeExploded += GrenadeAction_AnyActionStarted;
    }

    private void ShootAction_AnyStartShooting(object sender, ShootAction.StartShootingEventArgs e)
    {
        ScreenShake.Instance.Shake();
    }

    private void GrenadeAction_AnyActionStarted(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }

}
