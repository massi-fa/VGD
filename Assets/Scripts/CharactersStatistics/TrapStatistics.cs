using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapStatistics : CharacterStatistics {
    protected override void Start()
    {
        maxHp = 1;
        base.Start();
    }
}
