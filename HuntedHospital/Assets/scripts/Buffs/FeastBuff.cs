using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Feast_Buff", menuName = "Buffs/Feast_Buff")]
public class FeastBuff : BuffsSO
{
    public float FeastSpeedBuff;
    public BuffTypes buffType = BuffTypes.FeastSpeedBuff;
    public override void RecalculateStats()
    {
        base.RecalculateStats();
        FeastSpeedBuff = buffTier * 0.1f;
    }

    public override void ModifyStats()
    {
        base.ModifyStats();
        RoundController.Instance.feastSpeedMultiplier += FeastSpeedBuff;
    }

    public override void UnmodifyStats()
    {
        RoundController.Instance.feastSpeedMultiplier -= FeastSpeedBuff;
    }
}
