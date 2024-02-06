using Cinemachine;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator animator;
    [HideInInspector] public float damage;
    BoxCollider triggerBox;
    public PlayerCombat playerCombat;
    private StateManager stateManager;
    private ChromaticAberrationEffect chromaticEffect;
    private CinemachineImpulseSource impulseSource;
    private EnemyAI enemyAI;
    private PlayerHealthSystem playerHealth;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        triggerBox = GetComponent<BoxCollider>();
        stateManager = FindObjectOfType<StateManager>();
        chromaticEffect = FindObjectOfType<ChromaticAberrationEffect>();
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealthSystem>();
    }



    void OnTriggerEnter(Collider other)
    {
        if (playerCombat.isAttacking && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (stateManager != null && stateManager.CanAttack(other.gameObject))
            {
                enemyAI = other.GetComponent<EnemyAI>();
                playerHealth.Heal(10);
                enemyAI.takeDamage(damage);
            }
            else
            {
                chromaticEffect?.TriggerChromaAb();
                CamShake.instance.CameraShake(impulseSource, 0.2f);
                animator.SetTrigger("WrongColor");
            }
        }
    }

    public void EnableTrigger()
    {
        triggerBox.enabled = true;
    }

    public void DisableTrigger()
    {
        triggerBox.enabled = false;
    }
}
