using UnityEngine;

[CreateAssetMenu(fileName = "BuffsSO", menuName = "Buffs/BuffsSO", order = 1)]
public class BuffsSO : ScriptableObject
{
    public string buffName;
    public static int buffTier;
    public static int buffBaseTime;
    public int buffCurrentTime;
    public enum BuffTypes
    {
        FeastSpeedBuff,
        BloodGainBuff,
        PatientCountBuff,
        GhostSpawnBuff
    }

    

}
