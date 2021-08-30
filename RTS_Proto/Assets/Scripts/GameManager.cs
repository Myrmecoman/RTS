using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public LayerMask WallMask; // This is the mask that the program will look for when trying to find obstructions to the path
    public Vector2 vGridWorldSize; // A vector2 to store the width and height of the graph in world units
    public float fNodeRadius; // This stores how big each square on the graph will be
    public WorldGrid singleGrid;

    private static int arraysSize = 100;
    private WorldGrid[] grids = new WorldGrid[arraysSize];
    [HideInInspector] public int[] inUse = new int[arraysSize];


    private void Awake()
    {
        for (int i = 0; i < arraysSize; i++)
        {
            grids[i] = Instantiate(singleGrid, transform);
            grids[i].WallMask = WallMask;
            grids[i].vGridWorldSize = vGridWorldSize;
            grids[i].fNodeRadius = fNodeRadius;
        }
    }


    private void Start()
    {
        // For test purposes
        for (float i = 0; i < 20; i++)
        {
            GameObject obj = (GameObject) Instantiate(Resources.Load("Agent"), new Vector3(i * 1.5f, 0.5f, -65), Quaternion.identity);
            obj.GetComponent<AgentNavigation>().gameManager = this;
        }
    }


    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
            Debug.Log("Free grids : " + NbCurrentlyFree());

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            string s = "[";
            for (int i = 0; i < inUse.Length; i++)
                s += inUse[i] + ", ";
            s += "]";
            Debug.Log(s);
        }
    }


    public void MoveCommand(Dictionary<int, AgentNavigation> agents, Vector3 target)
    {
        for (int i = 0; i < arraysSize; i++)
        {
            if (inUse[i] == 0)
            {
                inUse[i] = agents.Count;
                grids[i].ChangeTarget(target);

                foreach (KeyValuePair<int, AgentNavigation> ag in agents)
                {
                    if (ag.Value.hasDestination)
                    {
                        foreach (int indexes in ag.Value.gridIndexes)
                            inUse[indexes]--;
                        ag.Value.UnsetDestination();
                    }

                    ag.Value.gridIndexes.Clear();
                    ag.Value.worldGrid.Clear();
                    ag.Value.worldGrid.Add(grids[i]);
                    ag.Value.gridIndexes.Add(i);
                    ag.Value.SetDestination();
                }

                return;
            }
        }

        Debug.Log("All grids already in use");
    }


    private int NbCurrentlyFree()
    {
        int total = 0;
        foreach (int i in inUse)
        {
            if (i == 0)
                total++;
        }
        return total;
    }
}
