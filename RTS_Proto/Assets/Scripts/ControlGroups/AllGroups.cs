using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AllGroups : MonoBehaviour
{
    public static AllGroups instance;

    public Dictionary<int, Selectable>[] controlGroups = new Dictionary<int, Selectable>[10];


    private void Start()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < controlGroups.Length; i++)
            controlGroups[i] = new Dictionary<int, Selectable>();
    }


    public void AddToGroup(Dictionary<int, Selectable> selected, int groupNb)
    {
        foreach (var i in selected)
        {
            if (!controlGroups[groupNb].ContainsKey(i.Key))
                controlGroups[groupNb].Add(i.Key, i.Value);
        }
    }


    public void RemoveFromGroup(Dictionary<int, Selectable> selected, int groupNb)
    {
        foreach (var i in selected)
        {
            if (controlGroups[groupNb].ContainsKey(i.Key))
                controlGroups[groupNb].Remove(i.Key);
        }
    }


    public void AddAndRemoveFromOtherGroups(Dictionary<int, Selectable> selected, int groupNb)
    {
        for (int i = 0; i < controlGroups.Length; i++)
            RemoveFromGroup(selected, i);

        AddToGroup(selected, groupNb);
    }


    public void SelectGroup(int groupNb)
    {
        SelectedDico.instance.DeselectAll();
        SelectedDico.instance.selectedTable = controlGroups[groupNb].ToDictionary(entry => entry.Key, entry => entry.Value); // Deep copy
        SelectedDico.instance.UpdateUI();
    }
}
