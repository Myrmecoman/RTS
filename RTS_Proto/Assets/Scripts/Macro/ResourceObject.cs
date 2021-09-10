using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public GameObject moveTowardsSprite;


    public void MoveTowardsSprite()
    {
        moveTowardsSprite.SetActive(true);
    }
}
