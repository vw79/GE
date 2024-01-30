using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
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


}

