using UnityEngine;

public class CollectibleRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f,0f,1f) * (Time.deltaTime * this.rotationSpeed));
    }
}
