using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    #region Variables

    // Attack Combo
    public List<AttackSO> combo;
    float lastClickTime;
    float lastComboEnd;
    int comboCounter;
    public bool isAttacking;
    public bool isUltimateActive;
    public AudioSource AttackSFX;

    // References
    private Animator anim;
    public Weapon weapon;
    public GameObject swordTrails;
    private float trailDuration = 0.2f;
    private BoxCollider swordCollider;
    private float hitDuration = 0.2f;
    private PlayerController playerController;
    private PlayerHealthSystem healthSystem;
    private GameObject player;

    [Header("SFX")]
    public AudioSource ultSFX;
 
    #endregion

    public GameObject ult;
    public UltMeter ultMeter;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        swordCollider = weapon.GetComponent<BoxCollider>();
        playerController = GetComponent<PlayerController>();
        healthSystem = GetComponent<PlayerHealthSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        swordTrails.SetActive(false);
        swordCollider.enabled = false;
        ult.SetActive(false);
    }

    void Update()
    {
        if (healthSystem.isDead) return;

        if (Input.GetMouseButtonDown(0) && !isUltimateActive)
        {
            isAttacking = true;
            Attack();
        }

        if (Input.GetKey(KeyCode.Q) && !isAttacking && ultMeter.canUlt)
        {
            Ultimate();
        }

        ExitAttack();
    }

    #region Attack Combo
    void Attack()
    {
        if (isUltimateActive) return;

        // Check if enough time has passed since the last click to process a new attack.
        if (Time.time - lastClickTime > 0.7f)
        {
            CancelInvoke("EndCombo");

            // Additional time check to prevent too rapid attacks.
            if (Time.time - lastClickTime >= 0.1f)
            {
                // Check if the comboCounter is within the bounds of the combo list.
                if (comboCounter < combo.Count)
                {

                    anim.runtimeAnimatorController = combo[comboCounter].animatorOV;
                    AttackSFX.Play();
                    anim.Play("Attack", 0, 0);

                    // Can set custom damage for each attack in the combo
                    weapon.damage = combo[comboCounter].damage;
                }

                comboCounter++;
                lastClickTime = Time.time;

                if (comboCounter >= combo.Count)
                {
                    comboCounter = 0;
                }
            }
        }
    }

    public void EndCombo()
    {
        isAttacking = false;
        comboCounter = 0;
        lastComboEnd = Time.time;
        swordTrails.SetActive(false);
    }

    void ExitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1f); // Time to determine if the player is still attacking
        }
    }
    #endregion

    #region Sword Collider
    public void SwordCollider()
    {
        swordCollider.enabled = true;
        StartCoroutine(EnableSwordCollider());
    }

    private IEnumerator EnableSwordCollider()
    {
        yield return new WaitForSeconds(hitDuration);
        swordCollider.enabled = false;
    }
    #endregion

    #region Sword Trails
    public void SwordTrails()
    {
        StartCoroutine(EnableSwordTrails());
    }

    private IEnumerator EnableSwordTrails()
    {
        swordTrails.SetActive(true);

        yield return new WaitForSeconds(trailDuration);

        swordTrails.SetActive(false);
    }
    #endregion

    #region Stop Movement During Attack
    public void StartAttack()
    {
        isAttacking = true;
        playerController.enabled = false;
    }

    public void AnimationAttackEnd()
    {
        isAttacking = false;
        playerController.enabled = true;
    }
    #endregion

    #region Ultimate

    private void Ultimate() 
    {
        isUltimateActive = true;
        StartAttack();
        anim.SetBool("Ultimate", true);
        anim.Play("Ult");
        StartCoroutine(Charge());
    }


    private IEnumerator Charge()
    {
        healthSystem.enabled = false;
        yield return new WaitForSeconds(1.6f);
        StartCoroutine(UltDamage());
    }

    private IEnumerator UltDamage()
    {
        ultSFX.Play();
        ult.SetActive(true);
        yield return new WaitForSeconds(.8f);    
        UltEnd();
    }

    private void UltEnd()
    {
        ultMeter.ResetUltBar();
        isUltimateActive = false;
        anim.SetBool("Ultimate", false);
        ult.SetActive(false);
        AnimationAttackEnd();
        healthSystem.enabled = true;
    }
    #endregion
}