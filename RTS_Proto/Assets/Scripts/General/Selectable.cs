using Unity.Collections;
using UnityEngine;


public abstract class Selectable : MonoBehaviour
{
    public Names unitName;
    public SpriteRenderer sprite;
    public GameObject moveTowardsSprite;
    public bool isWorker = false;
    public bool isAlly = true;

    [HideInInspector] public DijkstraTile[] path;
    [HideInInspector] public DijkstraTile[] pathImprecise;
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public bool isFullPath = false;

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


    public void FullPathDone(NativeArray<DijkstraTile> path)
    {
        NativeArray<DijkstraTile>.Copy(path, this.path);
        isFullPath = true;
    }


    public abstract void HoldPosition();


    public abstract void AddDestination(NativeArray<DijkstraTile> pathImprecise, Vector3 dest, Transform follow = null, Actions action = Actions.MOVE, ResourceManager res = null);
}
