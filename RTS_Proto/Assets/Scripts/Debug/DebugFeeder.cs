using UnityEngine;
using UnityEngine.UI;

public class DebugFeeder : MonoBehaviour
{
    public Text grids;
    public Text mineral;
    public Text gas;

    private void Update()
    {
        grids.text = "grids available : " + GameManager.instance.NbCurrentlyFree();
        mineral.text = "mineral : " + GameManager.instance.minerals;
        gas.text = "gas : " + GameManager.instance.gas;
    }
}
