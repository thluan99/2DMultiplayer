using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerComposer : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        this.enabled = true;
    }

    public override void OnStopAuthority()
    {
        this.enabled = false;
    }

    public override void OnStartClient()
    {
        if (!isLocalPlayer) return;

        AddPlayerInteract();
    }

    private void AddPlayerInteract()
    {
        if (gameObject.GetComponent<PlayerInteract>() == null)
            gameObject.AddComponent<PlayerInteract>();
    }
}
