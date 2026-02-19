using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
public class RobotBullet : MonoBehaviour
{
    private Rigidbody rb;
    private RobotPool pool;
    private Coroutine returnCoroutine;
    private float ExplosionRadius = 2f;
    private float RocketDamage = 25f;

   

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize( RobotPool poolReference)
    {
        
        pool = poolReference;
    }

    public void Shoot(Vector3 direction, float speed, float lifeTime)
    {
        gameObject.SetActive(true);

       
        rb.isKinematic = false;
        rb.WakeUp(); 

       
        rb.linearVelocity = direction * speed;

        returnCoroutine = StartCoroutine(ReturnAfterTime(lifeTime));
    }

    private IEnumerator ReturnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool();
    }
    private void OnCollisionEnter(Collision collision)
    {
 

        ExplosionDamage(transform.position, ExplosionRadius);
        ReturnToPool();
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.layer == 3)
            Debug.Log("Impactado: " + hitCollider.name);
            
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
    private void ReturnToPool()
    {
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }

        pool.ReturnBullet(this);
    }

    public void Deactivate()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; 
        rb.isKinematic = true; 
        gameObject.SetActive(false);
    }
}
