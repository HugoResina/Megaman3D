using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Interaction")]
    public Text InteractText;

    [Header("Health Bar")]
    public Image HealthBarFill;
    public Text HealthText; // optional: shows "75 / 100"

    [Header("Crosshair")]
    public GameObject Crosshair;

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


    /// <summary>
    /// Called by Health.cs every time the player takes damage or heals.
    /// fillAmount is a 0–1 value.
    /// </summary>
    public void UpdateHealthBar(float current, float max)
    {
        if (HealthBarFill != null)
            HealthBarFill.fillAmount = current / max;

        if (HealthText != null)
            HealthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }


    public void ShowCrosshair()
    {
        if (Crosshair != null)
            Crosshair.SetActive(true);
    }

    public void HideCrosshair()
    {
        if (Crosshair != null)
            Crosshair.SetActive(false);
    }


    public void ShowInteractPrompt(string message = "Press E to interact")
    {
        if (InteractText != null)
        {
            InteractText.text = message;
            InteractText.gameObject.SetActive(true);
        }
    }

    public void HideInteractPrompt()
    {
        if (InteractText != null)
            InteractText.gameObject.SetActive(false);
    }
}