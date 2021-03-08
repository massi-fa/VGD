using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatistics : CharacterStatistics
{
    public override void Die()
    {
        base.Die();

        // Animazione

        // Distruggi il gameobject
        Destroy(gameObject);
    }
}
