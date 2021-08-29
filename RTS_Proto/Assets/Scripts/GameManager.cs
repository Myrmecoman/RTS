using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LayerMask WallMask; // This is the mask that the program will look for when trying to find obstructions to the path
    public Vector2 vGridWorldSize; // A vector2 to store the width and height of the graph in world units
    public float fNodeRadius; // This stores how big each square on the graph will be
    public WorldGrid singleGrid;

    private static int arraysSize = 100;
    private WorldGrid[] grids = new WorldGrid[arraysSize];
    private bool[] inUse = new bool[arraysSize];


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
        for (float i = 0; i < 10; i++)
            Instantiate(Resources.Load("Agent"), new Vector3(i * 3, 0.5f, -65), Quaternion.identity);
    }


    public void MoveCommand(Dictionary<int, AgentNavigation> agents, Vector3 target)
    {
        for (int i = 0; i < arraysSize; i++)
        {
            if (inUse[i] == false)
            {
                inUse[i] = true;
                grids[i].ChangeTarget(target);

                foreach (KeyValuePair<int, AgentNavigation> ag in agents)
                {
                    ag.Value.UnsetDestination();
                    ag.Value.worldGrid.Clear();
                    ag.Value.worldGrid.Add(grids[i]);
                    ag.Value.SetDestination();
                }

                return;
            }
        }

        Debug.Log("All grids already in use");
    }
}
