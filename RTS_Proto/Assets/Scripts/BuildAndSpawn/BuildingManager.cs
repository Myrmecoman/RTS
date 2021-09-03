using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public string buildingName;
    public float diameter;
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


    public void MoveTowardsSprite()
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load("moveTowards"), transform);
        obj.transform.localScale = new Vector3(diameter / 4, diameter / 4, diameter / 4);
    }


    private void OnDestroy()
    {
        //double delay = Time.realtimeSinceStartupAsDouble;
        GetComponent<MeshCollider>().enabled = false;
        for (int i = 0; i < 100; i++)
            GameManager.instance.grids[i].RemoveGridColliders(transform.position);
        //Debug.Log("update grids colliders time : " + (Time.realtimeSinceStartupAsDouble - delay));
    }
}
