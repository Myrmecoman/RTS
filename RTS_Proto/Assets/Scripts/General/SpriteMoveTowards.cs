using UnityEngine;

public class SpriteMoveTowards : MonoBehaviour
{
    public float speed = 5;
    public double enableTime = 3;

    private double currTime = 0;


    void Update()
    {
        if (currTime > 0)
        {
            transform.rotation = transform.rotation * Quaternion.Euler(Vector3.forward * speed * Time.deltaTime);
            currTime -= Time.deltaTime;
            return;
        }

        currTime = enableTime;
        gameObject.SetActive(false);
    }
}
