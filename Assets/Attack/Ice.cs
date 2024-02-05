using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Ice : MonoBehaviour
{
    public LayerMask enemyLayer;
    private Animator animator;
    private PlayerCombat playerCombat;
    private PlayerController playerController;
    private CapsuleCollider playerCollider;
    private PlayerHealthSystem playerHealth;

    private VisualEffect iceEffect;
    private BoxCollider iceAttackCollider;

    private Cooldown blueCDScript;
    private GameObject blueCD;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealthSystem>();
        playerCombat = GetComponent<PlayerCombat>();
        playerController = GetComponent<PlayerController>();
        playerCollider = GetComponent<CapsuleCollider>();
        blueCD = GameObject.Find("BlueCdUI");
        blueCDScript = blueCD.GetComponentInChildren<Cooldown>();
        iceEffect = GameObject.Find("IceAttack").GetComponent<VisualEffect>();
        iceAttackCollider = GameObject.Find("IceAttack").GetComponent<BoxCollider>();
    }

    private void Start()
    {
        iceAttackCollider.enabled = false;
        iceEffect.Stop();     
    }

    public void StartIce()
    {
        animator.Play("Ice");
        iceEffect.Play();
        blueCDScript.UseSpell();
        iceAttackCollider.enabled = true;
        StartCoroutine(IceDamage());
    }

    IEnumerator IceDamage()
    {
        yield return new WaitForSeconds(1.7f);
        iceEffect.Stop();
        iceAttackCollider.enabled = false;
        playerCombat.enabled = true;
        playerController.enabled = true;
        playerCollider.enabled = true;
        StartCoroutine (Invincible());
    }

    IEnumerator Invincible()
    {
        yield return new WaitForSeconds(10f);      
        playerHealth.enabled = true;
    }

    public IEnumerator SlowEnemy(EnemyAI enemy)
    {
        float originalSpeed = enemy.navMeshAgent.speed;
        enemy.navMeshAgent.speed *= (1 - 0.5f);

        yield return new WaitForSeconds(5f);

        enemy.navMeshAgent.speed = originalSpeed;

    }
}