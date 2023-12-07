using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public EnemyData enemyData;

    private static GlobalData _ins;
    public static GlobalData Ins
    {
        get
        {
            if (!_ins)
            {
                _ins = FindObjectOfType<GlobalData>();
            }

            return _ins;
        }
    }
}