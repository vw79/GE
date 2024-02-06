using Cinemachine;
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

    private ParticleSystem fireEffect;
    private SphereCollider fireAttackCollider;

    private CinemachineImpulseSource impulseSource;

    private Cooldown redCDScript;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealthSystem>();
        playerCombat = GetComponent<PlayerCombat>();
        playerController = GetComponent<PlayerController>();
        redCDScript = GameObject.Find("RedCdUI").GetComponentInChildren<Cooldown>();
        fireEffect = GameObject.Find("FireAttack").GetComponent<ParticleSystem>();
        fireAttackCollider = GameObject.Find("FireAttack").GetComponent<SphereCollider>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        fireAttackCollider.enabled = false;
        fireEffect.Stop();
    }

    public void StartFire()
    {
        playerHealth.enabled = false;
        playerCombat.enabled = false;
        playerController.enabled = false;

        animator.Play("Fire");        
        redCDScript.UseSpell();
        StartCoroutine(WaitAnim());
    }

    IEnumerator WaitAnim() 
    {
        yield return new WaitForSeconds(1.25f);
        fireEffect.Play();
        CamShake.instance.CameraShake(impulseSource, 5f);
        fireAttackCollider.enabled = true;
        StartCoroutine(FireDamage()); 
    }

    IEnumerator FireDamage()
    {
        yield return new WaitForSeconds(1.5f);
        fireEffect.Stop();
        fireAttackCollider.enabled = false;
        playerCombat.enabled = true;
        playerController.enabled = true;
        playerHealth.enabled = true;
    }
}