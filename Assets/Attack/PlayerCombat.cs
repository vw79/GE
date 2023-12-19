using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public List<AttackSO> combo;
    float lastClickTime;
    float lastComboEnd;
    int comboCounter;

    Animator anim;
    [SerializeField] Weapon weapon;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        ExitAttack();
    }

    void Attack()
    {
        if (Time.time - lastClickTime > 0.7f)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickTime >= 0.1f)
            {
                if (comboCounter < combo.Count)
                {
                    anim.runtimeAnimatorController = combo[comboCounter].animatorOV;
                    anim.Play("Attack", 0, 0);
                    weapon.damage = combo[comboCounter].damage;
                }

                comboCounter++;
                lastClickTime = Time.time;

                // Reset the comboCounter if it exceeds the size of the combo list
                if (comboCounter >= combo.Count)
                {
                    comboCounter = 0;
                }
            }
        }
    }

    void ExitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.98f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1);
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}
