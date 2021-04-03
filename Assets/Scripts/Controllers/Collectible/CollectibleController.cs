using System.Collections;
using UnityEngine;


public class CollectibleController : MonoBehaviour
{
    
    private GameObject _rootParent;

    public enum BonusType
    {
        Damage,
        Armour,
        Heal
    };
    public BonusType bonusType;
    public int bonusValue = 10;
    public int bonusSeconds = 10;
    
    private void Start()
    {
        GameObject parent = gameObject;

        while (parent != null && !parent.CompareTag("Collectible")) 
            parent = parent.transform.parent.gameObject;

        _rootParent = parent;

        if (bonusType == BonusType.Heal)
            bonusSeconds = 0;

    }

    private void OnTriggerEnter(Collider other)
    {
        // Se il player ha toccato il collezionabile
        if (other.CompareTag("Player"))
        {

            // Attiva l'evento del collezionabile
            var c = other.gameObject.GetComponent<GameCharacterController>();
            foreach (var myRenderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                myRenderer.enabled = false;
            }

            switch (bonusType)
            {
                case BonusType.Damage:
                    StartCoroutine(c.TemporaneousDamageBuff(bonusValue, bonusSeconds));
                    break;
                
                case BonusType.Armour:
                    StartCoroutine(c.TemporaneousArmourBuff(bonusValue, bonusSeconds));
                    break;
                
                case BonusType.Heal:
                    c.TakeHitPoints(bonusValue);
                    break;
            }

            // Distrugge il collezionabile
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        yield return Waiter.Active(bonusSeconds);
        Destroy(_rootParent);
    }
}
