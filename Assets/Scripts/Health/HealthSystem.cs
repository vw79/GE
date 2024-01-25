using UnityEngine;
using UnityEngine.Events;

public class HealthSystem: MonoBehaviour
{
    [SerializeField] private float max_health = 100f;
    private float current_health;
    public HealthBar healthBar;

    public UnityEvent OnDeath;
    public UnityEvent OnHurt;

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
        current_health -= damage;
        healthBar.SetHealth(current_health);

        if (current_health <= 0)
        {
            OnDeath.Invoke();
        }
        else
        {
            OnHurt.Invoke();
        }
    }
}
