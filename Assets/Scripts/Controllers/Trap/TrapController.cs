using System.Collections;
using UnityEngine;

public class TrapController : GameCharacterController
{
    public int delayBeforeStartingAnimation;
    public float pushForce;

    private Vector3 _direction;
    private Vector3 _destination;
    private bool _hasHitSomething;
    
    private GameObject _player;
    
    //protected string lastTargetName;
    private float _lastPushTime;
    public float countdownPush = 1.0f;
    private Animation _animation;
    public float speedAnimation = 1.0f;
    public float speedForReachingDestinationAfterPushed = 10.0f;
    private CharacterController _characterController;

    protected override void Start()
    {
        base.Start();
        
        
        isAnimated = false;
        
        // Traccia il player
        _player = PlayerTracker.instance.player;
        _characterController = _player.GetComponent<CharacterController>();

        _animation = GetComponent<Animation>();
        
        foreach (AnimationState animationState in _animation)
        {
            animationState.speed = speedAnimation;
        }

        StartCoroutine(PlayAnimationAfterDelay());
    }

    private IEnumerator PlayAnimationAfterDelay()
    {
        yield return Waiter.Active(delayBeforeStartingAnimation);
        _animation.Play(PlayMode.StopAll);
    }

    public void HasTouchedPlayer(string action)
    {
        if (!action.Equals("PushAction")) return;
        float currentTime = Time.time;
        if (!(currentTime - _lastPushTime > countdownPush)) return;
        
        _hasHitSomething = true;
        var position = _player.transform.position;
        Vector3 diff =  position - transform.position;
        _direction = new Vector3(diff.x, 0f, diff.z).normalized;
        _destination = position;
        _destination += _direction * pushForce;
        _lastPushTime = currentTime;
    }
    
    protected override void Update()
    {
        base.Update();
        if (!_hasHitSomething) return;

        //var magnitudeVecBeforeUpdate = Vector3.Distance(player.transform.position, destination);
        var vecBeforeUpdate = _destination - _player.transform.position;
        if (vecBeforeUpdate.magnitude < 3)
        {
            _hasHitSomething = false;
            return;
        }
        var magnitudeVecAfterUpdate =  (speedForReachingDestinationAfterPushed * Time.deltaTime * pushForce);

        if (magnitudeVecAfterUpdate > vecBeforeUpdate.magnitude)
            magnitudeVecAfterUpdate = vecBeforeUpdate.magnitude;

        _direction = new Vector3(vecBeforeUpdate.x, 0f, vecBeforeUpdate.z);
        _characterController.Move(_direction * magnitudeVecAfterUpdate);

    }
}
