using System.Collections.Generic;
using UnityEngine;

public class SelectedDico : MonoBehaviour
{
    [HideInInspector] public Dictionary<int, AgentNavigation> selectedTable = new Dictionary<int, AgentNavigation>();

    public void AddSelected(AgentNavigation go)
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
        foreach(KeyValuePair<int, AgentNavigation> pair in selectedTable)
        {
            if(pair.Value != null)
            {
                selectedTable[pair.Key].UnSelect();
            }
        }
        selectedTable.Clear();
    }
}
