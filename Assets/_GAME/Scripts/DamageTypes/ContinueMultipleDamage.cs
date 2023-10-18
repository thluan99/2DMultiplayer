using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ContinueMultipleDamage : IInflictDamage
{
    public void InflictDamage(GameObject target, int[] damages)
    {
        AsyncDamage(target, damages);
    }

    async void AsyncDamage(GameObject target, int[] damages)
    {
        for (int i = 0; i < damages.Length; i++)
        {
            target.GetComponent<IHeath>().TakeDamage(damages[i]);
            await Task.Delay(1000);
        }
    }
}
