using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LayerMask WallMask; // This is the mask that the program will look for when trying to find obstructions to the path
    public Vector2 vGridWorldSize; // A vector2 to store the width and height of the graph in world units
    public float fNodeRadius; // This stores how big each square on the graph will be
    public WorldGrid singleGrid;

    private WorldGrid[] grids = new WorldGrid[50];
    private List<AgentNavigation>[] groups = new List<AgentNavigation>[50];


    private void Awake()
    {
        for (int i = 0; i < 50; i++)
        {
            grids[i] = Instantiate(singleGrid, transform);
            grids[i].WallMask = WallMask;
            grids[i].vGridWorldSize = vGridWorldSize;
            grids[i].fNodeRadius = fNodeRadius;
            groups[i] = new List<AgentNavigation>();
        }
    }


    private void Start()
    {
        // For test purposes
        for (float i = 0; i < 10; i++)
        {
            GameObject obj = (GameObject) Instantiate(Resources.Load("Agent"), new Vector3(i * 3, 0.5f, -65), Quaternion.identity);
            obj.GetComponent<AgentNavigation>().worldGrid = grids[0];
            groups[0].Add(obj.GetComponent<AgentNavigation>());
        }
    }


    public void MoveCommand(List<AgentNavigation> agents, Vector3 target)
    {
        for (int i = 0; i < 50; i++)
        {
            if (groups[i].Count == 0)
            {
                groups[i] = agents;
                foreach (var ag in agents)
                {
                    ag.worldGrid = grids[i];
                    ag.SetDestination();
                }

                grids[i].ChangeTarget(target);
                break;
            }
        }
    }
}
