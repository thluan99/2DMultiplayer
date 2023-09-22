using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UniRx;
using UnityEngine;

public class CharacterBeAttacked : NetworkBehaviour
{
    private PlayerHud _playerHud;
    private PlayerObservable _playerObservable;
    [SyncVar] private float _currentHealth;
    [SyncVar] private float _maxHealth = 1000;
    private void Awake() 
    {
        _playerObservable = GetComponent<PlayerObservable>();
    }

    private void Start() 
    {
        _currentHealth = _maxHealth;
        _playerHud = GetComponentInChildren<PlayerHud>();

        _playerObservable.OnDecreaseHealth
            .Subscribe(health => DecreaseHealthHandler(health))
            .AddTo(gameObject);
    }

    private void DecreaseHealthHandler(float health)
    {
        _currentHealth -= health;
        float healthRatio = _currentHealth / _maxHealth;
        _playerHud.SetHealthBarValue(healthRatio);
    }

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }
}
