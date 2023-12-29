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

    // References
    Animator anim;
    [SerializeField] Weapon weapon;
    public GameObject swordTrails;
    private float trailDuration = 0.2f;
    public BoxCollider swordCollider;
    private float hitDuration = 0.2f;
    public PlayerController playerController;

    #endregion

    public GameObject ult;

    void Start()
    {
        anim = GetComponent<Animator>();
        swordTrails.SetActive(false);
        swordCollider.enabled = false;
        ult.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            Attack();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Ultimate();
        }

        ExitAttack();
    }

    #region Attack Combo
    void Attack()
    {

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
                    anim.Play("Attack", 0, 0);
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
        StartCoroutine(EnableSwordCollider());
    }

    private IEnumerator EnableSwordCollider()
    {
        swordCollider.enabled = true;

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

    private void Ultimate() 
    {
        StartAttack();
        anim.SetBool("Ultimate", true);
        anim.Play("Ult");
        StartCoroutine(Charge());
    }


    private IEnumerator Charge()
    {
        //Disable Health System
        
        yield return new WaitForSeconds(1.6f);
        StartCoroutine(UltDamage());
    }

    private IEnumerator UltDamage()
    {
        ult.SetActive(true);
        yield return new WaitForSeconds(.8f);    
        UltEnd();
    }

    private void UltEnd()
    {
        anim.SetBool("Ultimate", false);
        ult.SetActive(false);
        AnimationAttackEnd();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}