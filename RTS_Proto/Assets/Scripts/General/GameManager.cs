using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int minerals = 0;
    public int gas = 0;


    private void Awake()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    private void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
                Instantiate(Resources.Load("Worker"), new Vector3(x - 30, 0, y), Quaternion.identity);
        }

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
                Instantiate(Resources.Load("Agent"), new Vector3(x, 0, y), Quaternion.identity);
        }

        GameObject obj = (GameObject)Instantiate(Resources.Load("buildings/base"), new Vector3(20, 0, -10), Quaternion.identity);
        obj.GetComponent<BuildingManager>().isAlly = false;

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                GameObject obj2 = (GameObject)Instantiate(Resources.Load("Agent"), new Vector3(x + 30, 0, y), Quaternion.identity);
                obj2.GetComponent<AgentManager>().isAlly = false;
            }
        }
    }


    public void AttackCommand(Dictionary<int, Selectable> agents, Transform target, bool focus = false)
    {
        int gridId;
        int calculatorId;

        PathRegister.instance.ProvidePath(out gridId, out calculatorId, target, agents.Count, focus);

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

        PathRegister.instance.ProvidePath(out gridId, out calculatorId, target, agents.Count, follow);

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
