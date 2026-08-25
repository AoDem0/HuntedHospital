using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ShopController : MonoBehaviour
{
    public List<DeathDealPanel> dealPanels = new List<DeathDealPanel>();
    public List<BuffsSO> allBuffs = new List<BuffsSO>();
    public List<BuffsSO> buffsToUse = new List<BuffsSO>();
    public List<int> buffsInUse = new List<int>();

    void Awake()
    {
        if (dealPanels.Count == 0)
        {
            Debug.Log("Not all panels were found");
        }
        CreateRandomBuffs(allBuffs);
    }

    private void CreateRandomBuffs(List<BuffsSO> allBuffs)
    {
        buffsToUse = allBuffs;
        foreach (var panel in dealPanels)
        {
            BuffsSO buff = GetRandomBuff(buffsToUse);
            float randomNum = Random.Range(0, 10)/3;
            int tier;

            if(randomNum < 2.5)
            {
                tier = 1;
            }
            else if(randomNum > 2.5 && randomNum < 3)
            {
                tier = 2;
            }
            else
            {
                tier = 3;
            }
            buff.buffTier =  tier;

            panel.SetBuffOnDealPanel(buff);

        }
    }

    private BuffsSO GetRandomBuff(List<BuffsSO> buffsToUse)
    {
        int randomBuffIndex = Random.Range(0, buffsToUse.Count);
        var buff = buffsToUse[randomBuffIndex];
        buffsToUse.Remove(buff);
        return buff;
    }


}
