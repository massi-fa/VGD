using UnityEngine;
using System.Linq;

public class CharacterCollisionController : MonoBehaviour
{
    private MeleeWeapon _enemyWeapon;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || other.gameObject == null)
            return;

        _enemyWeapon = other.gameObject.GetComponent<MeleeWeapon>();
        if (_enemyWeapon != null && _enemyWeapon.validTags.Contains(gameObject.tag))
        {
            _enemyWeapon.gameCharacterController.TargetTouched(_enemyWeapon.name, gameObject);
        }
    }
}
