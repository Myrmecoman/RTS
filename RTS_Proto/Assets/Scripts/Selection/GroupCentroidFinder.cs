using System;
using System.Collections.Generic;
using UnityEngine;


public static class GroupCentroidFinder
{
    public static Vector2 GetPlanarCentroid(Dictionary<int, Selectable> group)
    {
        if (group.Count == 0)
            return new Vector2(CamController.instance.transform.position.x, CamController.instance.transform.position.z);

        List <Tuple<int, float>> distances = new List<Tuple<int, float>>();
        foreach (KeyValuePair<int, Selectable> pair in group)
            distances.Add(new Tuple<int, float>(pair.Key, pair.Value.transform.position.sqrMagnitude)); // squared magnitude is more optimized

        distances.Sort((a, b) => a.Item2.CompareTo(b.Item2));

        // we don't average since we could end up in empty space with no unit
        Vector3 theChosenOne = group[distances[distances.Count / 2].Item1].transform.position;
        return new Vector2(theChosenOne.x, theChosenOne.z);
    }
}
