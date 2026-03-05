using UnityEngine;

public class FallingVFXTransition : MonoBehaviour
{
    public GameObject Dust;
    public GameObject Boss;
    public void DustCloud()
    {
        Instantiate(Dust, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
        Boss.SetActive(true);
    }
}
