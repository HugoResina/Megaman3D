using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{   
    public GameObject smallProjectile;
    public GameObject mediumProjectile;
    public GameObject largeProjectile;

    public int poolSize = 50;

    public Transform shootPoint;
    private Queue<GameObject> poolShort = new Queue<GameObject>();
    private Queue<GameObject> poolMedium = new Queue<GameObject>();
    private Queue<GameObject> poolLong = new Queue<GameObject>();

    public float mediumChargeTime = 0.5f;
    public float longChargeTime = 1.5f;

    public float maxTimePrefab = 3.0f;

    void Awake()
    {
        CreatePool(smallProjectile, poolShort);
        CreatePool(mediumProjectile, poolMedium);
        CreatePool(largeProjectile, poolLong);
    }
    void CreatePool(GameObject prefab, Queue<GameObject> pool)
    {
        //give them rigidbodies and colliders if they don't have them
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            obj.AddComponent<Rigidbody>();
            obj.GetComponent<Rigidbody>().useGravity = false;
            obj.GetComponent<Rigidbody>().freezeRotation = true;
            obj.AddComponent<SphereCollider>();
            obj.GetComponent<SphereCollider>().isTrigger = true;
            pool.Enqueue(obj);
        }
    }

    public void ChooseProj(double timePressed) 
    {
        Debug.Log("Time Pressed: " + timePressed);
        if (timePressed < mediumChargeTime)
        {
            Debug.Log("Shooting Small Projectile");
            ShootSmall();
        }
        else if (timePressed >= mediumChargeTime && timePressed < longChargeTime)
        {
            Debug.Log("Shooting Medium Projectile");
            ShootMedium();
        }
        else
        {
            Debug.Log("Shooting Large Projectile");
            ShootLarge();
        }
    }
    private void ShootSmall()
    {
        if (poolShort.Count > 0)
        {
            GameObject projectile = poolShort.Dequeue();
            projectile.transform.position = shootPoint.position;
            projectile.transform.rotation = shootPoint.rotation;
            projectile.SetActive(true);
            projectile.GetComponent<Rigidbody>().linearVelocity = shootPoint.forward * 20f;
            StartCoroutine(ReturnToPoolAfterDelay(projectile, poolShort, maxTimePrefab));
        }
    }
    private void ShootMedium()
    {
        if (poolMedium.Count > 0)
        {
            GameObject projectile = poolMedium.Dequeue();
            projectile.transform.position = shootPoint.position;
            projectile.transform.rotation = shootPoint.rotation;
            projectile.SetActive(true);
            projectile.GetComponent<Rigidbody>().linearVelocity = shootPoint.forward * 20f;
            StartCoroutine(ReturnToPoolAfterDelay(projectile, poolMedium, maxTimePrefab));
        }
    }
    private void ShootLarge()
    {
        if (poolLong.Count > 0)
        {
            GameObject projectile = poolLong.Dequeue();
            projectile.transform.position = shootPoint.position;
            projectile.transform.rotation = shootPoint.rotation;
            projectile.SetActive(true);
            projectile.GetComponent<Rigidbody>().linearVelocity = shootPoint.forward * 20f;
            StartCoroutine(ReturnToPoolAfterDelay(projectile, poolLong, maxTimePrefab));
        }
    }

    private System.Collections.IEnumerator ReturnToPoolAfterDelay(GameObject obj, Queue<GameObject> pool, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}