using UnityEngine;

public class PushPlayer : MonoBehaviour
{
    private GameObject _rootParent;
    private TrapController _trapController;
    private void Start()
    {
        _rootParent = gameObject;
        while (!_rootParent.CompareTag("Trap") && _rootParent.transform.parent != null)
            _rootParent = _rootParent.transform.parent.gameObject;

        _trapController = _rootParent.GetComponent<TrapController>();
    }

    public void OnTriggerStay(Collider col)
    {
        GameObject owner = col.gameObject;
        if (!owner.CompareTag("Player"))
            return;
         
        _trapController.HasTouchedPlayer("PushAction");
    }
}