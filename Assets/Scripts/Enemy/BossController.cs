using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject Boss;
    public bool inMotion;

    [Header("Colour")]
    public Material redMat;
    public Material blueMat;
    public Material greenMat;
    public bool isChanged;
    public bool isRed;
    public bool isBlue;
    public bool isGreen;
    public float colourTimer;

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
        PhaseFour

    }
    private void Awake()
    {  

    }

    private void Update()
    {
        if (!isChanged && !inMotion)
        {
            Invoke("colorChange", colourTimer);
            isChanged = true;
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
                Boss.tag = "Red";
                Boss.GetComponent<Renderer>().material = redMat;
                print(Boss.gameObject.tag);
                isChanged = false;
                break;
            //Green   
            case 2:
                Boss.tag = "Green";
                Boss.GetComponent<Renderer>().material = greenMat;
                print(Boss.gameObject.tag);
                isChanged = false;
                break;
            //Blue
            case 3:

                Boss.tag = "Blue";
                Boss.GetComponent<Renderer>().material = blueMat;
                print(Boss.gameObject.tag);
                isChanged = false;
                break;
        }
        
    }
}

