using UnityEngine;


public abstract class Selectable : MonoBehaviour
{
    public Names unitName;
    public SpriteRenderer sprite;
    public GameObject moveTowardsSprite;
    public bool isWorker = false;
    public bool isAlly = true;

    protected Vector3 destination;
    protected int gridId;
    protected int calculatorId;

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


    public abstract void AddDestination(int gridId, int calculatorId, Vector3 dest, Transform follow = null, Actions action = Actions.MOVE, ResourceManager res = null);
}
