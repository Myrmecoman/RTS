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
        if (isOccupied != -1 && workerTransform != null &&
            Vector3.Distance(new Vector3(workerTransform.position.x, transform.position.y, workerTransform.position.z), transform.position) < 0.1f)
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


    // 0 = already mining, 1 = accepted to mine, 2 = refused since occupied
    public int IsFreeSlot(int objId)
    {
        if (objId == isOccupied)
            return 0;

        if (isOccupied == -1)
            return 1;

        return 2;
    }


    public void GetFreeSlot(int objId, Transform trans)
    {
        isOccupied = objId;
        cooldown = harvestCooldown;
        workerTransform = trans;
    }


    public void FreeSlot()
    {
        Debug.Log("free");
        isOccupied = -1;
        cooldown = harvestCooldown;
        workerTransform = null;
    }


    public void MoveTowardsSprite()
    {
        moveTowardsSprite.SetActive(true);
    }
}
