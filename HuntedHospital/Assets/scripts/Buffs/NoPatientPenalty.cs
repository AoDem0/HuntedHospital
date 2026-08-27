using UnityEngine;

public class NoPatientPenalty : BuffsSO
{
    public float PatientSpawnMultiplier = 1f;
    public BuffTypes buffType = BuffTypes.NoPatientPenalty;

    public override void RecalculateStats()
    {
        buffBaseTime = 1;
        buffCost = 0;
    }

    public override void ModifyStats()
    {
        RoundController.Instance.patientSpawnMultiplier -= PatientSpawnMultiplier;
    }

    public override void UnmodifyStats()
    {
        RoundController.Instance.patientSpawnMultiplier += PatientSpawnMultiplier;
    }


}
