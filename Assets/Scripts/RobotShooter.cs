using UnityEngine;

public class RobotShooter : MonoBehaviour
{
    public RobotPool pool;

    public Transform[] shootPoints;

    
   

    public float projectileLife = 3f;
    public float projectileSpeed = 20f;

   
    public void Shoot(Transform objectiu)
    {
        foreach (Transform shootPoint in shootPoints)
        {
          
            RobotBullet bullet = pool.GetBullet();

            if (bullet != null)
            {
                Vector3 dir = objectiu.position - transform.position;
                bullet.transform.position = shootPoint.position;
                bullet.transform.rotation = shootPoint.rotation;
                bullet.Shoot(dir , projectileSpeed, projectileLife);
            }
        }
    }
}
