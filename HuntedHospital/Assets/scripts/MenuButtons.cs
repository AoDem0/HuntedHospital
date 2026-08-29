using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GameStart()
    {   
        SceneManager.LoadScene("MainHospitalScene");
    }
}
