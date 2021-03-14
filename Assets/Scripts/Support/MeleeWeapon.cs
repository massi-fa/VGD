using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeleeWeapon : MonoBehaviour
{
    public GameCharacterController gameCharacterController;
    public GameObject rootParent;
    public string[] validTags;

    private void Start()
    {
        GameObject parent = gameObject;
        
        while (parent != null && parent.CompareTag("Untagged")) 
            parent = parent.transform.parent.gameObject;
        
        rootParent = parent;
        gameCharacterController = rootParent.GetComponent<GameCharacterController>();

        //print("Io sono l'arma " + gameObject.name + " di " + characterGameObject.name + " con tag " + characterGameObject.tag);
        if (rootParent.CompareTag("Player"))
            validTags = new [] { "Enemy", "mechanism" };
        else if (rootParent.CompareTag("Enemy"))
            validTags = new string[] { "Player" };
    }

}
