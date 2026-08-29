using UnityEngine;

[CreateAssetMenu(fileName = "PC_Famous", menuName = "Patient/Characteristics/Famous")]
public class PC_Famous : PatientCharacteristicsSO
{
    public float bloodMultiplier = 1.3f;
    public NoPatientPenalty NoPatientPenalty;

    public void OnEnable()
    {
        CharName = "Star";
        CharDescription = "This patient is famous. If you take their blood, you'll get a bonus to received blood from this patient. People like famous people. However, if you kill them, tomorrow twice as few patients will come to the hospital.";
    }

    public override void ApplyOnKillGlobal()
    {
        BuffManager.Instance.AddBuffToList(NoPatientPenalty);
    }

    public override void ApplyOnBloodDrainPersonal(PatientScript patient)
    {
        patient.bloodAmmount *= bloodMultiplier;

    }
}
