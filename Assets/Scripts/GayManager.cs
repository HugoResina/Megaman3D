using UnityEngine;

public class GayManager : MonoBehaviour
{
    public static GayManager Instance;

    private bool _hasKey = false;
    public bool HasKey
    {
        get => _hasKey;
        set
        {
            _hasKey = value;
            UIManager.Instance?.UpdateKeyIndicator(_hasKey);
        }
    }

    public GameObject Player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TriggerGameOver() => MenuManager.Instance.ShowGameOver();
    public void TriggerYouWon() => MenuManager.Instance.ShowYouWon();
}