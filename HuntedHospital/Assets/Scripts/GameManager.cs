using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float BloodLevel = 0f;
    public float SoulsLevel = 0f;
    public float TimeOfDay;
    public int Days;
    [SerializeField] private List<DeathDealSO> allDeals;
    [SerializeField] private List<DeathDealPanel> deathPanels;

    public void ChangeDeathDeals()
    {
        for (int i = 0; i < deathPanels.Count; i++)
        { 
            int randidx = Random.Range(0, allDeals.Count + 1);
            //deathPanels[i].ReloadPanel(allDeals[randidx]);
        }
        
    }
}
