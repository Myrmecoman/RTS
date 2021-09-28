using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    public int resourceType = 0; // 0 for mineral, 1 for gas
    public GameObject moveTowardsSprite;
    public double harvestCooldown = 3;
    public int resourceValue = 3;

    private int isOccupied;
    private double cooldown;
    private Transform workerTransform;


    private void Awake()
    {
        isOccupied = -1;
        cooldown = harvestCooldown;
    }


    private void Update()
    {
        // distance between same height vectors
        Vector3 workerPos = new Vector3(workerTransform.position.x, transform.position.y, workerTransform.position.z);
        if (isOccupied != -1 && Vector3.Distance(workerPos, transform.position) < 0.1f)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
            {
                if (resourceType == 0)
                    GameManager.instance.minerals += resourceValue;
                if (resourceType == 1)
                    GameManager.instance.gas += resourceValue;

                cooldown = harvestCooldown;
            }
        }
    }


    public bool GetFreeSlot(int objId, Transform trans)
    {
        if (isOccupied == -1)
        {
            isOccupied = objId;
            cooldown = harvestCooldown;
            workerTransform = trans;
            return true;
        }

        Debug.Log("Not free");
        return false;
    }


    public void FreeSlot(int objId)
    {
        if (isOccupied == objId)
        {
            isOccupied = -1;
            cooldown = harvestCooldown;
            workerTransform = null;
            return;
        }

        Debug.Log("Not belonging to this resource");
    }


    public void MoveTowardsSprite()
    {
        moveTowardsSprite.SetActive(true);
    }
}
