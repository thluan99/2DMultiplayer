using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UIElements;
using System;
using UniRx;

public class CharacterMovement : NetworkBehaviour
{
    private const string WALK = "Walk";
    private const string IDLE = "Idle";
    private float _horizontal;
    private Rigidbody2D _rigid;
    private float _speed = 8f;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private PlayerObservable _playerObservable;

    [SyncVar(hook = nameof(FlipSpriteRenderer))]
    private bool _isFlipX;

    private void Awake() 
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerObservable = GetComponent<PlayerObservable>();
    }

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 2f);

        if (!isLocalPlayer) return;

        _playerObservable.HorizontalInput
            .Where(horizontal => horizontal < 0 && !_playerObservable.isAttacking)
            .Subscribe(horizontal => {
                _playerObservable.AnimNeedPlay.OnNext(WALK);
                _rigid.velocity = new Vector2(horizontal * _speed, _rigid.velocity.y);
                SetFlipX(true);
            }).AddTo(gameObject);
        
        _playerObservable.HorizontalInput
            .Where(horizontal => horizontal > 0 && !_playerObservable.isAttacking)
            .Subscribe(horizontal => {
                _playerObservable.AnimNeedPlay.OnNext(WALK);
                _rigid.velocity = new Vector2(horizontal * _speed, _rigid.velocity.y);
                SetFlipX(false);
            }).AddTo(gameObject);
        
        _playerObservable.HorizontalInput
            .Where(horizontal => horizontal == 0 && !_playerObservable.isAttacking)
            .Subscribe(_ => {
                _playerObservable.AnimNeedPlay.OnNext(IDLE);
            }).AddTo(gameObject);
    }

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }

    private void FlipSpriteRenderer(bool oldValue, bool newValue)
    {
        _spriteRenderer.flipX = newValue;
    }

    [Command]
    private void SetFlipX(bool newValue) => _isFlipX = newValue;
}
