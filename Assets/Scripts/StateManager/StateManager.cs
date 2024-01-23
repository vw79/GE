using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum State
    {
        State1,
        State2,
        State3
    }

    // Materials for the sword components
    public Material[] guardMaterials; // Assign materials for each state in the inspector
    public Material[] cylinderMaterials; // Assign materials for each state in the inspector

    // Renderer references for the sword's components
    public Renderer guardRenderer;
    public Renderer cylinderRenderer;

    // Trail particle system and its colors for each state
    public ParticleSystem trailParticleSystem;
    public Color[] trailColors; // Assign colors for each state in the inspector

    private State currentState = State.State1;
    private bool state2Unlocked = false;
    private bool state3Unlocked = false;
    private bool isCooldown = false;
    public float cooldown = 5f; // Cooldown time in seconds

    private bool isActionCooldown = false;
    public float actionCooldown = 10f;

    public Warp warp;
    public Animator animator;

    void Start()
    {
        UpdateMaterialsAndTrail();
    }

    void Update()
    {
        // Check for state change inputs
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentState != State.State1 && !isCooldown)
        {
            StartCoroutine(ChangeStateWithCooldown(State.State1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentState != State.State2 && !isCooldown && state2Unlocked)
        {
            StartCoroutine(ChangeStateWithCooldown(State.State2));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && currentState != State.State3 && !isCooldown && state3Unlocked)
        {
            StartCoroutine(ChangeStateWithCooldown(State.State3));
        }

        if (Input.GetKeyUp(KeyCode.E) && !isActionCooldown)
        {
            StartCoroutine(PerformActionWithCooldown());
        }
    }

    private IEnumerator PerformActionWithCooldown()
    {
        // Perform the action
        PerformStateSpecificAction();

        // Set the cooldown flag
        isActionCooldown = true;

        // Wait for the cooldown period
        yield return new WaitForSeconds(actionCooldown);

        // Reset the cooldown flag
        isActionCooldown = false;
    }

    private void PerformStateSpecificAction()
    {
        if (currentState == State.State1)
        {
            Debug.Log("Ice");
        }
        else if (currentState == State.State2)
        {
            Debug.Log("Fire");
        }
        else if (currentState == State.State3)
        {
            warp.StartWarp();
        }
    }

    public void UnlockState2()
    {
        state2Unlocked = true;
    }

    public void UnlockState3()
    {
        state3Unlocked = true;
    }

    IEnumerator ChangeStateWithCooldown(State newState)
    {
        ChangeState(newState);
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
        UpdateMaterialsAndTrail();
    }

    private void UpdateMaterialsAndTrail()
    {
        int stateIndex = (int)currentState;

        // Update materials
        if (stateIndex < guardMaterials.Length && guardRenderer)
        {
            guardRenderer.material = guardMaterials[stateIndex];
        }
        if (stateIndex < cylinderMaterials.Length && cylinderRenderer)
        {
            cylinderRenderer.material = cylinderMaterials[stateIndex];
        }

        // Update trail particle system color
        if (stateIndex < trailColors.Length && trailParticleSystem)
        {
            var colorModule = trailParticleSystem.colorOverLifetime;
            colorModule.color = new ParticleSystem.MinMaxGradient(trailColors[stateIndex]);
        }
    }

    public bool CanAttack(GameObject enemy)
    {
        switch (currentState)
        {
            case State.State1:
                return enemy.CompareTag("Red");
            case State.State2:
                return enemy.CompareTag("Green");
            case State.State3:
                return enemy.CompareTag("Blue");
            default:
                return false;
        }
    }
}
