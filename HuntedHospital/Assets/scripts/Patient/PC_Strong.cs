using UnityEngine;

[CreateAssetMenu(fileName = "PC_Strong", menuName = "Patient/Characteristics/Strong")]
public class PC_Strong : PatientCharacteristicsSO
{
    public float bloodMultiplier = 1.3f;

    public void OnEnable()
    {
        CharName = "Strong";
        CharDescription = "This patient is strong, healthy, and has a larger amount of blood. I wonder if they work out...";
    }

    public override void ApplyCharacteristics(PatientScript patient)
    {
        patient.bloodAmmount *= bloodMultiplier;
    }
}
