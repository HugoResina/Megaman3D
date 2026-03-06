using System.Collections;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Rigidbody rb;
    private float damage = 25f;
    private bool isHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public static void Create(GameObject prefab, Vector3 position, Quaternion rotation, float speed, float lifeTime)
    {
        GameObject obj = Instantiate(prefab, position, rotation);

        BossBullet bullet = obj.GetComponent<BossBullet>();
        if (bullet != null)
        {
            bullet.ExecuteShoot(rotation * Vector3.forward, speed, lifeTime);
        }
    }

    private void ExecuteShoot(Vector3 direction, float speed, float lifeTime)
    {
        isHit = false;
        rb.isKinematic = false;
        rb.linearVelocity = direction * speed;

        StartCoroutine(ReturnAfterTime(lifeTime));
    }

    private IEnumerator ReturnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (this != null) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHit) return;
        isHit = true;

        
        Destroy(gameObject);
    }
}