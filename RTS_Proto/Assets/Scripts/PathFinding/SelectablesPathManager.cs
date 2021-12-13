using KNN;
using KNN.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


// Credits for the lighting fast KNN : https://github.com/ArthurBrussee/KNN
public class SelectablesPathManager : MonoBehaviour
{
    public static SelectablesPathManager instance;

    // max numbers, could be changed depending on game mode
    private const int agentsListMaxSize = 2000;
    private const int buildingsListMaxSize = 2000;

    // all necessary containers
    public Transform[] allyAgentsTransforms;
    public NativeArray<float3> allyAgents;
    private KnnContainer allyAgentsContainer;

    public Transform[] enemyAgentsTransforms;
    public NativeArray<float3> enemyAgents;
    private KnnContainer enemyAgentsContainer;

    public Transform[] allyBuildingsTransforms;
    public NativeArray<float3> allyBuildings;
    private KnnContainer allyBuildingsContainer;

    public Transform[] enemyBuildingsTransforms;
    public NativeArray<float3> enemyBuildings;
    private KnnContainer enemyBuildingsContainer;

    // Result arrays
    public NativeArray<int> enemyAgentFromAllyAgent;
    public NativeArray<int> enemyBuildingFromAllyAgent;
    public NativeArray<int> allyAgentFromEnemyAgent;
    public NativeArray<int> allyBuildingFromEnemyAgent;

    // Predeclared variables
    private KnnRebuildJob rebuilder;
    private QueryKNearestBatchJob batchQuery;
    private int allyAgentIndexProvider = 0;
    private int enemyAgentIndexProvider = 0;
    private int allyBuildingIndexProvider = 0;
    private int enemyBuildingIndexProvider = 0;


    void Awake()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // allocate transform arrays
        allyAgentsTransforms = new Transform[agentsListMaxSize];
        enemyAgentsTransforms = new Transform[agentsListMaxSize];
        allyBuildingsTransforms = new Transform[buildingsListMaxSize];
        enemyBuildingsTransforms = new Transform[buildingsListMaxSize];

        // create points arrays
        allyAgents = new NativeArray<float3>(agentsListMaxSize, Allocator.Persistent);
        enemyAgents = new NativeArray<float3>(agentsListMaxSize, Allocator.Persistent);
        allyBuildings = new NativeArray<float3>(buildingsListMaxSize, Allocator.Persistent);
        enemyBuildings = new NativeArray<float3>(buildingsListMaxSize, Allocator.Persistent);

        // Create a container that accelerates querying for neighbours (false is to not build yet, we do it every frame in Update)
        allyAgentsContainer = new KnnContainer(allyAgents, false, Allocator.Persistent);
        enemyAgentsContainer = new KnnContainer(enemyAgents, false, Allocator.Persistent);
        allyBuildingsContainer = new KnnContainer(allyBuildings, false, Allocator.Persistent);
        enemyBuildingsContainer = new KnnContainer(enemyBuildings, false, Allocator.Persistent);

        // result arrays initialization
        enemyAgentFromAllyAgent = new NativeArray<int>(agentsListMaxSize, Allocator.Persistent);
        enemyBuildingFromAllyAgent = new NativeArray<int>(agentsListMaxSize, Allocator.Persistent);
        allyAgentFromEnemyAgent = new NativeArray<int>(buildingsListMaxSize, Allocator.Persistent);
        allyBuildingFromEnemyAgent = new NativeArray<int>(buildingsListMaxSize, Allocator.Persistent);

