using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text TimerText;
    public Player player;
    public CounterManager counterManager;

    public float elapsedTime;
    public int minutesPassed = 0;
    public bool oneMinuteMark = false;

    void Update()
    {
        if (player.playing)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);
            TimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");

            minutesPassed = minutes;
        }

        if (minutesPassed == 1 && oneMinuteMark == false)
        {
            InvokeRepeating("GemBonusUpgrade", 0f, 60f);
            oneMinuteMark = true;
        }

    }

    void GemBonusUpgrade()
    {
        Debug.Log("Survival Bonus: 20 gems");
        counterManager.AddPoint(20);
    }
}
