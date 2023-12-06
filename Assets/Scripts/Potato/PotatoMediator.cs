using System;
using Animancer;
using UnityEngine;
public class PotatoMediator : Character
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private AnimationClip[] clips;
    [SerializeField] private Transform potatoBody;
    
    private AnimancerComponent _animancer;
    private Rigidbody2D _rb;
    private bool _facingRight = true;
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
            Vector2 posMove = _rb.position + _move * (speed * Time.fixedDeltaTime);
            posMove.x = posMove.x > GameKey.MAP_MAX_X ? GameKey.MAP_MAX_X : posMove.x;
            posMove.x = posMove.x < GameKey.MAP_MIN_X ? GameKey.MAP_MIN_X : posMove.x;
            posMove.y = posMove.y > GameKey.MAP_MAX_Y ? GameKey.MAP_MAX_Y : posMove.y;
            posMove.y = posMove.y < GameKey.MAP_MIN_Y ? GameKey.MAP_MIN_Y : posMove.y;
            _rb.MovePosition(posMove);
            _animancer.Play(clips[(int)AnimPotato.Move]);
        }
        else
        {
            _animancer.Play(clips[(int)AnimPotato.Idle]);
        }
        switch (horizontal)
        {
            case < 0 when _facingRight:
            case > 0 when !_facingRight:
                FlipFace();
                break;
        }
        void FlipFace()
        {
            _facingRight = !_facingRight;
            var trans = potatoBody;
            
            trans.eulerAngles = new Vector3 (0, _facingRight ? 0 : 180, 0);
        }
    }
}

public enum AnimPotato
{
    Idle,
    Move,
    Death
}