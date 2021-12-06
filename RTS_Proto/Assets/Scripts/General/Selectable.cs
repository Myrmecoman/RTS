using Unity.Collections;
using UnityEngine;


public abstract class Selectable : MonoBehaviour
{
    public Names unitName;
    public SpriteRenderer sprite;
    public GameObject moveTowardsSprite;
    public bool isWorker = false;
    public bool isAlly = true;
    public int health = 50;
    public int armor = 0;
    public int sightRange = 10;

    protected NativeArray<DijkstraTile> customGrid;
    protected bool customUsed = false;
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


    public abstract void GetAttacked(int dmg);


    public abstract void AddDestination(int gridId, int calculatorId, Vector3 dest, Transform follow = null, Actions action = Actions.MOVE, ResourceManager res = null);
}
