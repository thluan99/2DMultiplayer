using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NormalAttackSkill : AttackSkill
{
    public override void InitInflictDamageType()
    {
        _inflictDamageType = new OneTimeDamage();
    }

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }

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
            
            AttackTo(otherNetworkId.connectionToClient, this.gameObject);
            _inflictDamageType.InflictDamage(other.gameObject, _skillDamges);
        }
    }

    [TargetRpc]
    public override void AttackTo(NetworkConnectionToClient target, GameObject skillObject)
    {
        skillObject.SetActive(false);
    }
}
