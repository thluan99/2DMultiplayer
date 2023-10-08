using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Collections;
using UnityEngine;
using Mirror;
using System;

public class PlayerObservable : NetworkBehaviour
{
    public bool isAttacking = false;
    public Subject<string> AnimNeedPlay;

    private void Awake() 
    {
        AnimNeedPlay = new Subject<string>();
    }

    public IObservable<float> HorizontalInput => Observable.EveryFixedUpdate()
        .Select(_ => DialogueManager.Instance.IsDialoguePlaying ? 0 : Input.GetAxis("Horizontal"));

    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }
}
