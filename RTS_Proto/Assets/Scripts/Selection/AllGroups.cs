using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

        UpdateGroupIcon(groupNb);
    }


    public void RemoveFromGroup(Dictionary<int, Selectable> selected, int groupNb)
    {
        foreach (var i in selected)
        {
            if (controlGroups[groupNb].ContainsKey(i.Key))
                controlGroups[groupNb].Remove(i.Key);
        }

        UpdateGroupIcon(groupNb);
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
    }


    private void UpdateGroupIcon(int groupNb)
    {
        Image img = InputReceiver.instance.buttonGroups[groupNb].gameObject.GetComponentInChildren<Image>();

        if (controlGroups[groupNb].Count == 0)
        {
            img.sprite = null;
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
            return;
        }

        Names lowestName = (Names)int.MaxValue;
        foreach (var i in controlGroups[groupNb])
        {
            if (i.Value.unitName < lowestName)
                lowestName = i.Value.unitName;
        }

        string path = "icons/";
        if (((int)lowestName) <= 1000)
            path += "unitIcons/";
        else if (((int)lowestName) <= 2000)
            path += "buildingIcons/";
        else
            path += "otherIcons/";

        path += lowestName.ToString();
        img.sprite = Resources.Load<Sprite>(path);
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
    }
}
