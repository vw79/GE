using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public List<AttackSO> combo; // List of attack scriptable objects for combo attacks.
    float lastClickTime; // Time when the last attack was initiated.
    float lastComboEnd; // Time when the last combo ended.
    int comboCounter; // Counter to keep track of the combo sequence.

    Animator anim; // Animator component to control animations.
    [SerializeField] Weapon weapon; // Reference to the weapon script.

    public GameObject vfxPrefab; // Prefab for visual effects.
    public bool isAttacking; // Boolean to check if the player is currently attacking.

    void Start()
    {
        anim = GetComponent<Animator>(); // Get the Animator component attached to the player.
    }

    void Update()
    {
        // Check for mouse button click to initiate an attack.
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        // Call to check if the attack animation has finished and possibly end the combo.
        ExitAttack();
    }

    void Attack()
    {
        // Check if enough time has passed since the last click to process a new attack.
        if (Time.time - lastClickTime > 0.7f)
        {
            // Reset the EndCombo invocation.
            CancelInvoke("EndCombo");

            // Additional time check to prevent too rapid attacks.
            if (Time.time - lastClickTime >= 0.1f)
            {
                // Check if the comboCounter is within the bounds of the combo list.
                if (comboCounter < combo.Count)
                {
                    // Set the animator override controller and play the attack animation.
                    anim.runtimeAnimatorController = combo[comboCounter].animatorOV;
                    anim.Play("Attack", 0, 0);
                    // Set the damage of the weapon based on the current combo attack.
                    weapon.damage = combo[comboCounter].damage;
                }

                // Increment the combo counter and update the last click time.
                comboCounter++;
                lastClickTime = Time.time;

                // Reset the comboCounter if it exceeds the size of the combo list.
                if (comboCounter >= combo.Count)
                {
                    comboCounter = 0;
                }
            }
        }
        // Set isAttacking to true as the attack begins.
        isAttacking = true;
    }

    void ExitAttack()
    {
        // Check if the attack animation is almost finished.
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            // Schedule to end the combo after a delay.
            Invoke("EndCombo", 1);
        }
    }

    void EndCombo()
    {
        // Reset the combo counter and update the time the last combo ended.
        comboCounter = 0;
        lastComboEnd = Time.time;
        // Set isAttacking to false as the combo ends.
        isAttacking = false;
    }

    public void Trigger()
    {
        // Debug log for testing or triggering an event.
        Debug.Log("yo");
    }
}
