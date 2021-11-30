using System.Collections;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;


public class PathRegister : MonoBehaviour
{
    public static PathRegister instance;

    public LayerMask WallMask;     // obstacles mask
    public Vector2 vGridWorldSize; // A vector2 to store the width and height of the graph in world units
    public float fNodeRadius;      // This stores how big each square on the graph will be

    [HideInInspector] public NativeArray<DijkstraTile>[] grids;
    [HideInInspector] public NativeArray<DijkstraTile>[] impreciseGrids;
    [HideInInspector] public int[] inUse;
    [HideInInspector] public PathCalculator[] calculators;

    // Variables for fast pathfinding
    [HideInInspector] public int iGridSizeX, iGridSizeY;                   // Size of the Grid in Array units
    [HideInInspector] public int impreciseiGridSizeX, impreciseiGridSizeY; // imprecise version
    [HideInInspector] public float fNodeDiameter;                          // 2 * R
    [HideInInspector] public Vector3 bottomLeft;                           // Get the real world position of the bottom left of the grid
    [HideInInspector] public float imprecisefNodeRadius;                   // imprecise version
    [HideInInspector] public float imprecisefNodeDiameter;                 // imprecise version


    private void Start()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // calculate bottomLeft position
        bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;

        // precise
        fNodeDiameter = fNodeRadius * 2;
        iGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);
        iGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);

        // imprecise
        imprecisefNodeRadius = fNodeDiameter;
        imprecisefNodeDiameter = imprecisefNodeRadius * 2;
        impreciseiGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / imprecisefNodeDiameter);
        impreciseiGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / imprecisefNodeDiameter);

        // create all needed grids
        grids = new NativeArray<DijkstraTile>[100];
        impreciseGrids = new NativeArray<DijkstraTile>[100];
        inUse = new int[100];
        for (int i = 0; i < 100; i++)
        {
            InitGrid(ref grids[i], ref impreciseGrids[i], i == 0);
            inUse[i] = 0;
        }

        calculators = new PathCalculator[10];
        for (int i = 0; i < 10; i++)
            calculators[i] = ((GameObject) Instantiate(Resources.Load("PathCalculator"), transform)).GetComponent<PathCalculator>();
    }


    private void InitGrid(ref NativeArray<DijkstraTile> nodeGrid, ref NativeArray<DijkstraTile> impreciseNodeGrid, bool generateColliders = false)
    {
        nodeGrid = new NativeArray<DijkstraTile>(iGridSizeX * iGridSizeY, Allocator.Persistent);
        impreciseNodeGrid = new NativeArray<DijkstraTile>(impreciseiGridSizeX * impreciseiGridSizeY, Allocator.Persistent);

        for (int x = 0; x < impreciseiGridSizeX; x++)
        {
            for (int y = 0; y < impreciseiGridSizeY; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * imprecisefNodeDiameter + imprecisefNodeRadius) + Vector3.forward * (y * imprecisefNodeDiameter + imprecisefNodeRadius);
                DijkstraTile tile = new DijkstraTile(new int2(x, y));

                if (Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, imprecisefNodeRadius - 0.01f /* in case of single point collision */, WallMask))
                    tile.weight = int.MaxValue;

                impreciseNodeGrid[impreciseiGridSizeY * x + y] = tile; // Create a new node in the array
            }
        }

        for (int x = 0; x < iGridSizeX; x++)
        {
            for (int y = 0; y < iGridSizeY; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);
                DijkstraTile tile = new DijkstraTile(new int2(x, y));

                if (Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, fNodeRadius - 0.01f /* in case of single point collision */, WallMask))
                {
                    tile.weight = int.MaxValue;
                    if (generateColliders)
                        Instantiate(Resources.Load("RuntimeWallCollider"), worldPoint, Quaternion.identity, transform);
                }

                nodeGrid[iGridSizeY * x + y] = tile; // Create a new node in the array
            }
        }
    }


    public IEnumerator AddGridColliders(Vector3 centerPos)
    {
        int largestBuildingRadius = (int)(2 / fNodeRadius) + 1;
        int impreciseLargestBuildingRadius = (int)(2 / imprecisefNodeRadius) + 1;

        float ixPos = (centerPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x;
        float iyPos = (centerPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y;

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        ix = Mathf.RoundToInt((impreciseiGridSizeX - 1) * ixPos);
        iy = Mathf.RoundToInt((impreciseiGridSizeY - 1) * iyPos);

        for (int x = ix - impreciseLargestBuildingRadius; x < ix + impreciseLargestBuildingRadius; x++)
        {
            for (int y = iy - impreciseLargestBuildingRadius; y < iy + impreciseLargestBuildingRadius; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * imprecisefNodeDiameter + imprecisefNodeRadius) + Vector3.forward * (y * imprecisefNodeDiameter + imprecisefNodeRadius);

                //Instantiate(Resources.Load("debugSphere"), worldPoint + Vector3.up * 2, Quaternion.identity);
                if (Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, imprecisefNodeRadius - 0.01f /* in case of single point collision */, WallMask))
                {
                    DijkstraTile tile = new DijkstraTile(new int2(x, y));
                    tile.weight = int.MaxValue;
                    for (int i = 0; i < impreciseGrids.Length; i++)
                        impreciseGrids[i][impreciseiGridSizeY * x + y] = tile;
                }
            }
        }

        yield return new WaitUntil(IsItFinished);

        ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        for (int x = ix - largestBuildingRadius; x < ix + largestBuildingRadius; x++)
        {
            for (int y = iy - largestBuildingRadius; y < iy + largestBuildingRadius; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);

                //Instantiate(Resources.Load("debugSphere"), worldPoint + Vector3.up, Quaternion.identity);
                if (Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, fNodeRadius - 0.01f /* in case of single point collision */, WallMask))
                {
                    DijkstraTile tile = new DijkstraTile(new int2(x, y));
                    tile.weight = int.MaxValue;
                    for (int i = 0; i < impreciseGrids.Length; i++)
                    {
                        grids[i][iGridSizeY * x + y] = tile;
                        if (i == 0 && !Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint - Vector3.up * 80, fNodeRadius - 0.01f /* in case of single point collision */, WallMask))
                            Instantiate(Resources.Load("RuntimeWallCollider"), worldPoint, Quaternion.identity, transform);
                    }
                }
            }
        }
    }


    public IEnumerator RemoveGridColliders(Vector3 centerPos)
    {
        int largestBuildingRadius = (int)(2 / fNodeRadius) + 1;
        int impreciseLargestBuildingRadius = (int)(2 / imprecisefNodeRadius) + 1;

        float ixPos = (centerPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x;
        float iyPos = (centerPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y;

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        ix = Mathf.RoundToInt((impreciseiGridSizeX - 1) * ixPos);
        iy = Mathf.RoundToInt((impreciseiGridSizeY - 1) * iyPos);

        for (int x = ix - impreciseLargestBuildingRadius; x < ix + impreciseLargestBuildingRadius; x++)
        {
            for (int y = iy - impreciseLargestBuildingRadius; y < iy + impreciseLargestBuildingRadius; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * imprecisefNodeDiameter + imprecisefNodeRadius) + Vector3.forward * (y * imprecisefNodeDiameter + imprecisefNodeRadius);

                //Instantiate(Resources.Load("debugSphere"), worldPoint + Vector3.up * 2, Quaternion.identity);
                if (Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, imprecisefNodeRadius - 0.01f /* in case of single point collision */, WallMask))
                {
                    DijkstraTile tile = new DijkstraTile(new int2(x, y));
                    for (int i = 0; i < grids.Length; i++)
                        impreciseGrids[i][impreciseiGridSizeY * x + y] = tile;
                }
            }
        }

        yield return new WaitUntil(IsItFinished);

        ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        for (int x = ix - largestBuildingRadius; x < ix + largestBuildingRadius; x++)
        {
            for (int y = iy - largestBuildingRadius; y < iy + largestBuildingRadius; y++)
            {
                //Get the world coordinates from the bottom left of the graph
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);

                //Instantiate(Resources.Load("debugSphere"), worldPoint + Vector3.up, Quaternion.identity);
                if (Physics.CheckCapsule(worldPoint - Vector3.up * 100, worldPoint + Vector3.up * 100, fNodeRadius - 0.01f /* in case of single point collision */, WallMask))
                {
                    DijkstraTile tile = new DijkstraTile(new int2(x, y));
                    for (int i = 0; i < grids.Length; i++)
                    {
                        grids[i][iGridSizeY * x + y] = tile;
                        RaycastHit hit;
                        if (i == 0 && Physics.Raycast(worldPoint - Vector3.up * 150, Vector3.up, out hit, 100, WallMask))
                            Destroy(hit.transform.gameObject);
                    }
                }
            }
        }
    }


    private bool IsItFinished()
    {
        for (int i = 0; i < calculators.Length; i++)
        {
            if (calculators[i].computingJobs)
                return false;
        }
        return true;
    }


    public void ProvidePath(out int gridId, out int calculatorId, Transform target, int nbAgents, bool follow)
    {
        for (int i = 0; i < 10; i++)
        {
            if (calculators[i].computingJobs)
                continue;

            for (int j = 0; j < 100; j++)
            {
                if (inUse[j] != 0)
                    continue;

                calculators[i].ChangeTarget(target, j, follow);
                inUse[j] = nbAgents;
                calculatorId = i;
                gridId = j;
                return;
            }
        }

        // You broke the fucking game
        Debug.LogError("Calculators all busy or no more arrays available");
        gridId = 0;
        calculatorId = 0;
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


    private void OnApplicationQuit()
    {
        for (int i = 0; i < grids.Length; i++)
        {
            grids[i].Dispose();
            impreciseGrids[i].Dispose();
        }
    }


#if (UNITY_EDITOR)
    // Function that draws the wireframe
    private void OnDrawGizmos()
    {
        // Draw a wire cube with the given dimensions from the Unity inspector
        Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y));

        if (!EditorApplication.isPlaying || calculators[0].computingJobs)
            return;

        if (impreciseGrids[0] != null && impreciseGrids[0].Length != 0) // If the grid is not empty
        {
            foreach (DijkstraTile n in impreciseGrids[0]) // Loop through every node in the grid
            {
                if (n.weight == int.MaxValue) // If the current node is a wall node
                {
                    Gizmos.color = Color.blue;
                    Vector3 worldPoint = bottomLeft + Vector3.right * (n.gridPos.x * imprecisefNodeDiameter + imprecisefNodeRadius) + Vector3.forward * (n.gridPos.y * imprecisefNodeDiameter + imprecisefNodeRadius);
                    Gizmos.DrawCube(worldPoint, new Vector3(1, 0.01f, 1) * imprecisefNodeDiameter);
                }
            }
        }

        if (grids[0] != null && grids[0].Length != 0) // If the grid is not empty
        {
            foreach (DijkstraTile n in grids[0]) // Loop through every node in the grid
            {
                if (n.weight == int.MaxValue) // If the current node is a wall node
                {
                    Gizmos.color = Color.red;
                    Vector3 worldPoint = bottomLeft + Vector3.right * (n.gridPos.x * fNodeDiameter + fNodeRadius) + Vector3.forward * (n.gridPos.y * fNodeDiameter + fNodeRadius);
                    Gizmos.DrawCube(worldPoint, new Vector3(1, 0.01f, 1) * fNodeDiameter);
                }

                Gizmos.color = Color.black;
                Vector3 nodePos = new Vector3(-128 + n.gridPos.x * fNodeDiameter + fNodeRadius, 0.01f, -128 + n.gridPos.y * fNodeDiameter + fNodeRadius);
                float x = (n.FlowFieldVector.x > 0) ? 0.5f : -0.5f;
                x = (n.FlowFieldVector.x == 0) ? 0 : x;
                float y = (n.FlowFieldVector.y > 0) ? 0.5f : -0.5f;
                y = (n.FlowFieldVector.y == 0) ? 0 : y;
                Gizmos.DrawLine(nodePos, new Vector3(nodePos.x + x, 0.01f, nodePos.z + y));
            }
        }
    }
#endif
}
