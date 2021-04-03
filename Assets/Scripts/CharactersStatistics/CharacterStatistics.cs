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
}
