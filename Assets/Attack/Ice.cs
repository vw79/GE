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
    private PlayerHealthSystem playerHealth;

    private VisualEffect iceEffect;
    private BoxCollider iceAttackCollider;

    private Cooldown blueCDScript;
    private GameObject blueCD;

    private AudioSource iceAudioSource;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealthSystem>();
        playerCombat = GetComponent<PlayerCombat>();
        playerController = GetComponent<PlayerController>();
        blueCD = GameObject.Find("BlueCdUI");
        blueCDScript = blueCD.GetComponentInChildren<Cooldown>();
        iceEffect = GameObject.Find("IceAttack").GetComponent<VisualEffect>();
        iceAttackCollider = GameObject.Find("IceAttack").GetComponent<BoxCollider>();
        iceAudioSource = GameObject.Find("IceAttack").GetComponent<AudioSource>();
    }

    private void Start()
    {
        iceAttackCollider.enabled = false;
        iceEffect.Stop();     
    }

    public void StartIce()
    {
        playerHealth.enabled = false;
        playerCombat.enabled = false;
        playerController.enabled = false;

        animator.Play("Ice");
        iceEffect.Play();
        iceAudioSource.Play();
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
        playerHealth.enabled = true;
    }

    public IEnumerator SlowEnemy(EnemyAI enemy)
    {
        float originalSpeed = enemy.navMeshAgent.speed;
        float originalAnimationSpeed = enemy.animator.speed;

        enemy.navMeshAgent.speed *= 0.5f;
        enemy.animator.speed = 0.5f; 

        yield return new WaitForSeconds(5f);

        enemy.navMeshAgent.speed = originalSpeed;
        enemy.animator.speed = originalAnimationSpeed;
    }
}