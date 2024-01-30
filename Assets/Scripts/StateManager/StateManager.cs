using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    [Header("Sword Material")]
    public Material[] guardMaterials; 
    public Material[] cylinderMaterials;

    [Header("Sword Renderer")]
    public Renderer guardRenderer;
    public Renderer cylinderRenderer;

    [Header("Sword Trail")]
    public ParticleSystem trailParticleSystem;
    public Color[] trailColors;

    private SkinnedMeshRenderer playerRenderer;
    private Material[] playerMaterialDefault;
    public Material[] playerNewMaterials;

    public Image stateImage; 
    public Sprite blueImage; 
    public Sprite redImage;
    public Sprite greenImage;

    public Image stateSkillImage;
    public Sprite blueSkillImage;
    public Sprite redSkillImage;
    public Sprite greenSkillImage;

    public Cooldown redCDScript;
    public Cooldown blueCDScript;
    public Cooldown greenCDScript;

    public GameObject redCD;
    public GameObject blueCD;
    public GameObject greenCD;

    public Canvas redCanvas;
    public Canvas blueCanvas;   
    public Canvas greenCanvas;

    public enum State
    {
        State1,
        State2,
        State3
    }

    private State currentState = State.State1;
    private bool state2Unlocked = false;
    private bool state3Unlocked = false;
    private bool isCooldown = false;
    private float cooldown = 0.5f;

    private float[] actionCooldowns;
    private bool[] isActionCooldowns;


    private bool isActionCooldown = false;

    private Warp warp;
    private PlayerCombat playerCombat;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        warp = player.GetComponent<Warp>();
        playerCombat = player.GetComponent<PlayerCombat>();
        playerRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
        playerMaterialDefault = playerRenderer.materials;

        actionCooldowns = new float[3] { 2f, 5f, 10f }; 
        isActionCooldowns = new bool[3] { false, false, false };
       
        redCDScript = redCD.GetComponentInChildren<Cooldown>();
        blueCDScript = blueCD.GetComponentInChildren<Cooldown>();
        greenCDScript = greenCD.GetComponentInChildren<Cooldown>();

        redCanvas = redCD.GetComponentInChildren<Canvas>();
        blueCanvas = blueCD.GetComponentInChildren<Canvas>();
        greenCanvas = greenCD.GetComponentInChildren<Canvas>();

        redCanvas.enabled = false;
        greenCanvas.enabled = false;
    }
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
            if (playerCombat.isUltimateActive) return;
            StartCoroutine(PerformActionWithCooldown());
        }
    }

    private IEnumerator PerformActionWithCooldown()
    {
        int stateIndex = (int)currentState;

        // Perform the action if not in cooldown
        if (!isActionCooldowns[stateIndex])
        {
            PerformStateSpecificAction();

            // Set the cooldown flag for the specific state
            isActionCooldowns[stateIndex] = true;

            // Wait for the cooldown period
            yield return new WaitForSeconds(actionCooldowns[stateIndex]);

            // Reset the cooldown flag for the specific state
            isActionCooldowns[stateIndex] = false;
        }
    }

    private void PerformStateSpecificAction()
    {
        if (currentState == State.State1)
        {
            blueCDScript.UseSpell();
            Debug.Log("Ice");
        }
        else if (currentState == State.State2)
        {
            redCDScript.UseSpell();
            Debug.Log("Fire");
        }
        else if (currentState == State.State3)
        {
            greenCDScript.UseSpell();
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
        StartCoroutine(FlashPlayerMaterial(newState));
        UpdateStateInfo();
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

    private IEnumerator FlashPlayerMaterial(State newState)
    {
        Material[] currentMaterials = playerRenderer.materials;

        switch (newState)
        {
            case State.State1:
                playerRenderer.materials = new Material[] { playerNewMaterials[0], playerNewMaterials[0] };
                break;
            case State.State2:
                playerRenderer.materials = new Material[] { playerNewMaterials[1], playerNewMaterials[1] };
                break;
            case State.State3:
                playerRenderer.materials = new Material[] { playerNewMaterials[2], playerNewMaterials[2] };
                break;
        }

        yield return new WaitForSeconds(0.5f); 

        playerRenderer.materials = playerMaterialDefault;
    }

    public float GetCurrentStateCooldown()
    {
        int stateIndex = (int)currentState; // Assuming currentState is a variable representing current state
        if (stateIndex >= 0 && stateIndex < actionCooldowns.Length)
        {
            return actionCooldowns[stateIndex];
        }
        else
        {
            Debug.LogWarning("State index out of range. Returning default cooldown.");
            return 0f; // Default or error value
        }
    }

    private void UpdateStateInfo()
    {
        switch (currentState)
        {
            case State.State1:
                stateImage.sprite = blueImage;
                stateSkillImage.sprite = blueSkillImage;
                blueCanvas.enabled = true;
                redCanvas.enabled = false;
                greenCanvas.enabled = false;
                break;

            case State.State2:
                stateImage.sprite = redImage;
                stateSkillImage.sprite = redSkillImage;
                blueCanvas.enabled = false;
                redCanvas.enabled = true;
                greenCanvas.enabled = false;
                break;

            case State.State3:
                stateImage.sprite = greenImage;
                stateSkillImage.sprite = greenSkillImage;
                blueCanvas.enabled = false;
                redCanvas.enabled = false;
                greenCanvas.enabled = true;
                break;
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
