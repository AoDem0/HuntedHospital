using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField]private soundManager soundMan;
    [SerializeField] private Slider[] sliders;
    [SerializeField] private GameObject comicsImg;
    private int comicIdx = 0;
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
        SceneManager.LoadScene("Story");
        
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

    public void SaveButton()
    {
        soundMan.Play("uiclick");
        //sliders[0];

    }

    public void HowToPlayPage()
    {
        soundMan.Play("uiclick");
        SceneManager.LoadScene("HowToPlay");
    public void NextSlide()
    {
        soundMan.Play("uiclick");
        if (comicIdx >= 1)
        {
            SceneManager.LoadScene("MainHospitalScene");
        }
        else
        {
            comicsImg.SetActive(true);
            comicIdx += 1;
        }
    }
}
