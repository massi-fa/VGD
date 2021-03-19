using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatistics : MonoBehaviour
{
    public int maxHp;

    public int baseArmour;
    public int baseDamage;

    public int hp;
    public int armour;
    public int damage;

    protected virtual void Start()
    {
        hp = maxHp;
        armour = baseArmour;
        damage = baseDamage;
    }


  /*  private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            StartCoroutine(TemporaneousDamageBuff(100, 10));
        else if (Input.GetKeyDown(KeyCode.R))
                StartCoroutine(TemporaneousArmourBuff(100, 10));
    }*/
  

    #region FixedBuffs
    public void TakeHitPoints(int bonus_hp) {
        hp += bonus_hp;
        if (hp > maxHp)
            hp = maxHp;
    }


    public void UpgradeArmour(int bonus_armour) {
        armour += bonus_armour;
    }

    public void UpgradeDamage(int bonus_damange)
    {
        damage += bonus_damange;
    }
    #endregion

    #region TemporaneousBuffs
    public IEnumerator TemporaneousDamageBuff(int bonus_damage, int seconds) {
        damage += bonus_damage;
        yield return Waiter.Active(seconds);
        damage -= bonus_damage;
    }

    public IEnumerator TemporaneousArmourBuff(int bonus_armour, int seconds) {
        armour += bonus_armour;
        yield return Waiter.Active(seconds);
        armour -= bonus_armour;
    }
    #endregion
}
