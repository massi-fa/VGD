using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterCollisionController : MonoBehaviour
{
    private MeleeWeapon enemyWeapon;

    private void OnTriggerEnter(Collider other)
    {
       // print("Protagonist hitF #1");
        if (other == null || other.gameObject == null)
            return;
        
     //   print("Protagonist hitF #2");
        enemyWeapon = other.gameObject.GetComponent<MeleeWeapon>();
        if (enemyWeapon == null  || !enemyWeapon.validTags.Contains(gameObject.tag))
            return;
        
        
      //  print("Protagonist hitF #3");
        
        enemyWeapon.rootParent.GetComponent<GameCharacterController>().TargetTouched(enemyWeapon.name, gameObject);

    }
}
