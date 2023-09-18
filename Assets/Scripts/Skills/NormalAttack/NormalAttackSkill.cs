using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NormalAttackSkill : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other) 
    {
        var playerConnectClient = other.GetComponent<NetworkIdentity>().connectionToClient;
        if (playerConnectClient.connectionId == connectionToClient.connectionId) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player: " + other.gameObject.name + " be attacked!");
            TakeAttacking(playerConnectClient, this.gameObject);
            other.GetComponent<PlayerObservable>().OnDecreaseHealth.OnNext(100);
        }

    }

    [TargetRpc]
    public void TakeAttacking(NetworkConnectionToClient target, GameObject skillObject)
    {
        skillObject.SetActive(false);
    }
}
