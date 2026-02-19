using System.Collections.Generic;
using UnityEngine;

public class RobotPool : MonoBehaviour
{
    [Header("Prefabs")]
    public RobotBullet rocket;


    public int poolSize = 25;

    private Queue<RobotBullet> pool = new Queue<RobotBullet>();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AddBulletToPool(rocket);
            
        }
    }

    private void AddBulletToPool(RobotBullet prefab)
    {
       
        RobotBullet bullet = Instantiate(prefab);
        bullet.Initialize(this);
        bullet.gameObject.SetActive(false);
        pool.Enqueue(bullet);
    }

    public RobotBullet GetBullet()
    {
        //foreach (RobotBullet bullet in pool)
        //{
        //    if (!bullet.gameObject.activeInHierarchy)
        //    {
        //        pool.Dequeue();
        //        return bullet;
        //    }
        //}
        if(pool.Count > 0)
        {
            RobotBullet bullet = pool.Dequeue();
          
            return bullet;
        }
        return null;
    }

    public void ReturnBullet(RobotBullet bullet)
    {
        bullet.Deactivate();
        pool.Enqueue(bullet);
    }
}
