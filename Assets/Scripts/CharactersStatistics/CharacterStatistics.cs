using System.Collections;
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
    public void TakeHitPoints(int bonusHp) {
        hp += bonusHp;
        if (hp > maxHp)
            hp = maxHp;
    }


    public void UpgradeArmour(int bonusArmour) {
        armour += bonusArmour;
    }

    public void UpgradeDamage(int bonusDamange)
    {
        damage += bonusDamange;
    }
    #endregion

    #region TemporaneousBuffs
    public IEnumerator TemporaneousDamageBuff(int bonusDamage, int seconds) {
        damage += bonusDamage;
        yield return Waiter.Active(seconds);
        damage -= bonusDamage;
    }

    public IEnumerator TemporaneousArmourBuff(int bonusArmour, int seconds) {
        armour += bonusArmour;
        yield return Waiter.Active(seconds);
        armour -= bonusArmour;
    }
    #endregion
}
