using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LayerMask WallMask; // This is the mask that the program will look for when trying to find obstructions to the path
    public Vector2 vGridWorldSize; // A vector2 to store the width and height of the graph in world units
    public float fNodeRadius; // This stores how big each square on the graph will be
    public WorldGrid singleGrid; // Grid parameters
    public int minerals = 0;
    public int gas = 0;

    private static int arraysSize = 100;
    [HideInInspector] public WorldGrid[] grids = new WorldGrid[arraysSize];
    [HideInInspector] public int[] inUse = new int[arraysSize];

    [HideInInspector] public KdTree<Transform> allyUnits;
    [HideInInspector] public KdTree<Transform> allyBuildings;
    [HideInInspector] public KdTree<Transform> enemyUnits;
    [HideInInspector] public KdTree<Transform> enemyBuildings;


    private void Awake()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        allyUnits = new KdTree<Transform>();
        allyBuildings = new KdTree<Transform>();
        enemyUnits = new KdTree<Transform>();
        enemyBuildings = new KdTree<Transform>();

        for (int i = 0; i < arraysSize; i++)
        {
            grids[i] = Instantiate(singleGrid, transform);
            grids[i].WallMask = WallMask;
            grids[i].vGridWorldSize = vGridWorldSize;
            grids[i].fNodeRadius = fNodeRadius;
            grids[i].gridId = i;
        }
    }


    private void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                GameObject obj = (GameObject)Instantiate(Resources.Load("Worker"), new Vector3(x, 0, y), Quaternion.identity);
                allyUnits.Add(obj.transform);
            }
        }

        /*
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                GameObject obj = (GameObject)Instantiate(Resources.Load("Agent"), new Vector3(x, 0, y + 30), Quaternion.identity);
                obj.GetComponent<AgentManager>().isAlly = false;
                enemyUnits.Add(obj.transform);
            }
        }
        */
    }


    public void AttackCommand(Dictionary<int, Selectable> agents, Transform target, bool focus = false)
    {
        for (int i = 0; i < arraysSize; i++)
        {
            if (inUse[i] == 0)
            {
                int actualAgents = 0;
                grids[i].ChangeTarget(target);

                foreach (KeyValuePair<int, Selectable> ag in agents)
                {
                    if (ag.Value.GetComponent<AgentManager>())
                    {
                        actualAgents++;

                        if (focus)
                            ag.Value.AddDestination(grids[i], i, target, 1);
                        else
                            ag.Value.AddDestination(grids[i], i, null, 1);
                    }
                }

                inUse[i] = actualAgents;

                return;
            }
        }

        Debug.Log("All grids already in use");
    }


    public void PatrolCommand(Dictionary<int, Selectable> agents, Transform target)
    {
        for (int i = 0; i < arraysSize; i++)
        {
            if (inUse[i] == 0)
            {
                int actualAgents = 0;
                grids[i].ChangeTarget(target);

                foreach (KeyValuePair<int, Selectable> ag in agents)
                {
                    if (ag.Value.GetComponent<AgentManager>())
                    {
                        actualAgents++;
                        ag.Value.AddDestination(grids[i], i, null, 2);
                    }
                }

                inUse[i] = actualAgents;

                return;
            }
        }

        Debug.Log("All grids already in use");
    }


    public void MoveCommand(Dictionary<int, Selectable> agents, Transform target, bool follow = false, bool resource = false)
    {
        for (int i = 0; i < arraysSize; i++)
        {
            if (inUse[i] == 0)
            {
                inUse[i] = agents.Count;
                grids[i].ChangeTarget(target);

                foreach (KeyValuePair<int, Selectable> ag in agents)
                {
                    if (follow)
                        ag.Value.AddDestination(grids[i], i, target);
                    else if (resource && ag.Value.isWorker)
                    {
                        ag.Value.AddDestination(grids[i], i, null, 3, target.GetComponent<ResourceManager>());
                        target.GetComponent<ResourceManager>().MoveTowardsSprite();
                    }
                    else
                        ag.Value.AddDestination(grids[i], i);
                }

                return;
            }
        }

        Debug.Log("All grids already in use");
    }


    public int NbCurrentlyFree()
    {
        int total = 0;
        foreach (int i in inUse)
        {
            if (i == 0)
                total++;
        }
        return total;
    }


    #if (UNITY_EDITOR)
    // Function that draws the wireframe
    private void OnDrawGizmos()
    {
        // Draw a wire cube with the given dimensions from the Unity inspector
        Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y));
    }
    #endif
}
