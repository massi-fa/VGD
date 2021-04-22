using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float distanceBeforeDie = 10f;

    public float speed = 20f;

    private Vector3 _direction;

    private GameObject _player;

    // Start is called before the first frame update
    private void Start()
    {
        _player = PlayerTracker.instance.player;

        var tmp = _player.transform.position;
        tmp.y+= _player.GetComponent<CharacterController>().height / 2;
        _direction = (tmp - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(_direction.x, 0f, _direction.z));
        
        transform.SetParent(null, true);

    }

    // Update is called once per frame
    private void Update()
    {
        var currenDistance = Time.deltaTime * speed;

        var tmp = transform;
        tmp.position += currenDistance * tmp.forward;
       
       distanceBeforeDie -= currenDistance;
        if(distanceBeforeDie<=0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
