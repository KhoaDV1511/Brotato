using System;
using Animancer;
using UnityEngine;
public class PotatoMediator : Character
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private AnimationClip[] clips;
    [SerializeField] private Transform potatoBody;
    
    private readonly PotatoModel _potatoModel = PotatoModel.Instance;
    
    private AnimancerComponent _animancer;
    private Rigidbody2D _rb;
    private Vector2 _move;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        speed = 5;
        _animancer = GetComponent<AnimancerComponent>();
        _animancer.Play(clips[(int)AnimPotato.Idle]);
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

public enum AnimPotato
{
    Idle,
    Move,
    Death
}