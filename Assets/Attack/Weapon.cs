using Cinemachine;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator animator;
    public float damage;
    BoxCollider triggerBox;
    public PlayerCombat playerCombat;
    private StateManager stateManager;
    private ChromaticAberrationEffect chromaticEffect;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    
    void Start()
    {
        triggerBox = GetComponent<BoxCollider>();
        stateManager = FindObjectOfType<StateManager>();
        chromaticEffect = FindObjectOfType<ChromaticAberrationEffect>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (playerCombat.isAttacking && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (stateManager != null && stateManager.CanAttack(other.gameObject))
            {
                // Destroy or damage the enemy
                Destroy(other.gameObject);
            }
            else
            {
                chromaticEffect?.TriggerChromaAb();
                CamShake.instance.CameraShake(impulseSource);
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
