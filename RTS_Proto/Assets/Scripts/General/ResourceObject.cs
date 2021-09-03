using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public GameObject moveTowardsSprite;
    public float heightFromCenter;


    public void MoveTowardsSprite()
    {
        moveTowardsSprite.SetActive(true);
    }
}
