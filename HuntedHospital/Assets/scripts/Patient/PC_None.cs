using UnityEngine;

[CreateAssetMenu(fileName = "PC_None", menuName = "Patient/Characteristics/None")]
public class PC_None : PatientCharacteristicsSO
{
    public void OnEnable()
    {
        CharName = "Brak";
        CharDescription = "Ta jednostka to average Joe, szara mysz, nikt specjalny, jałowy jak chleb. Nikt nawet nie zauważy, że zniknął.";
    }
}
