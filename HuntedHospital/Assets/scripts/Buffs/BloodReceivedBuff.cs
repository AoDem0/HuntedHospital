using UnityEngine;

[CreateAssetMenu(fileName = "Blood_received_Buff", menuName = "Buffs/Blood_received_Buff")]
public class BloodReceivedBuff : BuffsSO
{
    public float additionalBloodReceived;

    public override void RecalculateStats()
    {
        base.RecalculateStats();
        additionalBloodReceived = buffTier * 0.1f;
    }

    public override void ModifyStats()
    {
        base.ModifyStats();
        RoundController.Instance.bloodReceivedMultiplier += additionalBloodReceived;
    }

    public override void UnmodifyStats()
    {
       RoundController.Instance.bloodReceivedMultiplier -= additionalBloodReceived;
    }
}
