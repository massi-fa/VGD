using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;

public class CharacterCollisionController : MonoBehaviour
{
    private MeleeWeapon enemyWeapon;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || other.gameObject == null)
            return;

        enemyWeapon = other.gameObject.GetComponent<MeleeWeapon>();
        if (enemyWeapon != null && enemyWeapon.validTags.Contains(gameObject.tag))
        {
            enemyWeapon.gameCharacterController.TargetTouched(enemyWeapon.name, gameObject);
        }
    }
}
