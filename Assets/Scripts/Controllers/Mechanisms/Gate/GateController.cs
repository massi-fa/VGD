using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GateController : MechanismController
{
    public GameObject gate;
    private Animation _gateAnimation;
    private string[] animationNames;
    private int _currentIndex = 0;
    

    private void Start()
    {
        _gateAnimation = gate.GetComponent<Animation>();

        animationNames = new string[_gateAnimation.GetClipCount()];
        foreach (AnimationState clip in _gateAnimation)
        {
            animationNames[_currentIndex] = clip.name;
            _currentIndex++;
        }

        _currentIndex = 0;
    }

    public override void Active()
    {
        base.Active();
        ManageGate();
    }

    private void ManageGate()
    {
        if (_gateAnimation.isPlaying) return;
        
        _gateAnimation.PlayQueued(animationNames[_currentIndex], QueueMode.PlayNow);
        _currentIndex = (_currentIndex + 1) % animationNames.Length;
    }
}
