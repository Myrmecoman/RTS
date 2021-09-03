using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public double timeBeforeDeath = 3;


    void Update()
    {
        timeBeforeDeath -= Time.deltaTime;
        if (timeBeforeDeath <= 0)
            Destroy(gameObject);
    }
}
