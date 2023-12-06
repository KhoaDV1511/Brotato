using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : Character
{
    protected virtual void LookAtTarget(Vector3 target, Transform weaponPos)
    {
        Quaternion rotation = Quaternion.LookRotation
            (target - weaponPos.position, weaponPos.TransformDirection(Vector3.up));
        weaponPos.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }
}