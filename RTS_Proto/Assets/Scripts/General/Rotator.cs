using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 5;

    void Update()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(Vector3.forward * speed * Time.deltaTime);
    }
}
