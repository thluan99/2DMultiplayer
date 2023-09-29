using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class NormalAttackSkill : NetworkBehaviour
{
    private const float SKILL_DAMAGE = 100;
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
        var otherConnectClient = other.GetComponent<NetworkIdentity>().connectionToClient;

        if (otherConnectClient.connectionId == connectionToClient.connectionId) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log(other.gameObject.name + " be attacked!");
            
            TakeAttacking(otherConnectClient, this.gameObject);
            other.GetComponent<IHeath>().TakeDamage(SKILL_DAMAGE);
        }

    }

    [TargetRpc]
    public void TakeAttacking(NetworkConnectionToClient target, GameObject skillObject)
    {
        skillObject.SetActive(false);
    }
}
