using System.Collections;
using UnityEngine;

public class TrapController : GameCharacterController
{
    public int delayBeforeStartingAnimation = 0;
    public float pushForce;

    private Vector3 direction;
    private Vector3 destination;
    private bool hasHitSomething;
    
    private GameObject player;
    
    //protected string lastTargetName;
    private float lastPushTime;
    public float countdownPush = 1.0f;
    private Animation _animation;
    public float speedAnimation = 1.0f;
    public float speedForReachingDestinationAfterPushed = 10.0f;
    protected override void Start()
    {
        base.Start();
        isAnimated = false;
        player = PlayerTracker.instance.player;

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
        if (!(currentTime - lastPushTime > countdownPush)) return;
        
        hasHitSomething = true;
        var position = player.transform.position;
        Vector3 diff =  position - transform.position;
        direction = new Vector3(diff.x, 0f, diff.z).normalized;
        destination = position;
        destination += direction * pushForce;
        lastPushTime = currentTime;
    }
    
    protected override void Update()
    {
        base.Update();
        if (!hasHitSomething) return;
        
        /*player.transform.position = Vector3.Lerp(
            player.transform.position,
            destination,
            Time.deltaTime*25f
        );*/
        //var y = Vector3.Distance(player.transform.position, destination);
        var motionVectBeforeUpdate = Vector3.Distance(player.transform.position, destination);
        if (motionVectBeforeUpdate < 3)
        {
            hasHitSomething = false;
            return;
        }
        var motionVecAfterUpdate =  (speedForReachingDestinationAfterPushed * Time.deltaTime * pushForce);
        //if (Vector3.Distance(motionVecAfterUpdate, destination) > y)
        //    motionVecAfterUpdate = direction * y;
        if (motionVecAfterUpdate > motionVectBeforeUpdate)
            motionVecAfterUpdate = motionVectBeforeUpdate;
        
        player.GetComponent<CharacterController>().Move(direction * motionVecAfterUpdate);
        //player.transform.position = destination;

    }
}
