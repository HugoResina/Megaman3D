using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;

    [Header("Optional - only needed on the Player")]
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private bool isBoss = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    private void Die()
    {
        if (isPlayer)
            GayManager.Instance.TriggerGameOver();
        else if (isBoss)
            GayManager.Instance.TriggerYouWon();
        else
            Destroy(gameObject);
    }

    private void UpdateHealthUI()
    {
        if (!isPlayer) return;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
}