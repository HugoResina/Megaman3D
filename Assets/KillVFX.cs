using System.Collections;
using UnityEngine;

public class KillVFX : MonoBehaviour
{
    [SerializeField]
    float TTK = 1;
    private void OnEnable()
    {
        Debug.Log("hola");
        StartCoroutine(KillAnimation());
    }
   
    public IEnumerator KillAnimation()
    {
        Debug.Log("memato");
        yield return new WaitForSeconds(TTK);
        for(int i=0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);//.SetActive(false);
        }
        Destroy(gameObject);
    }
}
