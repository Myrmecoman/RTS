using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int minerals = 0;
    public int gas = 0;

    [HideInInspector] public KdTree<Transform> allyUnits;
    [HideInInspector] public KdTree<Transform> allyBuildings;
    [HideInInspector] public KdTree<Transform> enemyUnits;
    [HideInInspector] public KdTree<Transform> enemyBuildings;


    private void Awake()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        allyUnits = new KdTree<Transform>();
        allyBuildings = new KdTree<Transform>();
        enemyUnits = new KdTree<Transform>();
        enemyBuildings = new KdTree<Transform>();
    }


    private void Start()
    {
        for (int x = 0; x < 30; x++)
        {
            for (int y = 0; y < 30; y++)
            {
                GameObject obj = (GameObject)Instantiate(Resources.Load("Worker"), new Vector3(x, 0, y), Quaternion.identity);
                allyUnits.Add(obj.transform);
            }
        }
    }


    public void AttackCommand(Dictionary<int, Selectable> agents, Transform target, bool focus = false)
    {
        int gridId;
        int calculatorId;
        PathRegister.instance.ProvidePath(out gridId, out calculatorId, target, agents.Count);
        foreach (KeyValuePair<int, Selectable> ag in agents)
        {
            if (ag.Value.GetComponent<AgentManager>())
            {
                if (focus)
                    ag.Value.AddDestination(gridId, calculatorId, target.position, target, Actions.ATTACK);
                else
                    ag.Value.AddDestination(gridId, calculatorId, target.position, null, Actions.ATTACK);
            }
        }
    }


    public void MoveCommand(Dictionary<int, Selectable> agents, Transform target, bool follow = false, bool resource = false)
    {
        int gridId;
        int calculatorId;
        PathRegister.instance.ProvidePath(out gridId, out calculatorId, target, agents.Count);
        foreach (KeyValuePair<int, Selectable> ag in agents)
        {
            if (follow)
                ag.Value.AddDestination(gridId, calculatorId, target.position, target, Actions.FOLLOW);
            else if (resource && ag.Value.isWorker)
            {
                ag.Value.AddDestination(gridId, calculatorId, target.position, null, Actions.HARVEST, target.GetComponent<ResourceManager>());
                target.GetComponent<ResourceManager>().MoveTowardsSprite();
            }
            else
                ag.Value.AddDestination(gridId, calculatorId, target.position);
        }
    }
}
