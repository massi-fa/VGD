using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HitColliderController : MonoBehaviour
{
    GameCharacterController gameCharacterController;
    string[] validTags;

    private void Start()
    {
        Transform transformRoot = transform.root;
        GameObject characterGameObject = transformRoot.gameObject;

        //print("Io sono l'arma " + gameObject.name + " di " + characterGameObject.name + " con tag " + characterGameObject.tag);
        if (characterGameObject.CompareTag("Player"))
            validTags = new string[] { "Enemy", "mechanism" };
        else if (characterGameObject.CompareTag("Enemy"))
            validTags = new string[] { "Player" };
        gameCharacterController = characterGameObject.GetComponent<GameCharacterController>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (validTags.Contains(other.tag) || validTags.Contains(other.gameObject.tag) || validTags.Contains(other.transform.root.gameObject.tag))
        {
            if (validTags.Contains("Enemy"))
            {
                print("other.tag: " + other.tag);
                print("other.gameObject.tag: " + other.gameObject.tag);
                print("other.transform.gameObject.tag " + other.transform.gameObject.tag);
                print("other.transform.root.gameObject.tag " + other.transform.root.gameObject.tag);
            }
            gameCharacterController.TargetTouched(gameObject.name, other.transform.root.gameObject);
        }
    }
}
