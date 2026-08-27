using UnityEngine;

[CreateAssetMenu(fileName = "PC_Famous", menuName = "Patient/Characteristics/Famous")]
public class PC_Famous : PatientCharacteristicsSO
{
    public float bloodMultiplier = 1.3f;
    public NoPatientPenalty NoPatientPenalty;

    public void OnEnable()
    {
        CharName = "Sławny";
        CharDescription = "Ten pacjent jest sławny. Jeśli weźmiesz jego krew, dostaniesz bonus do dziennego zbioru krwi. Ludzie lubią sławnych ludzi. Jeśli zaś go zabijesz, jutro nikt nie przyjdzie do szpitala.";
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
