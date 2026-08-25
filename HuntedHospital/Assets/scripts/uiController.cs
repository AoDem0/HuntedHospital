using TMPro;
using UnityEngine;

public class uiController : MonoBehaviour
{
    public static uiController Instance { get; private set; }
    [Header("UI")]
    [SerializeField] public TextMeshProUGUI dayNumDisplay;
    [SerializeField] public TextMeshProUGUI patientNumDisplay;
    [SerializeField] public TextMeshProUGUI ghostNumDisplay;
    [SerializeField] public TextMeshProUGUI bloodAmmountDisplay;
    RoundController RC;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        RC = RoundController.Instance;

    }

    public void DisplayValues()
    {

        if(RC == null)
        {
            RC = RoundController.Instance;
        }

        dayNumDisplay.text = RC.currentDay.ToString();
        patientNumDisplay.text = RC.patientsInHospital.ToString();
        ghostNumDisplay.text = RC.currentGhostCount.ToString();
        bloodAmmountDisplay.text = $"{RC.bloodInBank}L";
    }
}
