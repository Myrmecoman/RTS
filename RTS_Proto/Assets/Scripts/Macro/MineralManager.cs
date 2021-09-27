using UnityEngine;


public class MineralManager : MonoBehaviour
{
    public int mineralValue = 3;
    public Transform[] slots;

    private int[] isOccupied;


    private void Awake()
    {
        isOccupied = new int[slots.Length];
        for (int i = 0; i < isOccupied.Length; i++)
            isOccupied[i] = -1;
    }


    public Transform GetFreeSlot(int objId)
    {
        for(int i = 0; i < isOccupied.Length; i++)
        {
            if (isOccupied[i] == -1)
            {
                isOccupied[i] = objId;
                return slots[i];
            }
        }

        return null;
    }


    public void FreeSlot(int objId)
    {
        for (int i = 0; i < isOccupied.Length; i++)
        {
            if (isOccupied[i] == objId)
            {
                isOccupied[i] = -1;
                return;
            }
        }

        Debug.Log("Not belonging to this mineral bg");
    }
}
