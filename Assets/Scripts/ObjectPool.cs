using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Prefabs")]
    public Bullet smallPrefab;
    public Bullet mediumPrefab;
    public Bullet largePrefab;

    public int poolSize = 50;

    private Queue<Bullet> pool = new Queue<Bullet>();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AddBulletToPool(smallPrefab, BulletType.Small);
            AddBulletToPool(mediumPrefab, BulletType.Medium);
            AddBulletToPool(largePrefab, BulletType.Large);
        }
    }

    private void AddBulletToPool(Bullet prefab, BulletType type)
    {
        Bullet bullet = Instantiate(prefab, transform);
        bullet.Initialize(type, this);
        bullet.gameObject.SetActive(false);
        pool.Enqueue(bullet);
    }

    public Bullet GetBullet(BulletType type)
    {
        foreach (Bullet bullet in pool)
        {
            if (bullet.Type == type && !bullet.gameObject.activeInHierarchy)
            {
                pool.Dequeue();
                return bullet;
            }
        }

        return null;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.Deactivate();
        pool.Enqueue(bullet);
    }
}
