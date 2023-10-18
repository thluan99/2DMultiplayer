using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SwordRain : AttackSkill
{
    [SerializeField] private int _numberSwords = 1;
    [SerializeField] private GameObject _sword;
    private void Start() 
    {
        for (int i = 0; i < _numberSwords; i++)
        {
            GameObject sw = Instantiate(_sword, transform);
            sw.transform.localPosition = new Vector3(0, 2);
            sw.GetComponent<Rigidbody2D>().gravityScale = 0;
            sw.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, -1) * 5, ForceMode2D.Impulse);
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

    [TargetRpc]
    public override void AttackTo(NetworkConnectionToClient target, GameObject skillObject)
    {
        
    }

    public override void InitInflictDamageType()
    {
        _inflictDamageType = new ContinueMultipleDamage();
    }
}
