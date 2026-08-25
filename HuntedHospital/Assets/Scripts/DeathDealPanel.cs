using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathDealPanel : MonoBehaviour
{
    //[SerializeField] private DeathDealSO deathDeal;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;

    private void Awake()
    {
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        if(text!=null) Debug.Log("found text");
        if(button!=null) Debug.Log("found button");
    }

    

    public void ReloadPanel(DeathDealSO deal)
    {
        if (text == null || button == null) return;
        button.interactable = true;
        text.text = "";
        for (int i = 0; i < deal.Deals.Count; i++)
        {
            text.text += deal.Deals[i].dealAmount.ToString() + " " + deal.Deals[i].dealType.ToString() + "\n";
        }

        text.text += "Price: " + deal.SoulPrice.ToString();
    }

    public void SendDealInfo()
    {
        //wyslanie do gamemanagera danych
        //DeactivateDeathPanel()
    }
    public void DeactivateDeathPanel()
    {
        text.text = "";
        button.interactable = false;

    }
}
