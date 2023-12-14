using UnityEngine;
using Dreamteck.Forever;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Canary")
        {
            Destroy(gameObject);
            Debug.Log("Power up hit");
            audioManager.PlaySFX(audioManager.powerUpSFX);
        }
    }
}
