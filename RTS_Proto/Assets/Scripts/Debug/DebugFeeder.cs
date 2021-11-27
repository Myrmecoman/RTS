using UnityEngine;
using UnityEngine.UI;

public class DebugFeeder : MonoBehaviour
{
    public static DebugFeeder instance;

    public Text fps;
    public Text grids;
    public Text mineral;
    public Text gas;
    public Text pathingtime;

    [HideInInspector] public double lastPathingTime = 0;


    private void Start()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    private void Update()
    {
        fps.text = ((int) (1.0f / Time.smoothDeltaTime)) + " fps";
        grids.text = "grids available : " + PathRegister.instance.NbCurrentlyFree();
        mineral.text = "mineral : " + GameManager.instance.minerals;
        gas.text = "gas : " + GameManager.instance.gas;
        pathingtime.text = "pathing time : " + ((float)lastPathingTime * 1000) + " ms";
    }
}
