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
        selectablePathManagerId = SelectablesPathManager.instance.ProvideSlot(transform);

        //double delay = Time.realtimeSinceStartupAsDouble;
        GameObject newhandler = (GameObject)Instantiate(Resources.Load("CoroutineHandler"), null);
        newhandler.GetComponent<CoroutineHandler>().CallCoroutine(PathRegister.instance.AddGridColliders(transform.position));
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


    public override void GetAttacked(int dmg)
    {
        int diff = dmg - armor;

        if (diff <= 0)
            return;

        health -= diff;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        SelectedDico.instance.DeslectDueToDestruction(GetInstanceID());

        //double delay = Time.realtimeSinceStartupAsDouble;
        GetComponent<MeshCollider>().enabled = false;
        GameObject newhandler = (GameObject)Instantiate(Resources.Load("CoroutineHandler"), null);
        newhandler.GetComponent<CoroutineHandler>().CallCoroutine(PathRegister.instance.RemoveGridColliders(transform.position));
        //Debug.Log("update grids colliders time : " + (Time.realtimeSinceStartupAsDouble - delay));

        //ownGrid.Dispose();
        //UnsetDestination();
    }
}
