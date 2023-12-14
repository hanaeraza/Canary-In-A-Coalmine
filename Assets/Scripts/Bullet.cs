using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    CounterManager counterManager;
    void Awake()
    {
        Destroy(gameObject, 3);
        counterManager = GameObject.Find("CounterManager").GetComponent<CounterManager>();
    }

    void OnTriggerEnter(Collider collision)
    {
        Bat bat = collision.GetComponent<Bat>();
        if (bat != null)
        {
            bat.TakeDamage(counterManager.damage);
        }
        //Debug.Log("Collision!"); 
        Destroy(gameObject); 
    }
}
