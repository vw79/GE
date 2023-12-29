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
    public LayerMask Ground, Player;

    [Header("Patrolling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    
    

    public void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        
    }
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new EnemyAIStateMachine(this);
        //to register a new state
        stateMachine.RegisterState(new EnemyChaseState());
        stateMachine.RegisterState(new EnemyPatrolState());
        stateMachine.RegisterState(new EnemyDeathState());
        stateMachine.ChangeState(initialState);
    }
    void Update()
    {
        stateMachine.Update();
    }
}


public class EnemyPatrolState : EnemyState
{

    public EnemyStateId getID()
    {
        return EnemyStateId.patrolling;
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
        agent.stateMachine.ChangeState(EnemyStateId.attacking);
    }

    public void Exit(EnemyAI agent)
    {
    }
}

public class EnemyDeathState : EnemyState
{
 
    public EnemyStateId getID()
    {
        return EnemyStateId.death;
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

