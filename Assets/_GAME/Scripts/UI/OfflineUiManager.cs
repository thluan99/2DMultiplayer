using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class OfflineUiManager : MonoBehaviour
{
    [SerializeField] private NetworkManager _networkManager;

    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _serverButton;

    private void Start() 
    {
        _loginButton.onClick.AddListener(OnLoginButtonClicked);
        _hostButton.onClick.AddListener(OnHostButtonClicked);
        _serverButton.onClick.AddListener(OnServerButtonClicked);
    }

    private void OnServerButtonClicked()
    {
        _networkManager.StartServer();
    }

    private void OnHostButtonClicked()
    {
        _networkManager.StartHost();
    }

    private void OnLoginButtonClicked()
    {
        _networkManager.StartClient();
    }
}
