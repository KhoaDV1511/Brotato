using System;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Potato : Character
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private AnimationClip[] clips;
    [SerializeField] private Transform potatoBody;
    [SerializeField] private RenderPotato renderPotato = new RenderPotato();
    [SerializeField] private PotatoMediator potatoMediator;
    private readonly StartGameSignals _startGameSignals = Signals.Get<StartGameSignals>();
    private readonly PotatoRevivalSignals _potatoRevivalSignals = Signals.Get<PotatoRevivalSignals>();
    private readonly StartNewWaveSignals _startNewWaveSignals = Signals.Get<StartNewWaveSignals>();
    private readonly UpgradeItemSignals _upgradeItemSignals = Signals.Get<UpgradeItemSignals>();
    
    private AnimancerComponent _animancer;
    private Rigidbody2D _rb;

    protected override void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animancer = GetComponent<AnimancerComponent>();
        _animancer.Play(clips[(int)AnimPotato.Idle]);
    }

    protected override void OnEnable()
    {
        _startGameSignals.AddListener(InitStat);
        _potatoRevivalSignals.AddListener(PotatoResetHp);
        _startNewWaveSignals.AddListener(PotatoResetHp);
        _upgradeItemSignals.AddListener(UpdateWeapon);
    }

    protected override void OnDisable()
    {
        _startGameSignals.RemoveListener(InitStat);
        _potatoRevivalSignals.RemoveListener(PotatoResetHp);
        _startNewWaveSignals.RemoveListener(PotatoResetHp);
        _upgradeItemSignals.RemoveListener(UpdateWeapon);
    }
    
    private void UpdateWeapon(EquipmentItemInfo equipmentItemInfo)
    {
        var statType = equipmentItemInfo.ItemStat.statItemIncreases.Find(s => s.increaseFor == IncreaseFor.Potato);
        if (statType != null)
        {
            stats.Find(s => s.statType == statType.statType).statIncrease += statType.statIncrease;
            Debug.Log("update potato");
        }
    }

    private void InitPotato()
    {
        stats.Clear();
        var statPotato = new PotatoStatInit(10, 5, 5, 6);
        stats.Add(new StatCharacter(StatType.HP, statPotato.health));
        stats.Add(new StatCharacter(StatType.SpeedVelocity, statPotato.speedVelocity));
        stats.Add(new StatCharacter(StatType.AttackRange));
        stats.Add(new StatCharacter(StatType.AttackSpeed));
        stats.Add(new StatCharacter(StatType.Dodge));
        stats.Add(new StatCharacter(StatType.HPRegeneration));
        stats.Add(new StatCharacter(StatType.DetectRange, statPotato.detectRange));
    }
    private void InitStat()
    {
        InitPotato();
        RenderPotato();

        UpdatePotato(PotatoModel.levelPotato);
        potatoMediator.InitHealthAndLevel(Mathf.CeilToInt(CurrentHp));
    }
    
    private void PotatoResetHp()
    {
        Debug.Log(stats.Count);
        stats.Find(s => s.statType == StatType.HP).statIncrease = 0;
        UpdatePotato(PotatoModel.levelPotato);
        potatoMediator.maxHp = Mathf.CeilToInt(CurrentHp);
        potatoMediator.SetValueHp(Mathf.CeilToInt(CurrentHp));
    }

    private void UpdatePotato(int level)
    {
        stats.Find(s => s.statType == StatType.HP).statIncrease += level * 10;
        stats.Find(s => s.statType == StatType.SpeedVelocity).statIncrease += level * 0.2f;
    }
    
    public override void ReceiveDamage(StatType statType, float statIncreases)
    {
        base.ReceiveDamage(statType, statIncreases);
        potatoMediator.SetValueHp(Mathf.CeilToInt(CurrentHp));
        if(CurrentHp <= 0) Signals.Get<PotatoDeathSignals>().Dispatch();
    }
    private void RenderPotato()
    {
        renderPotato.RenderEyes(PotatoModel.potatoId);
        renderPotato.RenderHairs(PotatoModel.potatoId);
        renderPotato.RenderMouths(PotatoModel.potatoId);
    }
    private void FixedUpdate()
    {
        if(PotatoModel.isHarvestToStore) return;
        
        var horizontal = joystick.Horizontal;
        var vertical = joystick.Vertical;
        var move = new Vector2(horizontal, vertical);
        
        if (horizontal != 0)
        {
            PotatoMove(move);
        }
        else
        {
            _animancer.Play(clips[(int)AnimPotato.Idle]);
        }
        FlipFace(horizontal);
    }

    private void PotatoMove(Vector2 move)
    {
        var position = _rb.position;
        Vector3 posMove = position + move * (SpeedVelocity * Time.fixedDeltaTime);
        PotatoModel.moveDirection = ((Vector2)posMove - position).normalized;
        _rb.MovePosition(posMove.MapLimited());
        PotatoModel.potatoPos = posMove;
        _animancer.Play(clips[(int)AnimPotato.Move]);
    }

    private void FlipFace(float horizontal)
    {
        switch (horizontal)
        {
            case < 0 when PotatoModel.facingRight:
            case > 0 when !PotatoModel.facingRight:
                Flip();
                break;
        }
        void Flip()
        {
            PotatoModel.facingRight = !PotatoModel.facingRight;
            var trans = potatoBody;
            trans.eulerAngles = new Vector3 (0, PotatoModel.facingRight ? 0 : 180, 0);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 3);
        Gizmos.DrawWireSphere(transform.position, 7);
    }
}

[Serializable]
public class RenderPotato
{
    [SerializeField] private SpriteRenderer[] potato;
    private static string[] names = { "PotatoEyes", "PotatoMouth", "PotatoHair"};
    private static SpriteAtlas _potatoAtlas;
    private static SpriteAtlas PotatoAtlas(TypeBody typeBody)
    {
        _potatoAtlas = Resources.Load<SpriteAtlas>($"SpriteAtlas/{names[(int)typeBody]}");

        return _potatoAtlas;
    }

    private Sprite GetSprite(int id, TypeBody typeBody)
    {
        return PotatoAtlas(typeBody).GetSprite($"{typeBody.ToString()}_{id}");
    }

    public void RenderEyes(int id)
    {
        potato[(int)TypeBody.Eyes].sprite = GetSprite(id, TypeBody.Eyes);
    }
    public void RenderMouths(int id)
    {
        potato[(int)TypeBody.Mouth].sprite = GetSprite(id, TypeBody.Mouth);
    }
    public void RenderHairs(int id)
    {
        potato[(int)TypeBody.Hair].sprite = GetSprite(id, TypeBody.Hair);
    }
}
public enum AnimPotato
{
    Idle,
    Move,
    Death
}

public enum TypeBody
{
    Eyes,
    Mouth,
    Hair,
    Weapon
}