using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    internal GameCharacterController gameCharacterController;
    private GameObject rootParent;
    protected internal string[] validTags;

    private void Start()
    {
        GameObject parent = gameObject;
        
        while (parent != null && parent.CompareTag("Untagged")) 
            parent = parent.transform.parent.gameObject;
        
        rootParent = parent;
        gameCharacterController = rootParent.GetComponent<GameCharacterController>();

        if (rootParent.CompareTag("Player"))
            validTags = new [] { "Enemy", "mechanism" };
        else if (rootParent.CompareTag("Enemy"))
            validTags = new [] { "Player" };
        else if (rootParent.CompareTag("Trap"))
            validTags = new [] { "Player" };

    }

}
