using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Collections;
using UnityEngine;
using Mirror;

public class PlayerObservable : NetworkBehaviour
{
    public bool isAttacking = false;
    public Subject<string> AnimNeedPlay;

    private void Awake() 
    {
        AnimNeedPlay = new Subject<string>();
    }

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }
}
