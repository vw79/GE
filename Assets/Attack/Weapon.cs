using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    BoxCollider triggerBox;
    public PlayerCombat playerCombat;
    private StateManager stateManager;
    private ChromaticAberrationEffect chromaticEffect;

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
                // Trigger chromatic aberration effect if state or layer condition not met
                chromaticEffect?.TriggerChromaAb();
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
