using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //State Machine
    [HideInInspector] public EnemyAIStateMachine stateMachine;
    public EnemyStateId initialState;

    //Miscellaneous
    [HideInInspector] public NavMeshAgent navMeshAgent;
    //public NavMeshSurface navMeshSurface;
    [HideInInspector] public Transform playerTransform;
    public LayerMask GroundLayer, PlayerLayer;
    [SerializeField] public PlayerHealthSystem pHealth;

    [Header("Health")]
    public float maxEnemyHealth;
    private float currentHealth;

    [Header("Animation")]
    public Animation[] meleeAnimation;
    public Animation[] rangedAnimation;

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
    public float attackDamage;
    public Animator animator;
    public bool isDead;

    [Header("Shooting")]
    public Transform gunTip;
    public GameObject bullet;
    public float shootForce;
    public float bulletDamage;
    [HideInInspector] public bullet Bullet;

    public void Awake()
    {
        //to randomly generate melee or range
        if (!isMelee) attackRange = 5;
        else attackRange = 1;
        nextAttack = true;
        isDead = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (!isMelee)
        {
            Bullet.bulletDamage = bulletDamage;
        }

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
        currentHealth = maxEnemyHealth;
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
                dead();
            }
        }

    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            dead();
        }
    }

    public void dead()
    {
        stateMachine.ChangeState(EnemyStateId.Death);
    }

    //animation event functions
    public void Shoot()
    {
        //instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, gunTip.position, Quaternion.identity);

        //add forces to bullets
        currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce, ForceMode.Impulse);

        Destroy(currentBullet,2);
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

