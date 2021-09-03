using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public float diameter;

    public void MoveTowardsSprite()
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load("moveTowards"), transform);
        obj.transform.localScale = new Vector3(diameter / 4, diameter / 4, diameter / 4);
    }
}
