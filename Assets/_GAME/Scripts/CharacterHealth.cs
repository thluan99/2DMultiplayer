using System;
using System.Collections;
using Mirror;
using UniRx;
using UnityEngine;

public class CharacterHealth : NetworkBehaviour, IHeath
{
    private PlayerObservable _playerObservable;
    [SerializeField] private PlayerHud _playerHud;
    [SyncVar] private float _currentHealth;
    [SyncVar] private float _maxHealth;
    private void Awake() 
    {
        _playerObservable = GetComponent<PlayerObservable>();
    }

    private void Start() 
    {
        if (isServer)
            ConfigHealthServer();
        else if (isClient) 
            ConfigHealthCommand();
    }

    [Server]
    private void ConfigHealthServer() => ConfigHealth();

    [Command]
    private void ConfigHealthCommand() => ConfigHealth();

    private void ConfigHealth()
    {
        _maxHealth = 1000;
        _currentHealth = _maxHealth;
        Debug.Log(_currentHealth + " / " + _maxHealth);
    }

    public void TakeDamage(float health)
    {
        Debug.Log("Take damage: " + health + " _currentHealth: " + _currentHealth + " / " + _maxHealth);
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
