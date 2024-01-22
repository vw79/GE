using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedUnlock : MonoBehaviour
{
    public StateManager stateManager;

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player
        if (other.CompareTag("Player"))
        {
            // Call UnlockState2 on the StateManager script
            if (stateManager != null)
            {
                stateManager.UnlockState2();
            }

            // Optionally, destroy the orb after unlocking the state
            Destroy(gameObject);
        }
    }
}
