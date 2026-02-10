using System.Collections;
using UnityEngine;

public class WaspBehaviour : MonoBehaviour
{
    private float ShootRate = 0.2f;
    private bool CanShoot = true;
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("te veo");
            transform.LookAt(other.transform.position);
            if (CanShoot)
            {
                Debug.Log("pium");
                CanShoot = false;
                StartCoroutine(ShootPlayer());
            }
            
        }

    }

    private IEnumerator ShootPlayer()
    {
        //yield return new WaitForSeconds(ShootRate);

        var waitForGrounded = new WaitUntil(() => CanShoot);
        

        yield return new WaitForSeconds(ShootRate);
        CanShoot = true ;
    }
}
