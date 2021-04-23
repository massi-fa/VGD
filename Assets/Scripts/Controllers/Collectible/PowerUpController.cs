using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;

public class PowerUpController : CollectibleController
{
    public enum BonusType
    {
        Damage,
        Armour,
        Speed,
        Heal
    };
    public BonusType bonusType;
    public int bonusValue = 10;
    public int bonusSeconds = 10;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        if (bonusType == BonusType.Heal)
            bonusSeconds = 0;
    }

    protected override void ActionPerformed(GameCharacterController c)
    {
        base.ActionPerformed(c);
        
        
        switch (bonusType)
        {
            case BonusType.Damage:
                StartCoroutine(c.TemporaneousDamageBuff(bonusValue, bonusSeconds));
                break;
                
            case BonusType.Armour:
                StartCoroutine(c.TemporaneousArmourBuff(bonusValue, bonusSeconds));
                break;
                
            case BonusType.Speed:
                StartCoroutine(c.TemporaneousSpeedBuff(bonusValue, bonusSeconds));
                break;                
            case BonusType.Heal:
                c.TakeHitPoints(bonusValue);
                break;
        }
    }

    protected override IEnumerator Die()
    {
        yield return Waiter.Active(bonusSeconds);
        yield return base.Die();
    }
}
