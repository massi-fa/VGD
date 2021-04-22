using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperController : MonoBehaviour
{
    public float powerJumper = 2f;

    private ProtagonistController _playerController;

    public float countDown = 0.3f;

    private float _lastTime = 0;

    // Start is called before the first frame update
    private void Start()
    {
        _playerController = PlayerTracker.instance.player.GetComponent<ProtagonistController>();
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        var currentTime = Time.time;

        if (!(currentTime - _lastTime > countDown)) return;


        _lastTime = currentTime;
        _playerController.CollisionWithJumper(powerJumper);
    }
}