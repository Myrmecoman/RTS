using System.Collections.Generic;
using UnityEngine;

public class SelectedDico : MonoBehaviour
{
    public static SelectedDico instance;

    [HideInInspector] public Dictionary<int, AgentManager> selectedTable = new Dictionary<int, AgentManager>();


    private void Awake()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    public void AddSelected(AgentManager go)
    {
        int id = go.GetInstanceID();

        if (!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, go);
            go.Select();
        }
    }


    public void Deselect(int id)
    {
        selectedTable[id].UnSelect();
        selectedTable.Remove(id);
    }


    public void DeselectAll()
    {
        foreach(KeyValuePair<int, AgentManager> pair in selectedTable)
        {
            if(pair.Value != null)
            {
                selectedTable[pair.Key].UnSelect();
            }
        }
        selectedTable.Clear();
    }
}
