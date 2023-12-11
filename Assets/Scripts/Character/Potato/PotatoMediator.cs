using System;
using Animancer;
using UnityEngine;
using UnityEngine.U2D;

public class PotatoMediator : Character
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private AnimationClip[] clips;
    [SerializeField] private Transform potatoBody;
    [SerializeField] private RenderPotato renderPotato;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    private readonly RenderPotatoSignals _renderPotatoSignals = Signals.Get<RenderPotatoSignals>();
    
    private AnimancerComponent _animancer;
    private Rigidbody2D _rb;
    private Vector2 _move;

    private void OnEnable()
    {
        _renderPotatoSignals.AddListener(RenderPotato);
    }

    private void OnDisable()
    {
        _renderPotatoSignals.RemoveListener(RenderPotato);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        speed = 5;
        _animancer = GetComponent<AnimancerComponent>();
        _animancer.Play(clips[(int)AnimPotato.Idle]);
        
    }

    private void RenderPotato()
    {
        renderPotato.RenderEyes(_potatoModel.potatoId);
        renderPotato.RenderHairs(_potatoModel.potatoId);
        renderPotato.RenderMouths(_potatoModel.potatoId);
    }
    private void FixedUpdate()
    {
        var horizontal = joystick.Horizontal;
        var vertical = joystick.Vertical;
        _move = new Vector2(horizontal, vertical);
        if (horizontal != 0)
        {
            Vector3 posMove = _rb.position + _move * (speed * Time.fixedDeltaTime);
            
            _rb.MovePosition(posMove.MapLimited());
            _potatoModel.potatoPos = posMove;
            _animancer.Play(clips[(int)AnimPotato.Move]);
        }
        else
        {
            _animancer.Play(clips[(int)AnimPotato.Idle]);
        }
        switch (horizontal)
        {
            case < 0 when _potatoModel.facingRight:
            case > 0 when !_potatoModel.facingRight:
                FlipFace();
                break;
        }
        void FlipFace()
        {
            _potatoModel.facingRight = !_potatoModel.facingRight;
            var trans = potatoBody;
            
            trans.eulerAngles = new Vector3 (0, _potatoModel.facingRight ? 0 : 180, 0);
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
    private static SpriteAtlas[] _potatoAtlas;
    private static SpriteAtlas[] PotatoAtlas
    {
        get
        {
            for (var i = 0; i < names.Length; i++)
            {
                if (_potatoAtlas[i] == null) _potatoAtlas[i] = Resources.Load<SpriteAtlas>($"SpriteAtlas/{names[i]}");
            }

            return _potatoAtlas;
        }
    }

    private Sprite GetSprite(int id, TypeBody typeBody)
    {
        return PotatoAtlas[(int)typeBody].GetSprite($"{typeBody.ToString()}_{id}");
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