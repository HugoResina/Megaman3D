using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private ObjectPool pool;

    public BulletType Type { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(BulletType type, ObjectPool poolReference)
    {
        Type = type;
        pool = poolReference;
    }

    public void Shoot(Vector3 direction, float speed, float lifeTime)
    {
        gameObject.SetActive(true);
        rb.linearVelocity = direction * speed;

        StartCoroutine(ReturnAfterTime(lifeTime));
    }

    private IEnumerator ReturnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        pool.ReturnBullet(this);
    }

    public void Deactivate()
    {
        rb.linearVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
