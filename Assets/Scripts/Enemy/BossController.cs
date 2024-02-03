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


    private enum bossState
    {
        Spawn,
        Wait,
        //Phase 1 - moving to player
        PhaseOne,
        //Phase 2 - Laser
        PhaseTwo,
        //Phase 3 - Sphere Spawn
        PhaseThree,
        //Phase 4 - shinra tensei
        PhaseFour,
        PhaseFive,
        Shinda
    }

    private void Awake()
    {
        transform.rotation = Quaternion.identity;
        currentHealth = maxHealth;
        inMotion = false;
    }
    private void Start()
    {
        CurrentState = bossState.Spawn;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        phaseTimer = 0f;
        waitTimer = 0f;
        agent = GetComponent<NavMeshAgent>();
       
    }

    private void Update()
    {
        if (!inMotion) { stateHandler(); }
        
        if (!isChanged)
        {
            Invoke("colorChange", colourTimer);
            isChanged = true;
        }

    }

  
    public void stateHandler()
    {
            switch (CurrentState)
            {
                case bossState.Spawn:
                    print("SPAWN");
                    Spawn();
                    break;
                case bossState.Wait:
                    print("WAIT PHASE");
                    PhaseInterval();
                    break;
                case bossState.PhaseOne:
                    print("PHASE ONE");
                    moveToPlayer();
                    break;
                case bossState.PhaseTwo:
                    print("PHASE TWO");
                    BanshoTenin();
                    break;
                case bossState.PhaseThree:
                    print("PHASE THREE");
                    rasengan();
                    break;
                case bossState.PhaseFour:
                    print("PHASE FOUR");
                    shinraTensei();
                    break;
            case bossState.PhaseFive:
                    print("PHASE FIVE");
                    aoeBlast();
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
            CurrentState = (bossState)Random.Range((int)bossState.PhaseOne, (int)bossState.PhaseFour + 1);
            

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
        print("change!");
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
                    print("emission colour is "+emissionColor);

                    // Check if the emission color is not black (non-zero)
                    if (emissionColor.r > 0f || emissionColor.g > 0f || emissionColor.b > 0f || emissionColor.a > 0f)
                    {
                        Debug.Log("Material with emission detected: " + material.name);
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
        //animator.SetTrigger("Spawn");
        CurrentState = bossState.Wait; 
    }

    public void Death()
    {
        //animator.SetTrigger("Death");
        Destroy(BossObject);
    }

    public void moveToPlayer()
    {

        agent.SetDestination(playerTransform.position);

        if (Vector3.Distance(transform.position, playerTransform.position) < 4.0f)
        {
            CurrentState = bossState.PhaseFour;
            phaseTimer = phaseDuration; // Reset the phase timer for the next phase
        }
    }

    public void BanshoTenin()
    {
        Vector3 directionToPlayer =  (playerTransform.position - transform.position).normalized;
        playerTransform.GetComponent<Rigidbody>().AddForce(-directionToPlayer * PullForce, ForceMode.Impulse);
        phaseTimer = phaseDuration;
        CurrentState = bossState.PhaseFive;
        

    }

    public void aoeBlast()
    {
        print("Blast");

        if (phaseTimer <= 0f)
        {
            CurrentState = bossState.Wait;
            phaseTimer = phaseDuration;
        }
        else
        {
            phaseTimer -= Time.deltaTime;
        }
    }

    public void rasengan()
    {
        inMotion = true;
        if (bulletsShot < maxBulletsPerWave)
        {
            foreach (Transform shootingPoint in spawnPoint)
            {
                GameObject clone = Instantiate(Sphere, shootingPoint.position, transform.rotation);
                clone.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce, ForceMode.Acceleration);
                // Increment the counter
                bulletsShot++;
            }

            
        }

        // Check if all bullets for this wave have been shot
        if (bulletsShot >= maxBulletsPerWave)
        {
            // Reset the counter for the next wave
            bulletsShot = 0;
            CurrentState = bossState.Wait;
            phaseTimer = phaseDuration;
            inMotion = false;

        }


    }


    public void shinraTensei()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, tenseiRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                Vector3 directionToPlayer = (collider.transform.position - transform.position).normalized;
                collider.GetComponent<Rigidbody>().AddForce(directionToPlayer * 10f, ForceMode.Impulse);
            }
        }
        if (phaseTimer <= 0f)
        {
            CurrentState = bossState.Wait;
            phaseTimer = phaseDuration;
        }
        else
        {
            phaseTimer -= Time.deltaTime;
        }
    }

}

