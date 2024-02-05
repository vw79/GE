using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public GameObject BossObject;
    public bool inMotion;
    public Animator animator;
    public NavMeshAgent agent;
    PlayerHealthSystem playerHealthSystem;

    [Header("Health")]
    public float maxHealth;
    float currentHealth;

    [Header("Colour")]
    public Material redMat;
    public Material blueMat;
    public Material greenMat;
    public bool isChanged;
    public bool isRed;
    public bool isBlue;
    public bool isGreen;
    public float colourTimer;


    [Header("State")]
    public float phaseDuration; //duration for each attack
    public float waitDuration; //duration for each attack
    public float phaseTimer;
    public float waitTimer;
    bossState CurrentState;

    [Header("Phase 1")]
    [HideInInspector]public Transform playerTransform;


    [Header("Phase 2")]
    public float PullForce;


    [Header("Phase 3")]
    public int bulletsShot;
    public int maxBulletsPerWave;
    public float shootForce;
    public GameObject Sphere;
    public Transform[] spawnPoint;


    [Header("Phase 4")]
    public float tenseiRadius;
    public GameObject OrbVFX;

    [Header("Phase 5")]
    public float slamRadius;
    public float slamDamage;
    public GameObject SlamVFX;
    

    private enum bossState
    {
        Spawn,
        Wait,
        Shinda,
        //Phase 1 - moving to player
        PhaseOne,
        //Phase 2 - Laser
        PhaseTwo,
        //Phase 3 - Sphere Spawn
        PhaseThree,
        //Phase 4 - shinra tensei
        PhaseFour,
        PhaseFive
        
    }

    private void Awake()
    {
        CurrentState = bossState.Spawn;
        transform.rotation = Quaternion.identity;
        currentHealth = maxHealth;
        inMotion = false;
    }
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        phaseTimer = 0f;
        waitTimer = 2f;
        agent = GetComponent<NavMeshAgent>();
       
    }

    private void Update()
    {
        stateHandler();
        if (!isChanged)
        {
            Invoke("colorChange", colourTimer);
            isChanged = true;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.cyan;
        Gizmos.DrawWireSphere(transform.position, slamRadius);
    }

    public void stateHandler()
    {
            switch (CurrentState)
            {
                case bossState.Spawn:
                    animator.SetTrigger("Spawn");
                    break;
                case bossState.Wait:
                    print("WAIT PHASE");
                    animator.Play("Idle");
                    PhaseInterval();
                    break;
                case bossState.PhaseOne:
                    print("PHASE ONE");
                    animator.Play("Walking");
                    moveToPlayer();
                    break;
                case bossState.PhaseTwo:
                    print("PHASE TWO");
                    animator.Play("Pull");
                    Invoke("BanshoTenin", 1f);
                    break;
                case bossState.PhaseThree:
                    print("PHASE THREE");
                    animator.Play("Missile");
                    
                    break;
                case bossState.PhaseFour:
                    print("PHASE FOUR");
                    animator.Play("Push");
                    shinraTensei();
                    break;
            case bossState.PhaseFive:
                    print("PHASE FIVE");
                    animator.Play("Slam");
                    break;
                case bossState.Shinda:
                    Death();
                    break;
            }
    }
    public void PhaseInterval()
    {
        transform.LookAt(playerTransform.position);
        
        // Transition to a random phase after the wait duration
        if (waitTimer <= 0f)
        {
            // Choose a random phase
            CurrentState = (bossState)Random.Range((int)bossState.PhaseOne, (int)bossState.PhaseFive + 1);
            //CurrentState = bossState.PhaseTwo;
            

            // Reset the phase timer
            waitTimer = waitDuration;
            phaseTimer = phaseDuration;
        }
        else
        {
            // Decrement the phase timer
            waitTimer -= Time.deltaTime;
        }
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            CurrentState = bossState.Shinda;
        }
    }
    
    public void colorChange()
    {
        SkinnedMeshRenderer renderer = BossObject.GetComponent<SkinnedMeshRenderer>();
        Material[] emiMat = DetectMaterials();
        int randomNumber = Random.Range(1, 4);
        switch (randomNumber)
        {
            //Red
            case 1:
                BossObject.tag = "Red";
                foreach (Material mat in emiMat) {
                    mat.SetColor("_EmissionColor", UnityEngine.Color.red);
                }
                print(BossObject.gameObject.tag);
                isChanged = false;
                break;
            //Green   
            case 2:
                BossObject.tag = "Green";
                foreach (Material mat in emiMat)
                {
                    mat.SetColor("_EmissionColor", UnityEngine.Color.green);
                }
                print(BossObject.gameObject.tag);
                isChanged = false;
                break;
            //Blue
            case 3:

                BossObject.tag = "Blue";
                foreach (Material mat in emiMat)
                {
                    mat.SetColor("_EmissionColor", UnityEngine.Color.blue);
                }
                print(BossObject.gameObject.tag);
                isChanged = false;
                break;
        }
        
    }

    //to detect emission materials
    Material[] DetectMaterials()
    {
        SkinnedMeshRenderer renderer = BossObject.GetComponent<SkinnedMeshRenderer>();
        Material[] materialsWithEmission = new Material[0];

        if (renderer != null)
        {
            Material[] materials = renderer.materials;

            foreach (Material material in materials)
            {
                // Check if the material has an emission color
                if (material.HasProperty("_EmissionColor"))
                {
                    UnityEngine.Color emissionColor = material.GetColor("_EmissionColor");

                    // Check if the emission color is not black (non-zero)
                    if (emissionColor.r > 0f || emissionColor.g > 0f || emissionColor.b > 0f || emissionColor.a > 0f)
                    {
                        System.Array.Resize(ref materialsWithEmission, materialsWithEmission.Length + 1);
                        materialsWithEmission[materialsWithEmission.Length - 1] = material;
                    }

                }


            }
        }
         return materialsWithEmission;
 
    }

    public void Spawn()
    {
        CurrentState = bossState.Wait; 
    }

    public void Death()
    {
        //animator.SetTrigger("Death");
        Destroy(BossObject);
    }

    //Phase 1
    public void moveToPlayer()
    {
        print("move");
        agent.SetDestination(playerTransform.position);

        if (Vector3.Distance(transform.position, playerTransform.position) < 1.5f)
        {
            CurrentState = bossState.PhaseFour;
            phaseTimer = phaseDuration; // Reset the phase timer for the next phase
        }
    }

    //Phase 2
    public void BanshoTenin()
    {
        print("BANSHO");
        Vector3 directionToPlayer =  (playerTransform.position - transform.position).normalized;
        playerTransform.GetComponent<Rigidbody>().AddForce(-directionToPlayer * PullForce, ForceMode.VelocityChange);
        if (Vector3.Distance(transform.position, playerTransform.position) < 2.0f)
        {
            animator.ResetTrigger("Pull");
            phaseTimer = phaseDuration;
            CurrentState = bossState.PhaseFive;
        }
        

    }

    //Phase 3
    public void rasengan()
    {
        print("RASENENGAN");
        foreach (Transform shootingPoint in spawnPoint)
        {
            GameObject clone = Instantiate(Sphere, shootingPoint.position, Quaternion.identity);
            Vector3 playerOffset = new Vector3(0, 1, 0);
            Vector3 directionToShoot = playerTransform.position + playerOffset - shootingPoint.position;
            clone.GetComponent<Rigidbody>().AddForce(directionToShoot.normalized * shootForce, ForceMode.Impulse);
        }
         CurrentState = bossState.Wait;

    }
    //Phase 4
    public void shinraTensei()
    {
        print("shinra");
        if (!inMotion) 
        {
            inMotion = true;
            OrbVFX.SetActive(true);
        }

        if (phaseTimer <= 0f)
        {
            OrbVFX.SetActive(false);
            CurrentState = bossState.Wait;
            phaseTimer = phaseDuration;
            inMotion = false;

        }
        else
        {
            phaseTimer -= Time.deltaTime;
        }
    }

    //Phase 5
    public void aoeBlast()
    {
        print("Blast");
        GameObject clone = Instantiate(SlamVFX, transform.position, transform.rotation);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, slamRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerHealthSystem = collider.GetComponent<PlayerHealthSystem>();
                playerHealthSystem.TakeDamage(slamDamage);
            }
        }
        CurrentState = bossState.Wait;

    }


}

