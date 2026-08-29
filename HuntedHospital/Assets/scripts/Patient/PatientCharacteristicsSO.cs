using UnityEngine;

[CreateAssetMenu(fileName = "PatientCharacteristicsSO", menuName = "Patient/Characteristics/SO", order = 1)]
public class PatientCharacteristicsSO : ScriptableObject
{
    public string CharName { get; set; }
    public string CharDescription { get; set; }
    public virtual void ApplyCharacteristics(PatientScript patient ) { }    
    public virtual void ApplyGlobalDebuffs() { }
    public virtual void ApplyOnKillGlobal() { }
    public virtual void ApplyOnBloodDrainPersonal(PatientScript patient) { }
    public virtual void ApplyGlobalDebuffsAfterTakingBlood() { }
}
