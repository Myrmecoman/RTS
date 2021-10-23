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

        UpdateUI();
    }


    public void AddSelected(Selectable go)
    {
        int id = go.GetInstanceID();

        if (!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, go);
            go.Select();
        }

        UpdateUI();
    }


    public void Deselect(int id)
    {
        selectedTable[id].UnSelect();
        selectedTable.Remove(id);

        UpdateUI();
    }


    public void DeslectDueToDestruction(int id)
    {
        if (selectedTable.ContainsKey(id))
            selectedTable.Remove(id);

        foreach (var g in AllGroups.instance.controlGroups)
            g.Remove(id);

        UpdateUI();
    }


    public void DeselectAll()
    {
        foreach(KeyValuePair<int, Selectable> pair in selectedTable)
            selectedTable[pair.Key].UnSelect();

        selectedTable.Clear();

        UpdateUI();
    }


    public void SelectGroup(int groupId)
    {
        DeselectAll();
        foreach(var s in AllGroups.instance.controlGroups[groupId])
            AddSelected(s.Value);

        UpdateUI();
    }


    // Called everywhere to update to any change
    private void UpdateUI()
    {
        // Check if we have anything
        if (selectedTable.Count == 0)
        {
            // disable UI and return
            return;
        }

        // Check if we have a unit, it has priority over buildings
        bool haveUnit = false;

        foreach (var s in selectedTable)
        {
            if (s.Value.GetComponent<AgentManager>())
            {
                haveUnit = true;
                break;
            }
        }

        if (haveUnit) // have a unit
        {

        }
        else // have a building
        {

        }
    }
}
