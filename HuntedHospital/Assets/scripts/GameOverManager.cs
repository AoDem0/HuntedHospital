using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Displays")]
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI totalBloodDisplay;
    public TextMeshProUGUI totalPatientsDisplay;
    public TextMeshProUGUI killedPatientsDisplay;

    [Header("Values to display")]
    public float totalBlood;
    public int totalPatients;
    public int killedPatients;
    private RoundController RC;

    void Awake()
    {
        RC = RoundController.Instance;
        totalBlood = Mathf.Round(RC.totalBlood * 100f) / 100f;
        totalPatients = RC.totalPatientsSpawned;
        killedPatients = RC.totalPatientsKilled;
        GetAllDisplays();
        DisplayEndgameStats(RC.verdict);
    }

    public void DisplayEndgameStats(RoundController.Verdict verdict)
    {
        if (verdict == RoundController.Verdict.Win)
        {
            gameOverText.text = "Victory!";
        }
        else if (verdict == RoundController.Verdict.Lose)
        {
            gameOverText.text = "Defeat";
        }
        totalBloodDisplay.text = $"Total blood collected: {totalBlood}L";
        totalPatientsDisplay.text = $"Total patients visited: {totalPatients}";
        killedPatientsDisplay.text = $"Total patients killed: {killedPatients}";
    }

    private void GetAllDisplays()
    {
        if(gameOverText == null || totalBloodDisplay == null || totalPatientsDisplay == null || killedPatientsDisplay == null)
        {
            gameOverText = GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>();
            totalBloodDisplay =  GameObject.Find("TotalBloodDisplay").GetComponent<TextMeshProUGUI>();
            totalPatientsDisplay = GameObject.Find("TotalPatientsDisplay").GetComponent<TextMeshProUGUI>();
            killedPatientsDisplay = GameObject.Find("TotalKilledPatientsDisplay").GetComponent<TextMeshProUGUI>();            
        }
    }

    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