        // updating points positions
        for (int i = 0; i < agentsListMaxSize; ++i)
        {
            allyAgents[i] = new float3(-10000, -10000 - i, -10000); // the "- i" is to prevent same positions, that would kill the algorithm optimization
            enemyAgents[i] = new float3(-10000, -10000 - i, -10000);

            if (i < buildingsListMaxSize)
            {
                allyBuildings[i] = new float3(-10000, -10000 - i, -10000);
                enemyBuildings[i] = new float3(-10000, -10000 - i, -10000);
            }
        }
    }


    void Update()
    {
        double t = Time.realtimeSinceStartupAsDouble;

        // updating points positions
        for (int i = 0; i < agentsListMaxSize; ++i)
        {
            if (allyAgentsTransforms[i] != null)
                allyAgents[i] = new float3(allyAgentsTransforms[i].position.x, allyAgentsTransforms[i].position.y, allyAgentsTransforms[i].position.z);
            else
                allyAgents[i] = new float3(-10000, -10000 - i, -10000); // the "- i" is to prevent same positions, that would kill the algorithm optimization

            if (enemyAgentsTransforms[i] != null)
                enemyAgents[i] = new float3(enemyAgentsTransforms[i].position.x, enemyAgentsTransforms[i].position.y, enemyAgentsTransforms[i].position.z);
            else
                enemyAgents[i] = new float3(-10000, -10000 - i, -10000);

            if (i < buildingsListMaxSize)
            {
                if (allyBuildingsTransforms[i] != null)
                    allyBuildings[i] = new float3(allyBuildingsTransforms[i].position.x, allyBuildingsTransforms[i].position.y, allyBuildingsTransforms[i].position.z);
                else
                    allyBuildings[i] = new float3(-10000, -10000 - i, -10000);

                if (enemyBuildingsTransforms[i] != null)
                    enemyBuildings[i] = new float3(enemyBuildingsTransforms[i].position.x, enemyBuildingsTransforms[i].position.y, enemyBuildingsTransforms[i].position.z);
                else
                    enemyBuildings[i] = new float3(-10000, -10000 - i, -10000);
            }
        }

        // the points changed, rebuild the container:
        rebuilder = new KnnRebuildJob(allyAgentsContainer);
        rebuilder.Schedule().Complete();
        rebuilder = new KnnRebuildJob(enemyAgentsContainer);
        rebuilder.Schedule().Complete();
        rebuilder = new KnnRebuildJob(allyBuildingsContainer);
        rebuilder.Schedule().Complete();
        rebuilder = new KnnRebuildJob(enemyBuildingsContainer);
        rebuilder.Schedule().Complete();
        
        // get results for all points
        batchQuery = new QueryKNearestBatchJob(enemyAgentsContainer, allyAgents, enemyAgentFromAllyAgent); // get closest enemy agent from ally agent
        batchQuery.ScheduleBatch(allyAgents.Length, allyAgents.Length / 32).Complete();
        batchQuery = new QueryKNearestBatchJob(enemyBuildingsContainer, allyAgents, enemyBuildingFromAllyAgent); // get closest enemy building from ally agent
        batchQuery.ScheduleBatch(allyAgents.Length, allyAgents.Length / 32).Complete();
        batchQuery = new QueryKNearestBatchJob(allyAgentsContainer, enemyAgents, allyAgentFromEnemyAgent); // get closest ally agent from enemy agent
        batchQuery.ScheduleBatch(enemyAgents.Length, enemyAgents.Length / 32).Complete();
        batchQuery = new QueryKNearestBatchJob(allyBuildingsContainer, enemyAgents, allyBuildingFromEnemyAgent); // get closest ally agent from enemy agent
        batchQuery.ScheduleBatch(enemyAgents.Length, enemyAgents.Length / 32).Complete();
        
        DebugFeeder.instance.lastUnitsQueryTime = Time.realtimeSinceStartupAsDouble - t;


        //Instantiate(Resources.Load("debugSphere"), new Vector3(allyAgents[0].x, allyAgents[0].y, allyAgents[0].z) + Vector3.up * 2, Quaternion.identity);
        //Instantiate(Resources.Load("debugSphere"), new Vector3(enemyAgents[enemyAgentFromAllyAgent[0]].x, enemyAgents[enemyAgentFromAllyAgent[0]].y, enemyAgents[enemyAgentFromAllyAgent[0]].z) + Vector3.up * 2, Quaternion.identity);
    }


    public int ProvideSlot(Transform selectable)
    {
        Selectable selecComponent = selectable.GetComponent<Selectable>();

        if (selecComponent.isAlly)
        {
            if (selectable.GetComponent<AgentManager>())
            {
                while (allyAgentsTransforms[allyAgentIndexProvider] != null)
                    allyAgentIndexProvider = (allyAgentIndexProvider + 1) % agentsListMaxSize;

                allyAgentsTransforms[allyAgentIndexProvider] = selectable;
                return allyAgentIndexProvider;
            }
            else if (selectable.GetComponent<BuildingManager>())
            {
                while (allyBuildingsTransforms[allyBuildingIndexProvider] != null)
                    allyBuildingIndexProvider = (allyBuildingIndexProvider + 1) % buildingsListMaxSize;

                allyBuildingsTransforms[allyBuildingIndexProvider] = selectable;
                return allyBuildingIndexProvider;
            }

            Debug.LogError("Neither building nor agent");
            return -1;
        }
        else
        {
            if (selectable.GetComponent<AgentManager>())
            {
                while (enemyAgentsTransforms[enemyAgentIndexProvider] != null)
                    enemyAgentIndexProvider = (enemyAgentIndexProvider + 1) % agentsListMaxSize;

                enemyAgentsTransforms[enemyAgentIndexProvider] = selectable;
                return enemyAgentIndexProvider;
            }
            else if (selectable.GetComponent<BuildingManager>())
            {
                while (enemyBuildingsTransforms[enemyBuildingIndexProvider] != null)
                    enemyBuildingIndexProvider = (enemyBuildingIndexProvider + 1) % buildingsListMaxSize;

                enemyBuildingsTransforms[enemyBuildingIndexProvider] = selectable;
                return enemyBuildingIndexProvider;
            }

            Debug.LogError("Neither building nor agent");
            return -1;
        }

        Debug.LogError("Neither ally nor enemy");
        return -1;
    }


    private void OnDestroy()
    {
        allyAgentsContainer.Dispose();
        allyAgents.Dispose();

        enemyAgentsContainer.Dispose();
        enemyAgents.Dispose();

        allyBuildingsContainer.Dispose();
        allyBuildings.Dispose();

        enemyBuildingsContainer.Dispose();
        enemyBuildings.Dispose();

        enemyAgentFromAllyAgent.Dispose();
        enemyBuildingFromAllyAgent.Dispose();
        allyAgentFromEnemyAgent.Dispose();
        allyBuildingFromEnemyAgent.Dispose();
    }
}
