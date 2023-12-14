using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dreamteck.Forever;

public class CounterManager : MonoBehaviour
{

    public int points = 0;
    public int pointsToAdd = 1;
    public int batsKilled = 0;
    public int damage = 1;
    public float attractRadius = 0.37f;
    public float moveSpeed;

    public TMP_Text GemCounter;
    public GameObject player;
    public Vector3 playerPosition = Vector3.zero;

    void Update()
    {
        // Upgrade: Once player kills 5 bats, do 2x damage to bats
        if (batsKilled == 5)
            damage = 2;


        // Upgrade: Once player collects 30 gems, activate gem magnet
        if (points >= 30)
        {
            playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            GemMagnet();
        }
        
    }

    public void AddPoint()
    {
        points += pointsToAdd; 
        GemCounter.text = points.ToString();
    }

    public void AddPoint(int pointAdd)
    {
        points += pointAdd;
        GemCounter.text = points.ToString();
    }

    public void GemMagnet()
    {
        
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, attractRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Gem"))
            {
                moveSpeed = player.GetComponent<Runner>().followSpeed + 0.2f; // Make sure gem can catch up to player
                hitCollider.transform.position = Vector3.MoveTowards(hitCollider.transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            }
        }
    }
    // Show gem magnet range (For debugging)
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerPosition, attractRadius); 
    }


}
