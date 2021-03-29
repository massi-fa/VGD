using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceWeapon : MonoBehaviour
{
    public GameObject projectile;
    public Transform spawnTransform;

    public AnimationClip _animation;
    // private float repeat = 1.5f;

    private Animator _animator;

    private GameObject root;

    // Start is called before the first frame update
    void Start()
    {
        root = gameObject;
        while (root != null && root.CompareTag("Untagged"))
            root = root.transform.parent.gameObject;

        _animator = root.GetComponent<Animator>();
    }

    public void ShootNow()
    {
        Instantiate(projectile, spawnTransform.position, Quaternion.identity, spawnTransform);
    }
}