using System;
using Animancer;
using UnityEngine;
public class PotatoMediator : Character
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private AnimationClip[] clips;
    
    private AnimancerComponent _animancer;
    private Rigidbody2D _rb;
    private bool _facingRight = true;
    private Vector2 _move;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        speed = 10;
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
            posMove.x = posMove.x > 13 ? 13 : posMove.x;
            posMove.x = posMove.x < -13 ? -13 : posMove.x;
            posMove.y = posMove.y > 9 ? 9 : posMove.y;
            posMove.y = posMove.y < -9 ? -9 : posMove.y;
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
            var trans = transform;
            Vector3 tempLocalScale = trans.localScale;
            tempLocalScale.x *= -1; 
            trans.localScale = tempLocalScale; 
        }
    }
}

public enum AnimPotato
{
    Idle,
    Move,
    Death
}