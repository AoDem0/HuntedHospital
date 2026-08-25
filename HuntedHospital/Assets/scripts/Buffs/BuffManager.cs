using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }
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
                Destroy(buff);
            }
        }
    }

    public void AddBuffToList(BuffsSO buff)
    {
        BuffsSO newBuff = Instantiate(buff);
        RoundController.Instance.activeBuffs.Add(newBuff);
        newBuff.ModifyStats();
    }
    
}