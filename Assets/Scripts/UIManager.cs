using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject crosshair;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image healthBarBackground;
    [SerializeField] private GameObject keyIcon;
    [SerializeField] private GameObject noKeyIcon;
    [SerializeField] private GameObject hudRoot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateKeyIndicator(false);
    }

    public void ShowCrosshair()
    {
        if (crosshair != null) crosshair.SetActive(true);
    }

    public void HideCrosshair()
    {
        if (crosshair != null) crosshair.SetActive(false);
    }

    public void ShowHUD()
    {
        if (hudRoot != null) hudRoot.SetActive(true);
    }

    public void HideHUD()
    {
        if (hudRoot != null) hudRoot.SetActive(false);
    }

    public void UpdateHealthBar(float current, float max)
    {
        if (healthBarFill == null) return;
        healthBarFill.fillAmount = Mathf.Clamp01(current / max);

        if (healthBarFill.fillAmount > 0.5f)
            healthBarFill.color = new Color(0.2f, 0.9f, 0.2f);
        else if (healthBarFill.fillAmount > 0.25f)
            healthBarFill.color = new Color(0.95f, 0.8f, 0.1f);
        else
            healthBarFill.color = new Color(0.95f, 0.2f, 0.2f);
    }

    public void UpdateKeyIndicator(bool hasKey)
    {
        if (keyIcon != null) keyIcon.SetActive(hasKey);
        if (noKeyIcon != null) noKeyIcon.SetActive(!hasKey);
    }
}