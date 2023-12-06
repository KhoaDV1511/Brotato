using System;
using UnityEngine;

public class GunMediator : MonoBehaviour
{
    [SerializeField] private Transform guns;
    [SerializeField] private Gun gun;

    private void Start()
    {
        var objGun = Instantiate(gun, guns);
        objGun.Show();
    }
}