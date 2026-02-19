using UnityEngine;

public class RobotShooter : MonoBehaviour
{
    public RobotPool pool;

    public Transform[] shootPoints;

    
   

    public float projectileLife = 3f;
    public float projectileSpeed = 20f;

   
    public void Shoot()
    {
        foreach (Transform shootPoint in shootPoints)
        {
          
            RobotBullet bullet = pool.GetBullet();

            if (bullet != null)
            {
                bullet.transform.position = shootPoint.position;
                bullet.transform.rotation = shootPoint.rotation;
                bullet.Shoot(shootPoint.forward, projectileSpeed, projectileLife);
            }
        }
    }
}
