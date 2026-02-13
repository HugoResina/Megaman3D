using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private ObjectPool pool;
    private Coroutine returnCoroutine;
    private float SmallDmg = 10f;
    private float MediumDmg = 25f;
    private float LargeDmg = 50f;

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
            float damage = Type switch
            {
                BulletType.Small => SmallDmg,
                BulletType.Medium => MediumDmg,
                BulletType.Large => LargeDmg,
                _ => 0f
            };
            health.TakeDamage(damage);
        }
        if (!collision.gameObject.CompareTag("Player"))
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
