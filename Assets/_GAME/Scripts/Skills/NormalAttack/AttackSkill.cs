using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class AttackSkill : NetworkBehaviour
{
    [SerializeField] protected int[] _skillDamges;
    [SerializeField] protected LayerMask _canAttackLayer;
    protected IInflictDamage _inflictDamageType;
    public int ObjectId;

    public abstract void InitInflictDamageType();
    public abstract void AttackTo(NetworkConnectionToClient target, GameObject skillObject);

    private void Awake() 
    {
        InitInflictDamageType();
    }
}
