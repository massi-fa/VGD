using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    internal GameCharacterController gameCharacterController;
    private GameObject _rootParent;
    protected internal string[] validTags;

    private void Awake()
    {
        GameObject parent = gameObject;
        
        while (parent != null && parent.CompareTag("Untagged")) 
            parent = parent.transform.parent.gameObject;
        
        _rootParent = parent;
        gameCharacterController = _rootParent.GetComponent<GameCharacterController>();

        if (_rootParent.CompareTag("Player"))
            validTags = new [] { "Enemy", "Mechanism" };
        else if (_rootParent.CompareTag("Enemy"))
            validTags = new [] { "Player" };
        else if (_rootParent.CompareTag("Trap"))
            validTags = new [] { "Player" };
    }

}
