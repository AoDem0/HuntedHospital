using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeathDealPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Deal_Name;
    [SerializeField] private TextMeshProUGUI Deal_Description;
    [SerializeField] private TextMeshProUGUI Deal_Cost;
    [SerializeField] private TextMeshProUGUI Deal_Tier;
    [SerializeField] private Button button;
    [SerializeField] public BuffsSO buff;
    public ColorForTierScript colorForTier;
    public ColorBlock colorsToSet;
    public ShopController SC;
    [SerializeField]private soundManager soundMan;

    public void SetBuffOnDealPanel(BuffsSO buff)
    {

        this.buff = buff;
        Deal_Name.text = buff.buffName;
        Deal_Description.text = buff.buffDescription;
        Deal_Cost.text = $"Koszt dusz: {buff.buffCost}";
        Deal_Tier.text = $"Tier: {buff.buffTier}";
        var colorToSet = colorForTier.GetColorForTier(buff);

        colorsToSet = button.colors;
        var choosedColor = colorForTier.GetColorForTier(buff);
        colorsToSet.normalColor = choosedColor;
        colorsToSet.highlightedColor = choosedColor * 1.2f;
        colorsToSet.pressedColor = choosedColor * 0.8f;
        colorsToSet.selectedColor = choosedColor * 0.6f;
        button.colors = colorsToSet;
    }

    private void Awake()
    {
        SC = GameObject.Find("ShopController").GetComponent<ShopController>();
        soundMan = FindAnyObjectByType<soundManager>();
    }

    public void ChooseThisBuff()
    {
        if (RoundController.Instance.currentGhostCount >= buff.buffCost)
        {
            if (buff != null)
            {
                soundMan.Play("buffclick");
                RoundController.Instance.buffManager.AddBuffToList(buff);
            }
            RoundController.Instance.EndDealPhase();
        }
        else if(RoundController.Instance.currentGhostCount < buff.buffCost)
        {
            SC.TriggerNotEnoguhGhosts();
        }

    }
}
