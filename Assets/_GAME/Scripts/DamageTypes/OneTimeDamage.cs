using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeDamage : IInflictDamage
{
    public void InflictDamage(GameObject target, int[] damages)
    {
        target.GetComponent<IHeath>().TakeDamage(damages[0]);
    }
}
