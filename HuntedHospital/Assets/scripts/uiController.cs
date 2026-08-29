using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uiController : MonoBehaviour
{
    public static uiController Instance { get; private set; }
    [Header("UI")]
     public TextMeshProUGUI dayNumDisplay;
    public TextMeshProUGUI patientNumDisplay;
    public TextMeshProUGUI ghostNumDisplay;
    public TextMeshProUGUI bloodAmmountDisplay;
    public TextMeshProUGUI hungerDisplay;
    public GameObject buffListPanel;
    public GameObject buffListContent;
    public GameObject buffTile;
    public ColorForTierScript colorForTier;
    [SerializeField]private soundManager soundMan;

    RoundController RC;
    public bool isActive;

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
        buffListPanel.SetActive(false);
        soundMan = FindAnyObjectByType<soundManager>();
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
        bloodAmmountDisplay.text = $"{ (Mathf.Round(RC.bloodInBank * 100)) / 100.0}L";
        hungerDisplay.text = RC.hunger.ToString();
    }

    public void ToggleBuffList()
    {
        if (buffListPanel != null)
        {
            soundMan.Play("uiclick");
            buffListPanel.SetActive(!buffListPanel.activeSelf);
        }
    }

    public void DisplayBuffValues(Dictionary<BuffsSO, GameObject> dictionary)
    {
        foreach (var position in dictionary)
        {
            BuffsSO buff = position.Key;
            GameObject tile = position.Value;

            buffTileScript buffTile = tile.GetComponent<buffTileScript>();
            if (buffTile != null)
            {
                buffTile.SetValues(buff);
            }

            buffTile.GetComponent<Image>().color = colorForTier.GetColorForTier(buff);
        }
    }
    public void ToggleUI()
    {
        isActive = patientNumDisplay.gameObject.activeSelf;
        patientNumDisplay.gameObject.SetActive(!isActive);
        ghostNumDisplay.gameObject.SetActive(!isActive);
        bloodAmmountDisplay.gameObject.SetActive(!isActive);
    }
}
