using UnityEngine;

[CreateAssetMenu(fileName = "PC_Sick", menuName = "Patient/Characteristics/Sick")]
public class PC_Sick : PatientCharacteristicsSO
{
    public void OnEnable()
    {
        CharName = "Chory";
        CharDescription = "Ten pacjent jest chory. Jeśli weźmiesz jego krew, cały dotychczasowy zbiór dzienny pójdzie... do śmietnika. Nikt nie lubi chorej krwi. Fuj";
    }

    public override void ApplyGlobalDebuffs()
    {
        RoundController.Instance.bloodInBank = 0;
    } 
}
