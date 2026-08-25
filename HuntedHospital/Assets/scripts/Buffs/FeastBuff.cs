using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Feast_Buff", menuName = "Buffs/Feast_Buff")]
public class FeastBuff : BuffsSO
{
    public float FeastSpeedBuff = buffTier *  0.1f;

    public float buffTime = buffTier + 2;
    public int buffCost = buffTier * 5;
    public BuffTypes buffType = BuffTypes.FeastSpeedBuff;

    public virtual void ModifyFeastSpeed() { }
}
