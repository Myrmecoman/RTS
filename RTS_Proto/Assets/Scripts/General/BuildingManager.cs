using UnityEngine;

public class BuildingManager : Selectable
{
    public string buildingName;
    public float buildTime = 1f;
    public string[] unitsNames;
    public int[] unitsPrices;
    public float[] unitsBuildTime;
    public bool isBuilding = false;


    private void Start()
    {
        //double delay = Time.realtimeSinceStartupAsDouble;
        for (int i = 0; i < 100; i++)
            GameManager.instance.grids[i].AddGridColliders(transform.position);
        //Debug.Log("update grids colliders time : " + (Time.realtimeSinceStartupAsDouble - delay));
    }


    private void Update()
    {
        
    }


    private void OnDestroy()
    {
        //double delay = Time.realtimeSinceStartupAsDouble;
        GetComponent<MeshCollider>().enabled = false;
        for (int i = 0; i < 100; i++)
            GameManager.instance.grids[i].RemoveGridColliders(transform.position);
        //Debug.Log("update grids colliders time : " + (Time.realtimeSinceStartupAsDouble - delay));
    }


    public override void AddDestination(WorldGrid grid, int index, Transform follow = null, int action = 0 /* 1 = attack, 2 = patrol, 3 = collect-resource */, ResourceManager res = null)
    {
        Debug.LogError("not implemented yet");
    }

    public override void HoldPosition()
    {
        Debug.LogError("Trying to hold position a building");
    }
}
