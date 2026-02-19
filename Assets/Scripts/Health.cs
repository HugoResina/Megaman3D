using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float healthRegenRate = 5f; // Health points regenerated per second
    [SerializeField]
    private float healthRegenDelay = 3f; // Time in seconds before health starts regenerating after taking damage
    [SerializeField]
    private float damageCooldown = 1f; // Time in seconds during which the player cannot take damage after being hit
    [SerializeField]
    private float lastDamageTime = -Mathf.Infinity; // Time when the player last took damage
    [SerializeField]
    private float lastRegenTime = -Mathf.Infinity; // Time when health regeneration last occurred
    [SerializeField]
    private bool isDead = false;
    [SerializeField]
    private bool canTakeDamage = true;
    [SerializeField]
    private bool isRegenerating = false;
    [SerializeField]
    private UIManager uiManager;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void Update()
    {
        if (isDead)
            return;

        if (!canTakeDamage && Time.time - lastDamageTime >= damageCooldown)
        {
            canTakeDamage = true;
        }

        if (!isRegenerating && Time.time - lastDamageTime >= healthRegenDelay)
        {
            isRegenerating = true;
            lastRegenTime = Time.time;
        }

        if (isRegenerating && currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            lastRegenTime = Time.time;
            UpdateHealthBar();
        }
        //Debug.Log($"Health updated: {uiManager.HealthBarFill.fillAmount}");
        //Debug.Log($"Current Health: {currentHealth}");
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead || !canTakeDamage)
            return;

        currentHealth -= damageAmount;
        lastDamageTime = Time.time;
        canTakeDamage = false;
        isRegenerating = false;

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player has died.");
    }

    private void UpdateHealthBar()
    {
        //Debug.Log($"Updating health bar: Current Health = {currentHealth}, Max Health = {maxHealth}");
        if (uiManager != null)
        {
            uiManager.HealthBarFill.fillAmount = currentHealth / maxHealth;
            Debug.Log($"Health bar fill amount set to: {uiManager.HealthBarFill.fillAmount}");
        }
    }
}