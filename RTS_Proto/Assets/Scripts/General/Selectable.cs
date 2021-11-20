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
    public abstract void AddDestination(WorldGrid grid, int index, Transform follow = null, int action = 0 /* 1 = attack, 2 = patrol, 3 = collect-resource */, ResourceManager res = null);
}
