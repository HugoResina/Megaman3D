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
    public string AuidoClipName;
    public float damage;
    public GameObject Explsion;

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
        AudioManager.instance.PlaySFX(AuidoClipName);
        gameObject.SetActive(true);
        rb.linearVelocity = direction * speed;

        returnCoroutine = StartCoroutine(ReturnAfterTime(lifeTime));
        damage = Type switch
        {
            BulletType.Small => SmallDmg,
            BulletType.Medium => MediumDmg,
            BulletType.Large => LargeDmg,
            _ => 0f
        };
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
            health.TakeDamage(damage);

        if (!collision.gameObject.CompareTag("Player"))
        {
            Instantiate(Explsion, transform.position, Quaternion.identity);
            AudioManager.instance.PlaySFX("HitExplosion");
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