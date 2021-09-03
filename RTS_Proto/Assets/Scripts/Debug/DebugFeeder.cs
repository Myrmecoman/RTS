using UnityEngine;
using UnityEngine.UI;

public class DebugFeeder : MonoBehaviour
{
    public Text fps;
    public Text grids;
    public Text mineral;
    public Text gas;

    private void Update()
    {
        fps.text = ((int) (1.0f / Time.smoothDeltaTime)) + " fps";
        grids.text = "grids available : " + GameManager.instance.NbCurrentlyFree();
        mineral.text = "mineral : " + GameManager.instance.minerals;
        gas.text = "gas : " + GameManager.instance.gas;
    }
}
