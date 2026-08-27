using UnityEngine;

[CreateAssetMenu(fileName = "Patient_Count_Buff", menuName = "Buffs/Patient_Count_Buff")]
public class PatientCountBuff : BuffsSO
{
    public int extraPatients;
    public BuffTypes buffType = BuffTypes.PatientCountBuff;
     public override void RecalculateStats()
    {
        base.RecalculateStats();
        extraPatients = buffTier * 1;
    }
    public override void ModifyStats()
    {
        base.ModifyStats();
        RoundController.Instance.extraPatientCount += extraPatients;
    }

    public override void UnmodifyStats()
    {
        RoundController.Instance.extraPatientCount -= extraPatients;
    }
}
