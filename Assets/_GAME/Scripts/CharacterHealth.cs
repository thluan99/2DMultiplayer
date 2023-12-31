using System;
using System.Collections;
using Mirror;
using Mirror.Examples.Benchmark;
using UniRx;
using UnityEngine;

public class CharacterHealth : NetworkBehaviour, IHeath
{
    [SerializeField] private PlayerHud _playerHud;
    [SyncVar(hook = nameof(CheckCurrentHealth))] private float _currentHealth;
    [SyncVar] private float _maxHealth;
    private PlayerObservable _playerObservable;

    private void Awake() 
    {
        _playerObservable = GetComponent<PlayerObservable>();
    }

    private void Start() 
    {
        ConfigHealthCommand();
    }

    [Command]
    private void ConfigHealthCommand()
    {
        _maxHealth = 1000;
        _currentHealth = _maxHealth;
        Debug.Log(_currentHealth + " / " + _maxHealth);
    }

    private void CheckCurrentHealth(float prev, float newValue)
    {
        if (newValue == 0)
        {
            Debug.Log("Die!");
            _playerObservable.AnimNeedPlay.OnNext("Die");
            gameObject.GetComponent<CharacterMovement>().enabled = false;
        }
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
