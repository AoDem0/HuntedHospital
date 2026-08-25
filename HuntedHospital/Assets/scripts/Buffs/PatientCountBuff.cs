using UnityEngine;

[CreateAssetMenu(fileName = "Patient_Count_Buff", menuName = "Buffs/Patient_Count_Buff")]
public class PatientCountBuff : BuffsSO
{
    public int extraPatients = buffTier * 1;
    public float buffTime = buffTier + 2;
    public int buffCost = buffTier * 5;
    public BuffTypes buffType = BuffTypes.PatientCountBuff;

    public virtual void ModifyPatientCount()
    {
        RoundController.Instance.patientsToSpawn += extraPatients;
    }
}
