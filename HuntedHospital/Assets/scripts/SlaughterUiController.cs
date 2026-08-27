using System;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlaughterUiController : MonoBehaviour
{
    public TextMeshProUGUI charName;
    public TextMeshProUGUI charDescription;
    public TextMeshProUGUI bloodDisplay;
    public TextMeshProUGUI ghostCountDisplay;
    bool isActive = false;

    void Awake()
    {
        if (isActive)
        {
            ToggleCharInfo();
        }
    }

    void Update()
    {
        bloodDisplay.text = RoundController.Instance.bloodInBank.ToString();
        ghostCountDisplay.text = RoundController.Instance.currentGhostCount.ToString();
    }

    public void ToggleCharInfo()
    {
        isActive = charName.gameObject.activeSelf;
        charName.gameObject.SetActive(!isActive);
        charDescription.gameObject.SetActive(!isActive);
    }

    public void DisplayPatientInfo(PatientScript patient)
    {
        charName.text = patient.currentChar.CharName;
        charDescription.text = patient.currentChar.CharDescription;
    }

}
