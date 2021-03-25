using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public  float smoothSpeed = 0.5f;
    public Vector3 offset;
    
    public bool LookAtPlayer = false;
    public bool RotateAroundPlayer = true;
    public float RotationSpeed = 5.0f;
    void Start () {
        offset = transform.position - target.position;
    }
    void Update() {

        if(RotateAroundPlayer){
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
            offset = camTurnAngle * offset;
        }

        Vector3 newPos = target.position + offset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothSpeed);

        if (LookAtPlayer || RotateAroundPlayer){
            transform.LookAt(target);
        }
    }
}
