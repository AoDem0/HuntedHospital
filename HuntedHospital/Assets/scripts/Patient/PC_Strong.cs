using UnityEngine;

[CreateAssetMenu(fileName = "PC_Strong", menuName = "Patient/Characteristics/Strong")]
public class PC_Strong : PatientCharacteristicsSO
{
    public float bloodMultiplier = 1.3f;

    public void OnEnable()
    {
        CharName = "Silny";
        CharDescription = "Ten pacjent jest silny, zdrowy i posiada większą ilość krwi. Ciekawe czy ćwiczy...";
    }

    public override void ApplyCharacteristics(PatientScript patient)
    {
        patient.bloodAmmount *= bloodMultiplier;
    }
}
