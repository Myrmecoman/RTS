using System.Collections.Generic;
using UnityEngine;

public class SelectedDico : MonoBehaviour
{
    public static SelectedDico instance;

    [HideInInspector] public Dictionary<int, Selectable> selectedTable = new Dictionary<int, Selectable>();


    private void Awake()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    public void AddSelected(Selectable go)
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


    public void DeslectDueToDestruction(int id)
    {
        if (selectedTable.ContainsKey(id))
            selectedTable.Remove(id);
    }


    public void DeselectAll()
    {
        foreach(KeyValuePair<int, Selectable> pair in selectedTable)
        {
            if(pair.Value != null)
            {
                selectedTable[pair.Key].UnSelect();
            }
        }
        selectedTable.Clear();
    }
}
