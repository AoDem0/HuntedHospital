using UnityEngine;

[CreateAssetMenu(fileName = "PC_Weak", menuName = "Patient/Characteristics/Weak")]
public class PC_Weak : PatientCharacteristicsSO
{
    public float bloodMultiplier = 0.7f;
    public void OnEnable()
    {
        CharName = "Weak";
        CharDescription = "This patient is weak, has a smaller amount of blood, and sometimes gets depressed.";
    }

    public override void ApplyCharacteristics(PatientScript patient)
    {
        patient.bloodAmmount *= bloodMultiplier;
    }
}
