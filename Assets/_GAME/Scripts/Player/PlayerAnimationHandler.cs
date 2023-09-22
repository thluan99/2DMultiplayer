using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Mirror;
using UniRx;

public class PlayerAnimationHandler : NetworkBehaviour
{
    private Animator _animator;
    private PlayerObservable _playerObservable;

    private void Awake() 
    {
        _animator = GetComponent<Animator>();
        _playerObservable = GetComponent<PlayerObservable>();
    }
    
    void Start()
    {
        _playerObservable.AnimNeedPlay
            .Subscribe(anim => PlayAnimCommand(anim))
            .AddTo(gameObject);
    }

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }

    [Command]
    private void PlayAnimCommand(string anim)
    {
        _animator.Play(anim);
    }
}
