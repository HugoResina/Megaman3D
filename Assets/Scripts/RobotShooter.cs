using UnityEngine;

public class RobotShooter : MonoBehaviour
{
    public RobotPool pool;

    public Transform[] shootPoints;

    
   

    public float projectileLife = 3f;
    public float projectileSpeed = 20f;

    public void ChooseProj()
    {
        //Debug.Log($"Time Pressed: {timePressed}");
        

       

        Shoot();
    }

    private void Shoot()
    {
        RobotBullet bullet = pool.GetBullet();

        if (bullet == null)
            return;
        foreach(Transform shootPoint in shootPoints)
        {
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = shootPoint.rotation;

            bullet.Shoot(shootPoint.forward, projectileSpeed, projectileLife);
        }
       
    }
}
