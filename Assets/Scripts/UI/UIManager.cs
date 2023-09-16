using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _startServerButton;
    [SerializeField] private Button _startHostButton;
    [SerializeField] private Button _startClientButton;
    [SerializeField] private TextMeshProUGUI _playersInGameText;

    private void Awake() 
    {
        Cursor.visible = true;
    }

    private void Start() 
    {
        _startHostButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host started ...");
            }
            else
            {
                Debug.Log("Host could not be started ...");
            }
        });

        _startServerButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartServer())
            {
                Debug.Log("Server started ...");
            }
            else
            {
                Debug.Log("Server could not be started ...");
            }
        });

        _startClientButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client started ...");
            }
            else
            {
                Debug.Log("Client could not be started ...");
            }
        });
    }

    private void Update() 
    {
        _playersInGameText.SetText($"Players in game: {PlayersNetworkManager.Instance.PlayersInGame}");
    }
}
