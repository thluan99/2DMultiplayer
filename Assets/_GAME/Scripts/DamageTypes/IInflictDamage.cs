using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInflictDamage
{
    public void InflictDamage(GameObject target, int[] damages);
}
