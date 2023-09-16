using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Collections;
using UnityEngine;
using Mirror;

public class PlayerObservable : NetworkBehaviour
{
    public Subject<Unit> OnBeNormalAttacked;
    public bool IsAttacking = false;

    public Subject<string> AnimNeedPlay;

    private void Awake() 
    {
        OnBeNormalAttacked = new Subject<Unit>();
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
