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

    }


}
