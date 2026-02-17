using UnityEngine;
using System;
using System.Collections;
public class RobotBullet : MonoBehaviour
{
    private Rigidbody rb;
    private RobotPool pool;
    private Coroutine returnCoroutine;
   
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
        rb.linearVelocity = direction * speed;

        // Guardamos la coroutine para poder cancelarla
        returnCoroutine = StartCoroutine(ReturnAfterTime(lifeTime));
    }

    private IEnumerator ReturnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            float damage = RocketDamage;
            health.TakeDamage(damage);
        }
        if (!collision.gameObject.CompareTag("Player") && collision.gameObject.layer != 7)
        {
            ReturnToPool();
        }
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
        gameObject.SetActive(false);
    }
}
