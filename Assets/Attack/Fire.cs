using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{ 
    public LayerMask enemyLayer;
    private Animator animator;
    private PlayerCombat playerCombat;
    private PlayerController playerController;
    private PlayerHealthSystem playerHealth;
    private CapsuleCollider playerCollider;
    

    private ParticleSystem fireEffect;
    private SphereCollider fireAttackCollider;

    private Cooldown redCDScript;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealthSystem>();
        playerCombat = GetComponent<PlayerCombat>();
        playerController = GetComponent<PlayerController>();
        playerCollider = GetComponent<CapsuleCollider>();
        redCDScript = GameObject.Find("RedCdUI").GetComponentInChildren<Cooldown>();
        fireEffect = GameObject.Find("FireAttack").GetComponent<ParticleSystem>();
        fireAttackCollider = GameObject.Find("FireAttack").GetComponent<SphereCollider>();
    }

    private void Start()
    {
        fireAttackCollider.enabled = false;
        fireEffect.Stop();
    }

    public void StartFire()
    {
        animator.Play("Fire");
        redCDScript.UseSpell();
        StartCoroutine(WaitAnim());
    }

    IEnumerator WaitAnim() 
    {
        yield return new WaitForSeconds(1.25f);
        fireEffect.Play();
        fireAttackCollider.enabled = true;
        StartCoroutine(FireDamage()); 
    }

    IEnumerator FireDamage()
    {
        yield return new WaitForSeconds(1.2f);
        fireEffect.Stop();
        fireAttackCollider.enabled = false;
        playerCombat.enabled = true;
        playerController.enabled = true;
        playerCollider.enabled = true;
        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        yield return new WaitForSeconds(10f);
        playerHealth.enabled = true;
    }
}