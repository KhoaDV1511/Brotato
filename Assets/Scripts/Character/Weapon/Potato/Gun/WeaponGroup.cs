using System;
using UnityEngine;

public class WeaponGroup : MonoBehaviour
{
    [SerializeField] private Transform weapons;
    [SerializeField] private Weapon weapon;
    [SerializeField] private SpriteRenderer sprWeapon;

    public void SetData(Sprite spr)
    {
        var objWeapon = Instantiate(weapon, weapons);
        sprWeapon.sprite = spr;
        objWeapon.Show();
    }
}