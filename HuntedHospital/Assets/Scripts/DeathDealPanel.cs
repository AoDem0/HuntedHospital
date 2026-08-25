using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeathDealPanel : MonoBehaviour
{
    //[SerializeField] private DeathDealSO deathDeal;
    [SerializeField] private TextMeshProUGUI Deal_Name;
    [SerializeField] private TextMeshProUGUI Deal_Description;
    [SerializeField] private TextMeshProUGUI Deal_Cost;
    [SerializeField] private Button button;
    [SerializeField] private BuffsSO buff;

    public void SetBuffOnDealPanel(BuffsSO buff)
    {
        this.buff = buff;
        Deal_Name.text = buff.buffName;
        Deal_Description.text = buff.buffDescription;
        Deal_Cost.text = buff.buffCost.ToString();
    }

    private void Awake()
    {
        
    }

    

    /*public void ReloadPanel(DeathDealSO deal)
    {
        if (text == null || button == null) return;
        button.interactable = true;
        text.text = "";
        for (int i = 0; i < deal.Deals.Count; i++)
        {
            text.text += deal.Deals[i].dealAmount.ToString() + " " + deal.Deals[i].dealType.ToString() + "\n";
        }

        text.text += "Price: " + deal.SoulPrice.ToString();
    }*/

    public void SendDealInfo()
    {
        //wyslanie do gamemanagera danych
        //DeactivateDeathPanel()
    }
    /*public void DeactivateDeathPanel()
    {
        //text.text = "";
        button.interactable = false;

    }*/

    public void ChooseThisBuff()
    {
        if (buff != null)
        {
            RoundController.Instance.buffManager.AddBuffToList(buff);
            Debug.Log($"Buff {buff.buffName} został dodany do aktywnych buffów.");
        }
        else
        {
            Debug.LogWarning("Nie wybrano żadnego buffa.");
        }
        RoundController.Instance.EndDealPhase();

    }
}
