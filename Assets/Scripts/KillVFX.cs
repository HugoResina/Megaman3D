using System.Collections;
using UnityEngine;

public class KillVFX : MonoBehaviour
{
    [SerializeField]
    float TTK = 1;
    private void OnEnable()
    {
        StartCoroutine(KillAnimation());
    }
   
    public IEnumerator KillAnimation()
    {
        yield return new WaitForSeconds(TTK);
        for(int i=0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        Destroy(gameObject);
    }
}
