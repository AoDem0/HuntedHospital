using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class RoundController : MonoBehaviour
{
    public static RoundController Instance { get; private set; }

    public int currentDay;
    public float baseFeastSpeed = 1;
    public float bloodFromToday = 0;

    [Header("Obiekty")]
    public GameObject patientPrefab;
    public GameObject ghostPrefab;
    public GameObject UI;
    [SerializeField] private TextMeshProUGUI dayNumDisplay;

    [Header("Listy")]
    [SerializeField] private List<PatientScript> patientList = new List<PatientScript>();
    [SerializeField] private List<GhostScript> ghostList = new List<GhostScript>();
    public List<BuffsSO> activeBuffs = new List<BuffsSO>();
    public enum RoundPhase
    {
        DayStartPhase,
        FeastPhase,
        DealPhase
    }

    private int patientsToSpawn;
    private int ghostsToSpawn;
    public Vector2 spawnPoint;
    public Vector2 waitingRoom = new Vector2(100, 100);

    [Header(("Fazy rundy"))]
    public RoundPhase roundPhase = RoundPhase.DayStartPhase;
    public bool canStartNewDayPhase = false;
    public bool canStartFeastPhase = false;
    public bool canStartDealPhase = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log($"Duplikat singletonu ID {GetInstanceID()}, Instance ID {Instance.GetInstanceID()} został zniszczony!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Instance.gameObject.SetActive(true);

        if (dayNumDisplay == null)
        {
            dayNumDisplay = UI.transform.Find("DayNumDisplay").GetComponent<TextMeshProUGUI>();
        }    
    }

    private void Update()
    {
        DisplayDay();
        RoundManager();

        Debug.Log($"Current Day: {currentDay}, Current Phase: {roundPhase}, Patients: {patientList.Count}, Ghosts: {ghostList.Count}, Blood Today: {bloodFromToday}");
    }

    #region ------ RoundHandler ------

    private void RoundManager()
    {
        if (canStartNewDayPhase)
        {
            StartDayPhase();
            canStartNewDayPhase = false;
        }

        if (canStartFeastPhase)
        {
            StartFeastPhase();
            canStartFeastPhase = false;
        }

        if (canStartDealPhase)
        {
            StartDealPhase();
            canStartDealPhase = false;
        }
    }

    public void NextRound()
    {
        currentDay++;
    }

    public void StartDayPhase()
    {
        Vector3 exactSpawnPoint = new Vector3(spawnPoint.x, spawnPoint.y, 0);
        for (int i = 0; i < patientsToSpawn; i++)
        {
            GameObject newPatient = Instantiate(patientPrefab, exactSpawnPoint, Quaternion.identity);
        }

        if (patientList.Count == patientsToSpawn)
        {
            EndDayPhase();
        }
    }

    public void EndDayPhase()
    {
        canStartFeastPhase = true;
    }

    public void StartFeastPhase()
    {
        float feastSpeed = baseFeastSpeed;

        StartCoroutine(Feasting(feastSpeed));
    }

    private IEnumerator Feasting(float time)
    {
        List<PatientScript> patientsToRemove = new List<PatientScript>(patientList);
        foreach (var patient in patientList)
        {
            bloodFromToday += patient.bloodAmmount;
            patientsToRemove.Add(patient);

            GameObject newGhost = Instantiate(ghostPrefab, Vector3.zero, Quaternion.identity);
            //te duszki mogą sobie latać po ekranie 
            ghostList.Add(newGhost.GetComponent<GhostScript>());

            yield return new WaitForSeconds(time);

        }

        foreach(var patient in patientsToRemove)
        {
            patientList.Remove(patient);
            Destroy(patient);

        }
        EndFeastPhase();
    }

    public void EndFeastPhase()
    {
        canStartDealPhase = true;
    }

    public void StartDealPhase()
    {
        
    }

    public void EndDealPhase()
    {
        canStartNewDayPhase = true;
        NextRound();
    }
    #endregion ------------------------------------------

    public void DisplayDay()
    {
        dayNumDisplay.text = currentDay.ToString();
    }
    public void PatientEnteredHospital(PatientScript patient)
    {
        patientList.Add(patient);
        patient.transform.position = new Vector3(waitingRoom.x + patientList.Count, waitingRoom.y, 0);
    }
}
