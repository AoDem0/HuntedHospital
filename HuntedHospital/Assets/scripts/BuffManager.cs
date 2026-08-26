using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }
    Dictionary<BuffsSO, GameObject> activeBuffsDic = new Dictionary<BuffsSO, GameObject>();
    private float baseFeastTime;
    private void Start()
    {
        baseFeastTime = RoundController.Instance.baseFeastSpeed;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void DecreaseBuffTimeWithRound()
    {
        var buffs = RoundController.Instance.activeBuffs;
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            var buff = buffs[i];
            buff.buffCurrentTime -= 1;
            if (buff.buffCurrentTime <= 0)
            {
                buff.UnmodifyStats();
                buffs.RemoveAt(i);

                if(activeBuffsDic.TryGetValue(buff, out GameObject tile))
                {
                    Destroy(tile);
                    activeBuffsDic.Remove(buff);
                }
                Destroy(buff);
            }
        }
        uiController.Instance.DisplayBuffValues(activeBuffsDic);
    }

    public void AddBuffToList(BuffsSO buff)
    {
        BuffsSO newBuff = Instantiate(buff);
        RoundController.Instance.activeBuffs.Add(newBuff);
        newBuff.ModifyStats();
        newBuff.buffCurrentTime = newBuff.buffBaseTime;

        var UI = uiController.Instance;
        GameObject newBuffTile = Instantiate(UI.buffTile, UI.buffListContent.transform);
        activeBuffsDic.Add(newBuff, newBuffTile);

        UI.DisplayBuffValues(activeBuffsDic);
    }
    
}