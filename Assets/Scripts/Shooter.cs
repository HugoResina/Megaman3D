using UnityEngine;

public class Shooter : MonoBehaviour
{
    public ObjectPool pool;

    public Transform shootPoint;

    public float mediumChargeTime = 0.5f;
    public float longChargeTime = 1.5f;

    public float projectileLife = 3f;
    public float projectileSpeed = 20f;

    public void ChooseProj(double timePressed)
    {
        Debug.Log($"Time Pressed: {timePressed}");
        BulletType type;

        if (timePressed < mediumChargeTime)
            type = BulletType.Small;
        else if (timePressed < longChargeTime)
            type = BulletType.Medium;
        else
            type = BulletType.Large;

        Shoot(type);
    }

    private void Shoot(BulletType type)
    {
        Bullet bullet = pool.GetBullet(type);

        if (bullet == null)
            return;

        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.rotation;

        bullet.Shoot(shootPoint.forward, projectileSpeed, projectileLife);
    }
}
