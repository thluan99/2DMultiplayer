using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UniRx;
using UnityEngine;

public class CharacterBeAttacked : NetworkBehaviour
{
    private PlayerObservable _playerObservable;
    private bool _isAttacked = false;
    private void Awake() 
    {
        _playerObservable = GetComponent<PlayerObservable>();
    }

    [TargetRpc]
    public void TakeAttacking(NetworkConnectionToClient target, GameObject attackSkill)
    {
        attackSkill.SetActive(false);
    }

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }

    private IEnumerator WaitCooldown()
    {
        _isAttacked = true;
        yield return new WaitForSeconds(0.3f);
        _isAttacked = false;
    }
}
