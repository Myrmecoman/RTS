using Unity.Collections;
using Unity.Mathematics;
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

    protected Vector3 destination;
    protected int gridId;
    protected int calculatorId;
    protected NativePriorityQueue<int2> frontier;
    protected NativeHashMap<int2, int2> parents;
    protected NativeHashMap<int2, int> costs;
    protected NativeList<int2> neighbours;
    protected NativeList<int2> output;

    private void Awake()
    {
        frontier = new NativePriorityQueue<int2>(PathRegister.instance.iGridSizeX * PathRegister.instance.iGridSizeY, Allocator.Persistent);
        parents = new NativeHashMap<int2, int2>(PathRegister.instance.iGridSizeX * PathRegister.instance.iGridSizeY, Allocator.Persistent);
        costs = new NativeHashMap<int2, int>(PathRegister.instance.iGridSizeX * PathRegister.instance.iGridSizeY, Allocator.Persistent);
        neighbours = new NativeList<int2>(PathRegister.instance.iGridSizeX * PathRegister.instance.iGridSizeY, Allocator.Persistent);
        output = new NativeList<int2>(PathRegister.instance.iGridSizeX * PathRegister.instance.iGridSizeY, Allocator.Persistent);
    }


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
