using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerHud : NetworkBehaviour
{
    private NetworkVariable<FixedString32Bytes> _playerName;
    private bool _overlaySet = false;

    private void Awake() 
    {
        _playerName = new NetworkVariable<FixedString32Bytes>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            _playerName.Value = ("Player " + OwnerClientId);
        }
    }

    public void SetOverlay()
    {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.SetText(_playerName.Value.ToString());
    }

    private void Update() 
    {
        if (!_overlaySet && !string.IsNullOrEmpty(_playerName.Value.ToString()))
        {
            SetOverlay();
            _overlaySet = true;
        }    
    }
}