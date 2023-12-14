using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject DifficultyPanel;

    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1); 
    }

    public void Restart()
    {
        SceneManager.LoadScene(0); 
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }

    public void OpenDifficultyPanel()
    {
        DifficultyPanel.SetActive(true);
    }

    public void ChooseDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
        Debug.Log("Chose difficulty:" + difficulty); 
        DifficultyPanel.SetActive(false); 
    }


}
