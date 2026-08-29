using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager instance { get; private set; }
    public int threshold1 = 4;
    public int threshold2 = 6;
    public float patientsDebuffMultiplier = 1f;
    private float thresholdDebuff = 0.25f;
    private int GhostCount;
    private RoundController RC;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        RC = RoundController.Instance;
    }

    public void Update()
    {
        RefreshValues();
    }

    public void RefreshValues()
    {
        if (RC == null)
        {
            RC = RoundController.Instance;
            if (RC != null)
            {
\                return;
            }
        }
        GhostCount = RC.currentGhostCount;

        if (GhostCount > threshold1 && GhostCount < threshold2)
        {
            patientsDebuffMultiplier = 1f - thresholdDebuff;
        }
        else if (GhostCount > threshold2)
        {
            patientsDebuffMultiplier = 1f - 2 * thresholdDebuff;
        }
        else
        {
            patientsDebuffMultiplier = 1;
        }
        //Tak, da się to zrobić lepiej, ale nie umiem i czas mnie goni ~Vegot xD
    }

}
