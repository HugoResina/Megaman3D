using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
public class RobotBullet : MonoBehaviour
{
    private Rigidbody rb;
    private RobotPool pool;
    private Coroutine returnCoroutine;
    private float ExplosionRadius = 1.3f;
    private float RocketDamage = 25f;
    private bool isHit = false;
    public LayerMask playerLayer;
    public GameObject Explosion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
       
    }
    public void Initialize( RobotPool poolReference)
    {
        
        pool = poolReference;
    }

    public void Shoot(Vector3 direction, float speed, float lifeTime)
    {
        isHit  = false;
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
        if (isHit) return; 
        isHit = true;

        ExplosionDamage(transform.position, ExplosionRadius);
        Instantiate(Explosion, transform.position, Quaternion.identity);
        ReturnToPool();
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, playerLayer);
        System.Collections.Generic.List<GameObject> objectsHit = new System.Collections.Generic.List<GameObject>();

        foreach (var hitCollider in hitColliders)
        {
            GameObject rootEntity = hitCollider.attachedRigidbody != null
                                    ? hitCollider.attachedRigidbody.gameObject
                                    : hitCollider.gameObject;

            if (!objectsHit.Contains(rootEntity))
            {
                Debug.Log("Impactado: " + rootEntity.name);


                objectsHit.Add(rootEntity);
            }
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
