using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2InControllers : MonoBehaviour
{
    public float CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;
    Vector3 FollowPOS;
    public float clampAngle = 80.0f;
    [Tooltip("Sensibilità nella rotazione del mouse")]
    [Min(1f)]
    public float inputSensitivity = 150.0f;
    public float camDistanceXToPlayer;
    public float camDistanceYToPlayer;
    public float camDistanceZToPlayer;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    public float smoothY;
    public float rotY = 0.0f;
    public float rotX = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = 0.0f;//Input.GetAxis("Mouse Y");

        rotY += mouseX*inputSensitivity*Time.deltaTime;
        rotX += mouseY*inputSensitivity*Time.deltaTime;

        rotX = Mathf.Clamp (rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler (rotX, rotY ,0.0f);
        transform.rotation = localRotation;
    }

    void LateUpdate () {
        CameraUpdater ();
    }

    void CameraUpdater () {
        Transform target = CameraFollowObj.transform;
        float step = CameraMoveSpeed*Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
