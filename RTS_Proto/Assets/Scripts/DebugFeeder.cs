using UnityEngine;
using UnityEngine.UI;

public class DebugFeeder : MonoBehaviour
{
    public GameManager gm;
    public Text grids;
    public Text mineral;
    public Text gas;

    private void Update()
    {
        grids.text = "grids available : " + gm.NbCurrentlyFree();
        mineral.text = "mineral : " + gm.minerals;
        gas.text = "gas : " + gm.gas;
    }
}
