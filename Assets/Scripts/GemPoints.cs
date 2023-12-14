using UnityEngine;
using Dreamteck.Forever;
using UnityEngine.UI;

public class GemPoints : MonoBehaviour
{
    public CounterManager CounterManager;
    public AudioManager audioManager;

    public int point = 1;

    void Start()
    {
        CounterManager = GameObject.Find("CounterManager").GetComponent<CounterManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Canary")
        {
            Destroy(gameObject);
            CounterManager.AddPoint();
            audioManager.PlaySFX(audioManager.gemSFX);
        }
    }
}
