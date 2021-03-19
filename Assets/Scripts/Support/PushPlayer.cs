using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlayer : MonoBehaviour
{
    private GameObject rootParent;
    private TrapController trapController;
    private void Start()
    {
        rootParent = gameObject;
        while (!rootParent.CompareTag("Trap") && rootParent.transform.parent != null)
            rootParent = rootParent.transform.parent.gameObject;

        trapController = rootParent.GetComponent<TrapController>();
    }

    public void OnTriggerStay(Collider col)
    {
        GameObject owner = col.gameObject;
        if (!owner.CompareTag("Player"))
            return;
         
        trapController.HasTouchedPlayer("PushAction");
    }
}