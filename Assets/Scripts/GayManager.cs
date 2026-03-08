using UnityEngine;

public class GayManager : MonoBehaviour
{
    public static GayManager Instance;

    public bool HasKey = false;
    public GameObject Player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DamagePlayer(int damage)
    {
        // kept for compatibility
    }

    /// <summary>
    /// Call this when the player dies (from Health.cs or wherever death is handled).
    /// </summary>
    public void TriggerGameOver()
    {
        MenuManager.Instance.ShowGameOver();
    }

    /// <summary>
    /// Call this when the win condition is met (boss killed, objective completed, etc.).
    /// </summary>
    public void TriggerYouWon()
    {
        MenuManager.Instance.ShowYouWon();
    }
}