using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem: MonoBehaviour
{
    [SerializeField] private float max_health = 100f;
    public Animator animator;   
    public PlayerController playerController;
    private float current_health;
    public HealthBar healthBar;
    private ChromaticAberrationEffect chromaticEffect;
    private CinemachineImpulseSource impulseSource;
    public bool isDead;
    public PlayerCombat playerCombat;

    public UnityEvent OnDeath;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        chromaticEffect = FindObjectOfType<ChromaticAberrationEffect>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Start()
    {
        current_health = max_health;
        healthBar.SetHealth(current_health);
    }

    public float GetHealth()
    {
        return current_health;
    }

    public float GetMaxHealth()
    {
        return max_health;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        if (playerCombat.isUltimateActive) return;

        current_health -= damage;
        playerController.enabled = false;
        animator.Play("Impact");
        chromaticEffect?.TriggerChromaAb();
        CamShake.instance.CameraShake(impulseSource);

        Debug.Log("Health: " + current_health);
        
        healthBar.SetHealth(current_health);

        if (current_health <= 0)
        {
            if (!isDead)
            {
                isDead = true;
                OnDeath.Invoke();
            }
        }
    }
}
