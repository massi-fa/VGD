using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float distanceBeforeDie = 10f;

    public float speed = 20f;

    private Vector3 direction;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerTracker.instance.player;

        var tmp = player.transform.position;
        tmp.y+= player.GetComponent<CharacterController>().height / 2;
        direction = (tmp - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        
        transform.SetParent(null, true);

    }

    // Update is called once per frame
    void Update()
    {
        float currenDistance = Time.deltaTime * speed;

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
