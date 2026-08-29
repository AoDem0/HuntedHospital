using UnityEngine;

[CreateAssetMenu(fileName = "PC_None", menuName = "Patient/Characteristics/None")]
public class PC_None : PatientCharacteristicsSO
{
    public void OnEnable()
    {
        CharName = "Average Joe";
        CharDescription = "This patient is an average Joe, a gray mouse, nothing special, as bland as bread. Nobody will even notice that they disappeared.";
    }
}
