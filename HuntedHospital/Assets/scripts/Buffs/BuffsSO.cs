using UnityEngine;

[CreateAssetMenu(fileName = "BuffsSO", menuName = "Buffs/BuffsSO", order = 1)]
public class BuffsSO : ScriptableObject
{
    [Header("Buff Info")]
    public string buffName;
    public string buffDescription;
    public int buffTier = 1;
    public int buffBaseTime;
    public int buffCurrentTime;
    public int buffCost;
    public enum BuffTypes
    {
        FeastSpeedBuff,
        BloodReceivedBuff,
        PatientCountBuff,
        NoPatientPenalty,
    }

    public void OnEnable()
    {
        RecalculateStats();
    }

    public virtual void RecalculateStats()
    {
        buffBaseTime = buffTier + 2;
        buffCost = buffTier * 2;
    }

    public virtual void ModifyStats() 
    {
        RoundController.Instance.currentGhostCount -= buffCost;
    }
    public virtual void UnmodifyStats() { }

}
