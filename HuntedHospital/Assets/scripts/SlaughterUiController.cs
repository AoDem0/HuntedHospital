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
    public GameObject Buttons;
    bool isCharActive = true;
    bool isButtonsActive = true;

    void Awake()
    {
        if (isCharActive)
        {
            ToggleCharInfo();
        }

        if (isButtonsActive)
        {
            ToggleButtons();
        }
    }

    void Update()
    {
        bloodDisplay.text = RoundController.Instance.bloodInBank.ToString();
        ghostCountDisplay.text = RoundController.Instance.currentGhostCount.ToString();
    }

    public void ToggleCharInfo()
    {
        isCharActive = charName.gameObject.activeSelf;
        charName.gameObject.SetActive(!isCharActive);
        charDescription.gameObject.SetActive(!isCharActive);
    }

    public void DisplayPatientInfo(PatientScript patient)
    {
        charName.text = patient.currentChar.CharName;
        charDescription.text = patient.currentChar.CharDescription;
    }

    public void ToggleButtons()
    {
        isButtonsActive = Buttons.activeSelf;
        Buttons.SetActive(!isButtonsActive);
    }
}
