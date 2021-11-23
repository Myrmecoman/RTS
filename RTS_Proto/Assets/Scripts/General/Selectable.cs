using Unity.Collections;
using UnityEngine;


public abstract class Selectable : MonoBehaviour
{
    public Names unitName;
    public SpriteRenderer sprite;
    public GameObject moveTowardsSprite;
    public bool isWorker = false;
    public bool isAlly = true;


    protected bool IsSameObj(int objID)
    {
        return gameObject.GetInstanceID() == objID;
    }


    public void Select()
    {
        sprite.enabled = true;
    }


    public void UnSelect()
    {
        sprite.enabled = false;
    }


    public void MoveTowardsSprite()
    {
        moveTowardsSprite.SetActive(true);
    }


    public abstract void HoldPosition();


    public abstract void AddDestination(NativeArray<DijkstraTile> path, NativeArray<DijkstraTile> pathImprecise, Vector3 dest, Transform follow = null, int action = 0, ResourceManager res = null); // action : 1 = attack, 2 = patrol, 3 = collect-resource */
}
