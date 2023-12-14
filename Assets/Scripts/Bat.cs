using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    int health = 2;

    CounterManager counterManager;
    void Start()
    {
        counterManager = GameObject.Find("CounterManager").GetComponent<CounterManager>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Bat took damage. Now at: " + health + " health"); 

        if (health <= 0)
        {
            Debug.Log("Bat died"); 
            Destroy(gameObject);
            counterManager.batsKilled++; 
        }
    }
}
