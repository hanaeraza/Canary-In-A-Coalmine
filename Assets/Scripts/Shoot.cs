using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform firingPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 2;
    public float fireRate = 0.3f;
    float timeUntilFire;
    
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > timeUntilFire) {
            
            var bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = firingPoint.forward * bulletSpeed;
            timeUntilFire = Time.time + fireRate;
        }
        
    }
}
