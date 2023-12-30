using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //State Machine
    public EnemyAIStateMachine stateMachine;
    public EnemyStateId initialState;

    //Miscellaneous
    public NavMeshAgent navMeshAgent;
    public Transform playerTransform;
    public LayerMask GroundLayer, PlayerLayer;

    [Header("Patrolling")]
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    [Header("States")]
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Attacking")]
    public bool isMelee;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public Rigidbody body;


    public void Awake()
    {
        //to randomly generate melee or range
        isMelee = Random.value > 0.5f;
        if (!isMelee) attackRange = 5;
        else attackRange = 2;
        playerTransform = GameObject.Find("Ch44_nonPBR").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        
    }
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new EnemyAIStateMachine(this);
        //to register a new state
        stateMachine.RegisterState(new EnemyChaseState());
        stateMachine.RegisterState(new EnemyPatrolState());
        stateMachine.RegisterState(new EnemyAttackState());
        stateMachine.RegisterState(new EnemyDeathState());
        stateMachine.ChangeState(initialState);
    }
    void Update()
    {
      
        stateMachine.Update();
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerLayer);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerLayer);

        if (!playerInAttackRange && !playerInSightRange &&!walkPointSet) stateMachine.ChangeState(EnemyStateId.Patrolling);
        if (!playerInAttackRange && playerInSightRange) stateMachine.ChangeState(EnemyStateId.Chasing);
        if (playerInAttackRange && playerInSightRange) stateMachine.ChangeState(EnemyStateId.Attacking);
    }
}


public class EnemyPatrolState : EnemyState
{

    public EnemyStateId getID()
    {
        return EnemyStateId.Patrolling;
    }

    public void Enter(EnemyAI agent)
    {

    }


    public void Update(EnemyAI agent)
    {
        if (!agent.walkPointSet) {
            float RandomZ = Random.Range(-agent.walkPointRange, agent.walkPointRange);
            float RandomX = Random.Range(-agent.walkPointRange, agent.walkPointRange);

            agent.walkPoint = new Vector3(agent.transform.position.x + RandomX, agent.transform.position.y, agent.transform.position.z + RandomZ);
            agent.walkPointSet = true;

            if (Physics.Raycast(agent.walkPoint, -agent.transform.up, agent.GroundLayer))
            {
                agent.navMeshAgent.SetDestination(agent.walkPoint);
            }
        }

        Vector3 DistanceToWalkPoint =  agent.transform.position - agent.walkPoint;

        if(DistanceToWalkPoint.magnitude < 1f)
            agent.walkPointSet = false;
    }

    public void Exit(EnemyAI agent)
    {
   
    }
}

public class EnemyChaseState : EnemyState
{
 
    public EnemyStateId getID()
    {
        return EnemyStateId.Chasing;
    }

    public void Enter(EnemyAI agent)
    {
    }

    public void Update(EnemyAI agent)
    {
        agent.navMeshAgent.SetDestination(agent.playerTransform.position);
    }

    public void Exit(EnemyAI agent)
    {
    }
}

public class EnemyAttackState : EnemyState
{

    public EnemyStateId getID()
    {
        return EnemyStateId.Attacking;
    }
    public void Enter(EnemyAI agent)
    {
    }

    public void Exit(EnemyAI agent)
    {
    }


    public void Update(EnemyAI agent)
    {
        agent.navMeshAgent.SetDestination(agent.transform.position);
        agent.transform.LookAt(agent.playerTransform);

        if(!agent.isMelee)
        {
            
        }
    }
}

public class EnemyDeathState : EnemyState
{
 
    public EnemyStateId getID()
    {
        return EnemyStateId.Death;
    }

    public void Enter(EnemyAI agent)
    {
    }

    public void Update(EnemyAI agent)
    {
    }

    public void Exit(EnemyAI agent)
    {
    }
}

