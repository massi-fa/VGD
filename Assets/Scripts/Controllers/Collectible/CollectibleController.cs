using System.Collections;
using UnityEngine;


public class CollectibleController : MonoBehaviour
{
    private GameObject _rootParent;

    protected virtual void Start()
    {
        GameObject parent = gameObject;

        while (parent != null && !parent.CompareTag("Collectible"))
            parent = parent.transform.parent.gameObject;

        _rootParent = parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Se il player ha toccato il collezionabile
        if (!other.CompareTag("Player")) return;

        // Attiva l'evento del collezionabile
        var c = other.gameObject.GetComponent<GameCharacterController>();
        foreach (var myRenderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            myRenderer.enabled = false;
        }

        //
        ActionPerformed(c);

        // Distrugge il collezionabile
        StartCoroutine(Die());
    }

    protected virtual void ActionPerformed(GameCharacterController gameCharacterController)
    {
    }

    protected virtual IEnumerator Die()
    {
        yield return null;
        Destroy(_rootParent);
    }
}