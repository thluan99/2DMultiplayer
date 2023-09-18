using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Collections;
using UnityEngine;
using Mirror;

public class PlayerObservable : NetworkBehaviour
{
    public Subject<Unit> OnBeNormalAttacked;
    public bool isAttacking = false;
    public Subject<string> AnimNeedPlay;
    public Subject<float> OnDecreaseHealth;

    private void Awake() 
    {
        OnBeNormalAttacked = new Subject<Unit>();
        AnimNeedPlay = new Subject<string>();
        OnDecreaseHealth = new Subject<float>();
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
