using UnityEngine;

public class GayManager : MonoBehaviour
{
    public static GayManager Instance;
    public GameObject boss;
    public GameObject TruckL;
    public GameObject TruckR;
    public bool hasOpened = false;
    Vector3 initialPosL;
    Vector3 initialPosR;
    Vector3 goToL;
    Vector3 goToR;
    public bool _hasKey = false;
    public bool reachedRespawn = false;
    public Transform respawnPoint;
    public GameObject blockingCollider;
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
        initialPosL = TruckL.transform.position;
        initialPosR = TruckR.transform.position;
        goToL = new Vector3(initialPosL.x, initialPosL.y, initialPosL.z - 5);
        goToR = new Vector3(initialPosR.x, initialPosR.y, initialPosR.z + 5);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (hasOpened)
        {
            TruckL.transform.position = goToL;
            TruckR.transform.position = goToR;
        }
    }

    public void TriggerGameOver()
    {
        MenuManager.Instance.ShowGameOver();
    }

    public void TriggerYouWon() => MenuManager.Instance.ShowYouWon();

    public void OpenDoor()
    {
        if (!hasOpened)
        {
            hasOpened = true;
            boss.SetActive(true);
            if (blockingCollider != null) blockingCollider.SetActive(false);
        }
    }

    public void CloseDoor()
    {
        if (!hasOpened) return;

        hasOpened = false;
        TruckL.transform.position = initialPosL;
        TruckR.transform.position = initialPosR;
        boss.SetActive(false);
        if (blockingCollider != null) blockingCollider.SetActive(true);
    }
}