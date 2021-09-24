using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public string buildingName;
    public float buildTime = 1f;
    public GameObject moveTowardsSprite;
    public string[] unitsNames;
    public int[] unitsPrices;
    public float[] unitsBuildTime;
    public bool isBuilding = false;

    [HideInInspector] public bool isAlly = true;


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
        moveTowardsSprite.SetActive(true);
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
