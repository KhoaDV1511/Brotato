using System;

public class Potato : Character
{
    protected float currentHp;
    protected float speedVelocity;
    protected virtual void Start()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();
        currentHp = stats.Find(s => s.statType == StatType.HP).baseValue;
        speedVelocity = stats.Find(s => s.statType == StatType.SpeedVelocity).baseValue;
    }
}