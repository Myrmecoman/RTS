﻿using Unity.Mathematics;
using UnityEngine;


public class AgentManager : Selectable
{
    public UnitName unitName;
    public int sightRange = 10;
    public int supply = 1;
    public int attackDamage = 5;
    public int attackRange = 5;
    public float attackSpeed = 1;
    public int health = 50;
    public int armor = 0;
    public float speed = 1.0f;
    public bool canAttackGround = true;
    public bool canAttackAir = true;

    public Transform leftPart;
    public Transform rightPart;

    [HideInInspector] public WorldGrid worldGrid;
    [HideInInspector] public int gridIndexe;
    [HideInInspector] public bool hasDestination = false;

    private bool holdPosition = false;
    private bool attackCommand = false;
    private double attackCooldown;
    private Rigidbody rb;
    private Transform follow;
    private ResourceManager ressource = null;
     

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        // NEED TO FIND OUT WHEN DESTINATION IS REACHED

        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        AgentManager foundTarget = CheckEnnemy();

        if (attackCooldown <= 0 && (!hasDestination || attackCommand || holdPosition) && foundTarget != null && ressource == null)
        {
            // attack target
            Attack(foundTarget);
            return;
        }

        if ((attackCommand || holdPosition) && foundTarget != null && ressource == null)
        {
            // attacking, need to hold position
            rb.isKinematic = true;
            return;
        }
        else if (ressource == null)
            rb.isKinematic = false;

        if (!hasDestination)
            return;

        // Detecting if we reached our target position
        float horizontalDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(worldGrid.StartPosition.x, 0, worldGrid.StartPosition.z));
        if (horizontalDist <= 0.05f && follow == null)
        {
            UnsetDestinationExceptResource();
            if (ressource != null)
                HoldPosition();
            return;
        }
        
        MoveAndRotate(horizontalDist);
    }


    private void MoveAndRotate(float horizontalDist)
    {
        if (horizontalDist < 0.05f || worldGrid.StartPosition == null)
            return;

        Vector3 leftMostPart = new Vector3(leftPart.position.x, -50, leftPart.position.z);
        Vector3 rightMostPart = new Vector3(rightPart.position.x, -50, rightPart.position.z);
        Vector3 targetPosition = new Vector3(worldGrid.StartPosition.x, -50, worldGrid.StartPosition.z);

        // control height
        RaycastHit verifyHeight;
        Physics.Raycast(transform.position, Vector3.down, out verifyHeight, 1000f, LayerMask.GetMask("ground"));
        float heightDist = verifyHeight.distance * 10;

        RaycastHit hitLeft;
        RaycastHit hitRight;
        int layerMask = LayerMask.GetMask("wall");

        if (!Physics.Raycast(leftMostPart, targetPosition - leftMostPart, out hitLeft, horizontalDist, layerMask) &&
            !Physics.Raycast(rightMostPart, targetPosition - rightMostPart, out hitRight, horizontalDist, layerMask))
        {
            // Clear line of sight to target position
            if (follow == null)
            {
                Vector3 Ynull = new Vector3(worldGrid.StartPosition.x, transform.position.y, worldGrid.StartPosition.z);
                Vector3 moveDir = (Ynull - transform.position).normalized;

                rb.MovePosition(transform.position + new Vector3(moveDir.x, -heightDist, moveDir.z) * Time.fixedDeltaTime * speed);
                if (moveDir != Vector3.zero)
                    transform.LookAt(new Vector3(worldGrid.StartPosition.x, transform.position.y, worldGrid.StartPosition.z), Vector3.up);
            }
            else
            {
                Vector3 Ynull = new Vector3(follow.position.x, transform.position.y, follow.position.z);
                Vector3 moveDir = (Ynull - transform.position).normalized;

                rb.MovePosition(transform.position + new Vector3(moveDir.x, -heightDist, moveDir.z) * Time.fixedDeltaTime * speed);
                if (moveDir != Vector3.zero)
                    transform.LookAt(new Vector3(follow.position.x, transform.position.y, follow.position.z), Vector3.up);
            }
        }
        else
        {
            int2 flowVector = worldGrid.NodeFromWorldPoint(transform.position).FlowFieldVector;
            Vector3 moveDir = new Vector3(flowVector.x, 0, flowVector.y).normalized;

            rb.MovePosition(transform.position + (moveDir + Vector3.up * -heightDist) * Time.fixedDeltaTime * speed);
            if (moveDir != Vector3.zero)
                transform.forward = moveDir;
        }
    }


    // very intensive function (could be run every x seconds)
    private AgentManager CheckEnnemy()
    {
        if (GameManager.instance.enemyUnits.Count == 0 || GameManager.instance.allyUnits.Count == 0)
            return null;

        Transform nearestEnemy;

        if (isAlly)
            nearestEnemy = GameManager.instance.enemyUnits.FindClosest(transform.position);
        else
            nearestEnemy = GameManager.instance.allyUnits.FindClosest(transform.position);

        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(nearestEnemy.position.x, 0, nearestEnemy.position.z)) <= attackRange)
            return nearestEnemy.GetComponent<AgentManager>();

        return null;
    }


    private void Attack(AgentManager ag)
    {
        transform.LookAt(new Vector3(ag.transform.position.x, transform.position.y, ag.transform.position.z));
        ag.GetAttacked(attackDamage);
        attackCooldown = attackSpeed;
    }


    public override void HoldPosition()
    {
        UnsetDestinationExceptResource();
        holdPosition = true;
        rb.isKinematic = true;
    }


    private void GetAttacked(int dmg)
    {
        int diff = dmg - armor;

        if (diff <= 0)
            return;

        health -= diff;

        if (health <= 0)
        {
            if (isAlly)
                GameManager.instance.allyUnits.RemoveAll(new System.Predicate<int>(IsSameObj));
            else
                GameManager.instance.enemyUnits.RemoveAll(new System.Predicate<int>(IsSameObj));

            SelectedDico.instance.DeslectDueToDestruction(GetInstanceID());
            UnsetDestination();
            Destroy(gameObject);
        }
    }


    public override void AddDestination(WorldGrid grid, int index, Transform follow = null, int action = 0 /* 1 = attack, 2 = patrol, 3 = collect-resource */, ResourceManager res = null)
    {
        if (action == 3 && isWorker)
        {
            int result = res.IsFreeSlot(gameObject.GetInstanceID());
            if (result == 0)
            {
                UnsetDestinationExceptResource();
            }
            else if (result == 1)
            {
                UnsetDestination();
                res.GetFreeSlot(gameObject.GetInstanceID(), transform);
                this.ressource = res;
            }
            else
                UnsetDestination();
        }
        else
        {
            UnsetDestination();
        }

        worldGrid = grid;
        gridIndexe = index;
        hasDestination = true;
        this.follow = follow;

        if (action == 1 || action == 2)
            attackCommand = true;
        else
            attackCommand = false;
    }


    public void UnsetDestination()
    {
        follow = null;
        hasDestination = false;
        attackCommand = false;
        holdPosition = false;
        rb.isKinematic = false;
        if (ressource != null)
        {
            ressource.FreeSlot();
            ressource = null;
        }

        if (hasDestination)
            GameManager.instance.inUse[gridIndexe]--;
    }

    
    public void UnsetDestinationExceptResource()
    {
        follow = null;
        hasDestination = false;
        attackCommand = false;
        holdPosition = false;
        rb.isKinematic = false;

        if (hasDestination)
            GameManager.instance.inUse[gridIndexe]--;
    }
}
