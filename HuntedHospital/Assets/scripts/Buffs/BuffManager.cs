using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }
    private float baseFeastTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        baseFeastTime = RoundController.Instance.baseFeastSpeed;
    }

    private float CountFeastingSpeed(float baseFeastSpeed)
    {
        float modifiedFeastSpeed = baseFeastSpeed;

        foreach (var buff in RoundController.Instance.activeBuffs)
        {
            if (buff is FeastBuff feastBuff)
            {
                modifiedFeastSpeed -= feastBuff.FeastSpeedBuff;
            }
        }
        return modifiedFeastSpeed;
    }

    private void DecreaseBuffTimeWithRound()
    {
        foreach (var buff in RoundController.Instance.activeBuffs)
        {
            buff.buffCurrentTime -= 1;
            if (buff.buffCurrentTime <= 0)
            {
                RoundController.Instance.activeBuffs.Remove(buff);
                Destroy(buff);
            }
        }
    }
    
}