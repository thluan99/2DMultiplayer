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
        if (other.GetComponent<NetworkIdentity>().connectionToClient.connectionId == connectionToClient.connectionId) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player: " + other.gameObject.name + " be attacked!");
            other.GetComponent<CharacterBeAttacked>().TakeAttacking(
                other.gameObject.GetComponent<NetworkIdentity>().connectionToClient, 
                this.gameObject
            );
        }

    }
}
