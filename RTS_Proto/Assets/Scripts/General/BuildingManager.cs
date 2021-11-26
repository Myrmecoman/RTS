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
        {
            GameObject newhandler = (GameObject)Instantiate(Resources.Load("CoroutineHandler"), null);
            newhandler.GetComponent<CoroutineHandler>().CallCoroutine(PathRegister.instance.AddGridColliders(transform.position));
        }
        //Debug.Log("update grids colliders time : " + (Time.realtimeSinceStartupAsDouble - delay));
    }


    private void OnDestroy()
    {
        //double delay = Time.realtimeSinceStartupAsDouble;
        GetComponent<MeshCollider>().enabled = false;
        for (int i = 0; i < 100; i++)
        {
            GameObject newhandler = (GameObject) Instantiate(Resources.Load("CoroutineHandler"), null);
            newhandler.GetComponent<CoroutineHandler>().CallCoroutine(PathRegister.instance.RemoveGridColliders(transform.position));
        }
        //Debug.Log("update grids colliders time : " + (Time.realtimeSinceStartupAsDouble - delay));
    }


    public override void AddDestination(int gridId, int calculatorId, Vector3 dest, Transform follow = null, Actions action = Actions.MOVE, ResourceManager res = null)
    {
        Debug.LogError("Not implemented yet");
    }


    public override void HoldPosition()
    {
        Debug.LogError("Trying to hold position a building");
    }
}
