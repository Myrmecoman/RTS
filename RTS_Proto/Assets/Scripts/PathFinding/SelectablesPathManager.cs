using KNN;
using KNN.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


public class SelectablesPathManager : MonoBehaviour
{
    SelectablesPathManager instance;

    // max numbers, could be changed depending on game mode
    private const int agentsListMaxSize = 1000;
    private const int buildingsListMaxSize = 1000;

    // all necessary containers
    private Transform[] allyAgentsTransforms;
    private NativeArray<float3> allyAgents;
    private KnnContainer allyAgentsContainer;

    private Transform[] enemyAgentsTransforms;
    private NativeArray<float3> enemyAgents;
    private KnnContainer enemyAgentsContainer;

    private Transform[] allyBuildingsTransforms;
    private NativeArray<float3> allyBuildings;
    private KnnContainer allyBuildingsContainer;

    private Transform[] enemyBuildingsTransforms;
    private NativeArray<float3> enemyBuildings;
    private KnnContainer enemyBuildingsContainer;

    // Declared variables
    KnnRebuildJob rebuilder;
    QueryKNearestBatchJob batchQuery;


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
        
        // we want to query neighbours for all points, keep an array of neighbour indices of all points
        var enemyAgentFromAllyAgent = new NativeArray<int>(agentsListMaxSize, Allocator.TempJob);
        var enemyBuildingFromAllyAgent = new NativeArray<int>(agentsListMaxSize, Allocator.TempJob);
        var allyAgentFromEnemyAgent = new NativeArray<int>(buildingsListMaxSize, Allocator.TempJob);
        var allyBuildingFromEnemyAgent = new NativeArray<int>(buildingsListMaxSize, Allocator.TempJob);
        
        // get results for all points
        batchQuery = new QueryKNearestBatchJob(enemyAgentsContainer, allyAgents, enemyAgentFromAllyAgent); // get closest enemy agent from ally agent
        batchQuery.ScheduleBatch(allyAgents.Length, allyAgents.Length / 32).Complete();
        batchQuery = new QueryKNearestBatchJob(enemyBuildingsContainer, allyAgents, enemyBuildingFromAllyAgent); // get closest enemy building from ally agent
        batchQuery.ScheduleBatch(allyAgents.Length, allyAgents.Length / 32).Complete();
        batchQuery = new QueryKNearestBatchJob(allyAgentsContainer, enemyAgents, allyAgentFromEnemyAgent); // get closest ally agent from enemy agent
        batchQuery.ScheduleBatch(enemyAgents.Length, enemyAgents.Length / 32).Complete();
        batchQuery = new QueryKNearestBatchJob(allyBuildingsContainer, enemyAgents, allyBuildingFromEnemyAgent); // get closest ally agent from enemy agent
        batchQuery.ScheduleBatch(enemyAgents.Length, enemyAgents.Length / 32).Complete();
        
        enemyAgentFromAllyAgent.Dispose();
        enemyBuildingFromAllyAgent.Dispose();
        allyAgentFromEnemyAgent.Dispose();
        allyBuildingFromEnemyAgent.Dispose();

        DebugFeeder.instance.lastUnitsQueryTime = Time.realtimeSinceStartupAsDouble - t;
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
    }
}
