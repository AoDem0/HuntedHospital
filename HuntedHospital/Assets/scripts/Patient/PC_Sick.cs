using UnityEngine;

[CreateAssetMenu(fileName = "PC_Sick", menuName = "Patient/Characteristics/Sick")]
public class PC_Sick : PatientCharacteristicsSO
{
    public void OnEnable()
    {
        CharName = "Sick";
        CharDescription = "This patient is sick. If you take their blood, the entire daily collection will go... to the trash. Nobody likes sick blood. Yuck";
    }

    public override void ApplyGlobalDebuffsAfterTakingBlood()
    {
        RoundController.Instance.bloodInBank = 0;
    } 
}
