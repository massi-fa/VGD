using UnityEngine;

public class CharacterStatistics : MonoBehaviour
{
    [Header("Valori iniziali")]
    [Tooltip("Vita massima/di partenza del soggetto")]
    [Min(0)]
    public int maxHp;

    [Tooltip("Armatura di partenza del soggetto")]
    [Min(0)]
    public int baseArmour;
    [Tooltip("Armatura di partenza del soggetto")]
    [Min(0)]    
    public int baseDamage;

    [Header("Valori reali")]
    [Tooltip("NON TOCCARE")]
    public int hp;
    [Tooltip("NON TOCCARE")]
    public int armour;
    [Tooltip("NON TOCCARE")]
    public int damage;

    protected virtual void Start()
    {
        hp = maxHp;
        armour = baseArmour;
        damage = baseDamage;
    }
}
