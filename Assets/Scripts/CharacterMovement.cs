using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UIElements;
using System;

public class CharacterMovement : NetworkBehaviour
{
    private const string WALK = "Walk";
    private const string IDLE = "Idle";
    private float _horizontal;
    private Rigidbody2D _rigid;
    private float _speed = 8f;
    private bool _isMoving = false;
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
    }

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        
        if (_horizontal != 0)
        {   
            if (!_isMoving)
            {
                _isMoving = true;
                _playerObservable.AnimNeedPlay.OnNext(WALK);
            }

            if (_horizontal < 0)
                SetFlipX(true);
            else if (_horizontal > 0)
                SetFlipX(false);
        }
        else if (_horizontal == 0 && !_playerObservable.isAttacking)
        {
            _isMoving = false;
            _playerObservable.AnimNeedPlay.OnNext(IDLE);
        }
    }

    private void FixedUpdate() 
    {
        _horizontal = Input.GetAxis("Horizontal");
        _rigid.velocity = new Vector2(_horizontal * _speed, _rigid.velocity.y);
    }

    private void FlipSpriteRenderer(bool oldValue, bool newValue)
    {
        _spriteRenderer.flipX = newValue;
    }

    [Command]
    private void SetFlipX(bool newValue) => _isFlipX = newValue;
}
