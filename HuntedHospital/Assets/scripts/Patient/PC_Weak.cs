using UnityEngine;

[CreateAssetMenu(fileName = "PC_Weak", menuName = "Patient/Characteristics/Weak")]
public class PC_Weak : PatientCharacteristicsSO
{
    public float bloodMultiplier = 0.7f;
    public void OnEnable()
    {
        CharName = "Słaby";
        CharDescription = "Ten pacjent jest słaby, posiada mniejszą ilość krwi i czasem go łapie deprecha";
    }

    public override void ApplyCharacteristics(PatientScript patient)
    {
        patient.bloodAmmount *= bloodMultiplier;
    }
}
