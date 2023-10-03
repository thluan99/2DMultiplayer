using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NormalAttackSkill : NetworkBehaviour
{
    private const float SKILL_DAMAGE = 100;
    [SerializeField] private LayerMask _canAttackLayer;

    public int ObjectId;

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
        var otherNetworkId = other.GetComponent<NetworkIdentity>();

        if (otherNetworkId.connectionToClient != null)
        {
            Debug.Log("otherNetworkIdconnectionToClient.connectionId " + otherNetworkId.connectionToClient.connectionId  + " ObjectId: " + ObjectId);
            if (otherNetworkId.connectionToClient.connectionId == ObjectId) return;
        }

        if ((_canAttackLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            Debug.Log(other.gameObject.name + " be attacked!");
            
            TakeAttacking(otherNetworkId.connectionToClient, this.gameObject);
            other.GetComponent<IHeath>()?.TakeDamage(SKILL_DAMAGE);
        }
    }

    [TargetRpc]
    public void TakeAttacking(NetworkConnectionToClient target, GameObject skillObject)
    {
        skillObject.SetActive(false);
    }
}
