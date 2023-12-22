using System.Collections;
using System.Collections.Generic;
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

    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        swordTrails.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            Attack();
        }

        ExitAttack();
    }

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

        

        // VFX
        if (isAttacking)
        {
            swordTrails.SetActive(true);
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
            Invoke("EndCombo", .5f); // Time to determine if the player is still attacking
        }
    }

    public void AnimationAttackEnd()
    {
        isAttacking = false;
    }
}