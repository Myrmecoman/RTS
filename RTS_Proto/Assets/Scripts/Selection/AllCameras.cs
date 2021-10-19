using UnityEngine;


public class AllCameras : MonoBehaviour
{
    private bool[] isSet = new bool[10];
    private Vector3[] camPositions = new Vector3[10];


    public void SetCameraPosition(int camNb, Vector3 pos)
    {
        isSet[camNb] = true;
        camPositions[camNb] = pos;
    }


    public void GoToCameraPosition(int camNb)
    {
        if (isSet[camNb])
            CamController.instance.transform.localPosition = camPositions[camNb];
    }
}
