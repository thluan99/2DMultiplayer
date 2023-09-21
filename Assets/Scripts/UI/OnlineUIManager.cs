using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.TanksCoop;
using UnityEngine;
using UnityEngine.UI;

public class OnlineUIManager : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _stopHost;

    private NetworkManager _networkManager => NetworkManager.singleton;

    private void Start() 
    {
        _exitButton.onClick.AddListener(Exit);
        _stopHost.onClick.AddListener(StopHost);
    }

    private void StopHost()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            _networkManager.StopHost();
        }
    }

    private void Exit()
    {
        if (NetworkClient.isConnected)
        {
            _networkManager.StopClient();
        }
        else if (NetworkServer.active)
        {
            _networkManager.StopServer();
        }
    }
}
