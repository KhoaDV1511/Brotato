using System;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Potato : Character
{
    [SerializeField] private PotatoMediator potatoMediator;
    [SerializeField] private Joystick joystick;
    [SerializeField] private AnimationClip[] clips;
    [SerializeField] private Transform potatoBody;
    [SerializeField] private RenderPotato renderPotato = new RenderPotato();

    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private readonly StartGameSignals _startGameSignals = Signals.Get<StartGameSignals>();
    private readonly PotatoRevivalSignals _potatoRevivalSignals = Signals.Get<PotatoRevivalSignals>();
    
    private AnimancerComponent _animancer;
    private Rigidbody2D _rb;
    
    private void OnEnable()
    {
        _startGameSignals.AddListener(Init);
        _potatoRevivalSignals.AddListener(PotatoRevival);
    }

    private void OnDisable()
    {
        _startGameSignals.RemoveListener(Init);
        _potatoRevivalSignals.RemoveListener(PotatoRevival);
    }

    protected override void Start()
    {
        //InitStat();
        //base.Start();
        _rb = GetComponent<Rigidbody2D>();
        _animancer = GetComponent<AnimancerComponent>();
        _animancer.Play(clips[(int)AnimPotato.Idle]);
    }
    private void FixedUpdate()
    {
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
        _potatoModel.moveDirection = ((Vector2)posMove - position).normalized;
        _rb.MovePosition(posMove.MapLimited());
        _potatoModel.potatoPos = posMove;
        _animancer.Play(clips[(int)AnimPotato.Move]);
    }

    private void FlipFace(float horizontal)
    {
        switch (horizontal)
        {
            case < 0 when _potatoModel.facingRight:
            case > 0 when !_potatoModel.facingRight:
                Flip();
                break;
        }
        void Flip()
        {
            _potatoModel.facingRight = !_potatoModel.facingRight;
            var trans = potatoBody;
            trans.eulerAngles = new Vector3 (0, _potatoModel.facingRight ? 0 : 180, 0);
        }
    }
    
    private void Init()
    {
        InitStat();
        renderPotato.RenderEyes(_potatoModel.potatoId);
        renderPotato.RenderHairs(_potatoModel.potatoId);
        renderPotato.RenderMouths(_potatoModel.potatoId);
    }

    private void InitStat()
    {
        stats.Add(new StatCharacter(StatType.HP, 10));
        stats.Add(new StatCharacter(StatType.SpeedVelocity, 5));
        stats.Add(new StatCharacter(StatType.DetectRange, 5));

        UpdatePotato(_potatoModel.levelPotato);
        potatoMediator.InitHealthAndLevel((int)CurrentHp);
    }

    [Button("PotatoRevival")]
    private void PotatoRevival()
    {
        statIncrease = 0;
        stats.Find(s => s.statType == StatType.HP).statIncrease = 0;
        stats.Find(s => s.statType == StatType.SpeedVelocity).statIncrease = 0;
        UpdatePotato(_potatoModel.levelPotato);
        Debug.Log(CurrentHp);
        potatoMediator.SetValueHp((int)CurrentHp);
    }

    private void UpdatePotato(int level)
    {
        stats.Find(s => s.statType == StatType.HP).statIncrease += level * 10;
        stats.Find(s => s.statType == StatType.SpeedVelocity).statIncrease += level * 0.2f;
    }
    
    public override void ReceiveDamage(StatType statType, float statIncrease)
    {
        base.ReceiveDamage(statType, statIncrease);
        potatoMediator.SetValueHp((int)CurrentHp);
        Debug.Log(CurrentHp);
        if(CurrentHp <= 0) Signals.Get<PotatoDeathSignals>().Dispatch();
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