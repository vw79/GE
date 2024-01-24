using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //State Machine
    public EnemyAIStateMachine stateMachine;
    public EnemyStateId initialState;

    //Miscellaneous
    public NavMeshAgent navMeshAgent;
    //public NavMeshSurface navMeshSurface;
    public Transform playerTransform;
    public LayerMask GroundLayer, PlayerLayer;
    [SerializeField] public HealthSystem pHealth;

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
    public bool nextAttack;
    public float AttackCD;
    public float attackDamage;
    public GameObject projectile;
    public Rigidbody body;
    public Animator animator;


    public void Awake()
    {
        //to randomly generate melee or range
        isMelee = Random.value > 0.5f;
        if (!isMelee) attackRange = 5;
        else attackRange = 2;
        nextAttack = true;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new EnemyAIStateMachine(this);
        //to register a new state 
        stateMachine.RegisterState(new EnemyChaseState());
        stateMachine.RegisterState(new EnemyAttackState());
        stateMachine.RegisterState(new EnemyDeathState());
        stateMachine.ChangeState(initialState);
    }
    void Update()
    {
      
        stateMachine.Update();
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerLayer);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerLayer);

        if (!playerInAttackRange && playerInSightRange)
        {   
            stateMachine.ChangeState(EnemyStateId.Chasing);
            animator.Play("Run");       
        }

        if (playerInAttackRange && playerInSightRange) 
        { 
            stateMachine.ChangeState(EnemyStateId.Attacking); 
        }
    }


  
}


public class EnemyChaseState : EnemyState
{
 
    public EnemyStateId getID()
    {
        return EnemyStateId.Chasing;
    }

    public void Update(EnemyAI agent)
    {
        agent.navMeshAgent.SetDestination(agent.playerTransform.position );
    }


    public void Enter(EnemyAI agent)
    {
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

    public void Update(EnemyAI agent)
    {
        agent.animator.SetTrigger("Attack");
        agent.pHealth.TakeDamage(agent.attackDamage);
        agent.StartCoroutine(AttackDelay(agent));

    }

    public void Enter(EnemyAI agent)
    {
        agent.navMeshAgent.SetDestination(agent.transform.position);
        agent.transform.LookAt(agent.playerTransform);
    }

    public void Exit(EnemyAI agent)
    {


    }

    private IEnumerator AttackDelay(EnemyAI agent)
    {
        yield return new WaitForSeconds(agent.AttackCD);
        Debug.Log("Attack Delay!!");
       
    }

}

public class EnemyDeathState : EnemyState
{
 
    public EnemyStateId getID()
    {
        return EnemyStateId.Death;
    }

    public void Update(EnemyAI agent)
    {
    }

    public void Enter(EnemyAI agent)
    {
    }

    public void Exit(EnemyAI agent)
    {
    }
}

