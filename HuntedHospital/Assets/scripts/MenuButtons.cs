using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField]private soundManager soundMan;
    private void Start()
    {
        soundMan = FindAnyObjectByType<soundManager>();
    }

    public void QuitGame()
    {
        soundMan.Play("uiclick");
        Application.Quit();
    }
    public void GameStart()
    {   
        soundMan.Play("uiclick");
        SceneManager.LoadScene("MainHospitalScene");
        
    }

    public void BackButton()
    {
        soundMan.Play("uiclick");
        SceneManager.LoadScene("Menu");
    }
    public void SettingsButton()
    {
        soundMan.Play("uiclick");
        SceneManager.LoadScene("SoundSettings");
    }
}
