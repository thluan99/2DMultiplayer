using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UniRx;

public class EnemyHealth : NetworkBehaviour, IHeath
{
    [SerializeField] private PlayerHud _playerHud;
    [SyncVar(hook = nameof(CheckCurrentHealth))] private float _currentHealth;
    [SyncVar] private float _maxHealth;

    private void Start() 
    {
        ConfigHealthServer();
    }

    [Server]
    private void ConfigHealthServer()
    {
        _maxHealth = 1000;
        _currentHealth = _maxHealth;
    }

    private void CheckCurrentHealth(float prev, float newValue)
    {
        if (newValue == 0)
        {
            Debug.Log("Die!");
        }
    }

    public void TakeDamage(float health)
    {
        Debug.Log("Enemy Take damage: " + health + " _currentHealth: " + _currentHealth + " / " + _maxHealth);
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
