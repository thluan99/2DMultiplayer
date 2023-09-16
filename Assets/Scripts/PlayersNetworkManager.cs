using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersNetworkManager : Singleton<PlayersNetworkManager>
{
    private NetworkVariable<int> _playersInGame = new NetworkVariable<int>();

    public int PlayersInGame { get => _playersInGame.Value; }

    private void Start() 
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) => 
        {

            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log($"{id} is connected!");
                _playersInGame.Value++;
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log($"{id} is disconnected!");
                _playersInGame.Value--;
            }
        };
    }
}
