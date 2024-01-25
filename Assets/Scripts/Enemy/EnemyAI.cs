using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Transforms;
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
    public float CDTimer;
    public float attackDamage;
    public GameObject projectile;
    public Rigidbody body;
    public Animator animator;
    public bool isDead;

    [Header("Shooting")]
    public Transform gunTip;
    public GameObject bullet;


    public void Awake()
    {
        //to randomly generate melee or range
        isMelee = Random.value > 0.5f;
        if (!isMelee) attackRange = 5;
        else attackRange = 1;
        nextAttack = true;
        isDead = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        //pHealth = GetComponent<HealthSystem>();

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
        if (!isDead)
        {
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

            if (Input.GetKeyDown(KeyCode.J))
            {
                stateMachine.ChangeState(EnemyStateId.Death);
            }
        }

    }

    //animation event functions
    public void Shoot()
    {
        //instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        //rorate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //add forces to bullets
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(Camera.transform.up, ForceMode.Impulse);
    }
    public void DealDamage()
    {
        pHealth.TakeDamage(attackDamage);
        nextAttack = true;
    }
    public void Disintegrate()
    {
        GameObject.Destroy(gameObject);
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
    public void Enter(EnemyAI agent)
    {
        agent.navMeshAgent.SetDestination(agent.transform.position);
        agent.transform.LookAt(agent.playerTransform);
    }

    public void Update(EnemyAI agent)
    {
        
        if (agent.nextAttack)
        {
            agent.nextAttack = false;
            if (agent.isMelee)
            {
                agent.animator.SetTrigger("MeleeAttack");
            }else if(!agent.isMelee)
            {
                agent.animator.SetTrigger("RangedAttack");
            }
        }
    }

    public void Exit(EnemyAI agent)
    {
        agent.nextAttack=true;
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
        agent.animator.SetTrigger("Death");

    }

    public void Enter(EnemyAI agent)
    {
        agent.isDead = true;
    }

    public void Exit(EnemyAI agent)
    {
    }
}

