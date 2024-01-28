using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour
{
    public Action throwProjectile;
    public void Shoot()
    {
        throwProjectile?.Invoke();
    }
}
