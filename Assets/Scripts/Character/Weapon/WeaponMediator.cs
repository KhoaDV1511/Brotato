using UnityEngine;

public class WeaponMediator : MonoBehaviour
{
    [SerializeField] private Transform[] weapon;

    private void Start()
    {
        var obj = Instantiate(weapon[(int)TypeWeapon.Gun], transform);
        obj.transform.position = new Vector3(0, 0.25f, 0);
    }
}