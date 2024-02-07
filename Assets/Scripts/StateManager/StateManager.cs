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

    public GameObject redCD;
    public GameObject blueCD;
    public GameObject greenCD;

    public AudioSource orbPickupSFX;

    private Canvas redCanvas;
    private Canvas blueCanvas;   
    private Canvas greenCanvas;

    public enum State
    {
        State1,
        State2,
        State3
    }

    private State currentState = State.State1;
    private bool state2Unlocked = false;
    private bool state3Unlocked = false;

    private float[] actionCooldowns;
    private bool[] isActionCooldowns;

    private bool isActionCooldown = false;

    private Warp warp;
    private Ice ice;
    private Fire fire;
    private PlayerCombat playerCombat;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        warp = player.GetComponent<Warp>();
        ice = player.GetComponent<Ice>();
        fire = player.GetComponent<Fire>();
        playerCombat = player.GetComponent<PlayerCombat>();
        playerRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
        playerMaterialDefault = playerRenderer.materials;

        actionCooldowns = new float[3] { 10f, 10f, 10f }; 
        isActionCooldowns = new bool[3] { false, false, false };
       
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
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentState != State.State1)
        {
            ChangeState(State.State1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentState != State.State2 && state2Unlocked)
        {
            ChangeState(State.State2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && currentState != State.State3 && state3Unlocked)
        {
            ChangeState(State.State3);
        }

        if (Input.GetKeyUp(KeyCode.E) && !isActionCooldown)
        {
            if (playerCombat.isUltimateActive) return;
            StartCoroutine(PerformActionWithCooldown());
        }
    }

    // Perform the action for the current CurrentState with a cooldown
    private IEnumerator PerformActionWithCooldown()
    {
        int stateIndex = (int)currentState;

        if (!isActionCooldowns[stateIndex])
        {
            bool actionPerformed = PerformStateSpecificAction();

            if (actionPerformed)
            {
                isActionCooldowns[stateIndex] = true;
                yield return new WaitForSeconds(actionCooldowns[stateIndex]);
                isActionCooldowns[stateIndex] = false;
            }
        }
    }

    // Perform the action for the current CurrentState
    private bool PerformStateSpecificAction()
    {
        if (currentState == State.State1)
        {
            ice.StartIce();
            return true;
        }
        else if (currentState == State.State2)
        {
            fire.StartFire();
            return true;
        }
        else if (currentState == State.State3)
        {
            return warp.TryStartWarp();
        }
        return false;
    }

    public void UnlockState2()
    {
        orbPickupSFX.Play();
        state2Unlocked = true;
    }

    public void UnlockState3()
    {
        orbPickupSFX.Play();
        state3Unlocked = true;
    }


    private void ChangeState(State newState)
    {
        currentState = newState;
        UpdateMaterialsAndTrail();
        StartCoroutine(FlashPlayerMaterial(newState));
        UpdateStateInfo();
    }

    #region Update Player Material
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

    // Update the player material to reflect the current CurrentState
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

    #endregion

    #region Update UI
    // Update the UI info to reflect the current CurrentState
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
    #endregion

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
