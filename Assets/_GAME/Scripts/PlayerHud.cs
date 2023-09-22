using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class PlayerHud : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _localPlayerOverlay;
    [SerializeField] private Image _healthBar;
    private string _playerName;

    [SyncVar(hook = nameof(UpdateHealthBar))]
    private float _healthBarValue;

    public void SetHealthBarValue(float value)
    {
        _healthBarValue = value;
    }

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }

    private void UpdateHealthBar(float oldValue, float newValue)
    {
        _healthBar.fillAmount = newValue;
    }
}