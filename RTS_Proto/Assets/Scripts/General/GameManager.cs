using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LayerMask WallMask; // This is the mask that the program will look for when trying to find obstructions to the path
    public Vector2 vGridWorldSize; // A vector2 to store the width and height of the graph in world units
    public float fNodeRadius; // This stores how big each square on the graph will be
    public WorldGrid singleGrid;

    [HideInInspector] public int minerals = 0;
    [HideInInspector] public int gas = 0;

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

            #if (UNITY_EDITOR)
            if (i == 0)
                grids[i].debugNb = 1;
            #endif
        }
    }


    private void Start()
    {
        // For test purposes
        for (float i = 0; i < 15; i++)
        {
            for (float j = 0; j < 15; j++)
            {
                GameObject obj = (GameObject) Instantiate(Resources.Load("Agent"), new Vector3(i * 1.5f, 0.5f, j * 1.5f), Quaternion.identity);
                allyUnits.Add(obj.transform);
            }
        }

        for (float i = 0; i < 15; i++)
        {
            for (float j = 0; j < 15; j++)
            {
                GameObject obj = (GameObject)Instantiate(Resources.Load("Agent"), new Vector3(i * 1.5f, 0.5f, j * 1.5f + 30), Quaternion.identity);
                obj.GetComponent<AgentManager>().isAlly = false;
                enemyUnits.Add(obj.transform);
            }
        }
    }


    public void AttackCommand(Dictionary<int, AgentManager> agents, Transform target, bool focus = false)
    {
        for (int i = 0; i < arraysSize; i++)
        {
            if (inUse[i] == 0)
            {
                inUse[i] = agents.Count;
                grids[i].ChangeTarget(target);

                foreach (KeyValuePair<int, AgentManager> ag in agents)
                {
                    ag.Value.UnsetDestination();

                    if (focus)
                        ag.Value.AddDestination(grids[i], i, target, 1);
                    else
                        ag.Value.AddDestination(grids[i], i, null, 1);
                }

                return;
            }
        }

        Debug.Log("All grids already in use");
    }


    public void PatrolCommand(Dictionary<int, AgentManager> agents, Transform target)
    {
        for (int i = 0; i < arraysSize; i++)
        {
            if (inUse[i] == 0)
            {
                inUse[i] = agents.Count;
                grids[i].ChangeTarget(target);

                foreach (KeyValuePair<int, AgentManager> ag in agents)
                {
                    ag.Value.UnsetDestination();

                    ag.Value.AddDestination(grids[i], i, null, 2);
                }

                return;
            }
        }

        Debug.Log("All grids already in use");
    }


    public void MoveCommand(Dictionary<int, AgentManager> agents, Transform target, bool follow = false)
    {
        for (int i = 0; i < arraysSize; i++)
        {
            if (inUse[i] == 0)
            {
                inUse[i] = agents.Count;
                grids[i].ChangeTarget(target);

                foreach (KeyValuePair<int, AgentManager> ag in agents)
                {
                    ag.Value.UnsetDestination();

                    if (follow)
                        ag.Value.AddDestination(grids[i], i, target);
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
