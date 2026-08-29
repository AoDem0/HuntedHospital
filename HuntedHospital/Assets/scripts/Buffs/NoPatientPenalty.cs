using UnityEngine;

[CreateAssetMenu(fileName = "NoPatientPenalty", menuName = "Buffs/NoPatientPenalty")]
public class NoPatientPenalty : BuffsSO
{
    public float PatientSpawnMultiplier = 0.5f;
    public BuffTypes buffType = BuffTypes.NoPatientPenalty;

    public override void RecalculateStats()
    {
        buffBaseTime = 2;
        buffCost = 0;
    }

    public override void ModifyStats()
    {
        RoundController.Instance.patientSpawnMultiplier *= PatientSpawnMultiplier;
    }

    public override void UnmodifyStats()
    {
        RoundController.Instance.patientSpawnMultiplier /= PatientSpawnMultiplier;
    }


}
