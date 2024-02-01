using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject BossObject;
    public bool inMotion;
    public float intervalTime;
    public Animator animator;

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
    bossState state;
    public bool spawn;
    public bool wait;
    public bool phaseOne;
    public bool phaseTwo;
    public bool phaseThree;
    public bool phaseFour;
    public bool death;

    [Header("Phase 1")]
    public Transform playerTransform;


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
        //Phase 4 - Heat Seeking Missile
        PhaseFour,
        Shinda
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        stateHandler();
        if (!isChanged)
        {
            Invoke("colorChange", colourTimer);
            isChanged = true;
        }
        if (!inMotion)
        {
           randomPhase();
        }
    }

    public void stateHandler()
    {
        if (spawn)
        {
            state = bossState.Spawn;
            Spawn();
        }else if (wait)
        {
            state = bossState.Wait;
            Invoke("PhaseInterval", intervalTime);
        }
        else if (phaseOne)
        {
            state = bossState.PhaseOne;
        }
        else if (phaseTwo)
        {
            state = bossState.PhaseTwo;
        }
        else if (phaseThree)
        {
            state = bossState.PhaseThree;
        }
        else if (phaseFour)
        {
            state = bossState.PhaseFour;
        }
        else if (death)
        {
            state = bossState.Shinda;
            Death();
        }
    }

    public bool resetBool(bool trigger)
    {
        return trigger = false;
    }

    public void PhaseInterval()
    {
        inMotion = false;
        resetBool(wait);
    }

    public void randomPhase()
    {
        int randNum = Random.Range(1, 5);
        switch (randNum)
        {
            case 1:
                phaseOne = true;
                break;
            case 2:
                phaseTwo = true;
                break;
            case 3:
                phaseThree = true;
                break;
            case 4:
                phaseFour = true;
                break;
        }
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            death = true;
        }
    }
    
    public void colorChange()
    {
        print("change!");
        int randomNumber = Random.Range(1, 4);
        switch (randomNumber)
        {
            //Red
            case 1:
                BossObject.tag = "Red";
                BossObject.GetComponent<Renderer>().material = redMat;
                print(BossObject.gameObject.tag);
                isChanged = false;
                break;
            //Green   
            case 2:
                BossObject.tag = "Green";
                BossObject.GetComponent<Renderer>().material = greenMat;
                print(BossObject.gameObject.tag);
                isChanged = false;
                break;
            //Blue
            case 3:

                BossObject.tag = "Blue";
                BossObject.GetComponent<Renderer>().material = blueMat;
                print(BossObject.gameObject.tag);
                isChanged = false;
                break;
        }
        
    }

    public void Spawn()
    {
        animator.SetTrigger("Spawn");
        wait = true;
        resetBool(spawn);
    }

    public void Death()
    {
        animator.SetTrigger("Death");
        Destroy(BossObject);
    }

    public void moveToPlayer()
    {
        BossObject.transform.LookAt(playerTransform);

    }

    public void laser()
    {

    }

}

