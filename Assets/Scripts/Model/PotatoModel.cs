using UnityEngine;

public class PotatoModel : Singleton<PotatoModel>
{
    // position potato move and face potato
    public Vector3 potatoPos;
    public bool facingRight = true;
    public Vector3 moveDirection = Vector3.zero;

    // id choose wepon and potato
    public int potatoId;
    public int weaponId;

    public int currentWeaponValue;
}