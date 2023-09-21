using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerCameraHandler : NetworkBehaviour
{
    private const float Y_LOCAL_POSITION = 2.5f;
    private const float Z_LOCAL_POSITION = -10;
    private Camera _camera;
    private void Start() 
    {
        _camera = Camera.main;

        if (isLocalPlayer)
        {
            _camera.transform.SetParent(this.transform);
            _camera.transform.localPosition = new Vector3(0, Y_LOCAL_POSITION, Z_LOCAL_POSITION);
        }
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
