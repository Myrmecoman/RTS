using UnityEngine;
using UnityEngine.UI;

public class DebugFeeder : MonoBehaviour
{
    public static DebugFeeder instance;

    public Text fps;
    public Text grids;
    public Text calculators;
    public Text mineral;
    public Text gas;
    public Text totaltime;
    public Text arrayClearTime;
    public Text pathingTime;
    public Text flowFieldTime;
    public Text unitsQueryTime;

    [HideInInspector] public double lastTotalTime = 0;
    [HideInInspector] public double lastClearTime = 0;
    [HideInInspector] public double lastPathingTime = 0;
    [HideInInspector] public double lastFlowFIeldTime = 0;
    [HideInInspector] public double lastUnitsQueryTime = 0;


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
        grids.text = "grids available : " + PathRegister.instance.NbFreeGrids();
        calculators.text = "calculators available : " + PathRegister.instance.NbFreeCalculators();
        mineral.text = "mineral : " + GameManager.instance.minerals;
        gas.text = "gas : " + GameManager.instance.gas;
        totaltime.text = "total time : " + ((float)lastTotalTime * 1000) + " ms";
        arrayClearTime.text = "clear time : " + ((float)lastClearTime * 1000) + " ms";
        pathingTime.text = "pathing time : " + ((float)lastPathingTime * 1000) + " ms";
        flowFieldTime.text = "flowfield time : " + ((float)lastFlowFIeldTime * 1000) + " ms";
        unitsQueryTime.text = "units query time : " + ((float)lastUnitsQueryTime * 1000) + " ms";
    }
}
