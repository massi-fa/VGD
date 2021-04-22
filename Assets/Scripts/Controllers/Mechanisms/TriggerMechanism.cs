using UnityEngine;
using System.Linq;

public class TriggerMechanism : MonoBehaviour
{
    public GameObject mechanismLinked;

    private MechanismController _mechanismController;
    
    private MeleeWeapon _enemyWeapon;

    public Animation _animation;
    
    // Start is called before the first frame update
    void Start()
    {
        _mechanismController = mechanismLinked.GetComponent<MechanismController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == null || other.gameObject == null)
            return;

        
        _enemyWeapon = other.gameObject.GetComponent<MeleeWeapon>();

        if (_enemyWeapon != null && _enemyWeapon.validTags.Contains(gameObject.tag))
        {
            _enemyWeapon.gameCharacterController.TargetTouched(_enemyWeapon.name, gameObject);
        }
    }

    public void IsTriggered()
    {
        _animation.Play(PlayMode.StopAll);
        _mechanismController.Active();
    }


}
