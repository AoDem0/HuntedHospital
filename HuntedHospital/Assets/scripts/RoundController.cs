using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class RoundController : MonoBehaviour
{
    public static RoundController Instance { get; private set; }

    public List<GameObject> spawnPoints = new List<GameObject>();
    public int currentDay;
    public float baseFeastSpeed = 1;
    public float bloodFromToday = 0;

    [Header("Obiekty")]
    public GameObject patientPrefab;
    public GameObject ghostPrefab;
    public GameObject UI;
    public GameObject HospitalDoors;
    [SerializeField] private TextMeshProUGUI dayNumDisplay;
    [SerializeField] private TextMeshProUGUI patientNumDisplay;

    [Header("Listy")]
    [SerializeField] private List<PatientScript> patientList = new List<PatientScript>();
    [SerializeField] private List<GhostScript> ghostList = new List<GhostScript>();
    public List<BuffsSO> activeBuffs = new List<BuffsSO>();
    public enum RoundPhases
    {
        DayStartPhase,
        FeastPhase,
        DealPhase
    }

    private int patientsToSpawn = 5;
    private int ghostsToSpawn;
    public Vector2 spawnPoint;
    public Vector2 waitingRoom = new Vector2(100, 100);

    [Header(("Fazy rundy"))]
    public RoundPhases roundPhase = RoundPhases.DayStartPhase;
    public bool canStartNewDayPhase = true;
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
        if (dayNumDisplay == null)
        {
            dayNumDisplay = UI.transform.Find("DayNumDisplay").GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        DisplayValues();
        RoundManager();

        Debug.Log($"Current Day: {currentDay}, Current Phase: {roundPhase}, Patients: {patientList.Count}, Ghosts: {ghostList.Count}, Blood Today: {bloodFromToday}");
    }

    #region ------ RoundHandler ------

    private void RoundManager()
    {
        if (canStartNewDayPhase)
        {
            canStartNewDayPhase = false;
            StartDayPhase();
            Debug.Log("Day phase started");
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
        roundPhase = RoundPhases.DayStartPhase;  
        StartCoroutine(SpawnPatients(0.5f)); 

        if (patientList.Count == patientsToSpawn)
        {
            EndDayPhase();
        }
    }

    public void EndDayPhase()
    {
        canStartNewDayPhase = false;
        canStartFeastPhase = true;
    }

    public void StartFeastPhase()
    {
        roundPhase = RoundPhases.FeastPhase;
        float feastSpeed = baseFeastSpeed;

        StartCoroutine(Feasting(feastSpeed));
    }

    private IEnumerator SpawnPatients(float time)
    {
        for (int i = 0; i < patientsToSpawn; i++)
        {
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject newPatient = Instantiate(patientPrefab, spawnPoint.transform.position, Quaternion.identity);
            newPatient.GetComponent<PatientScript>().HospitalDoors = HospitalDoors.transform.position;

            yield return new WaitForSeconds(time);
        }
        canStartNewDayPhase = false;

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
        roundPhase = RoundPhases.DealPhase;
    }

    public void EndDealPhase()
    {
        canStartNewDayPhase = true;
        NextRound();
    }
    #endregion ------------------------------------------

    public void DisplayValues()
    {
        dayNumDisplay.text = currentDay.ToString();
        patientNumDisplay.text = patientList.Count.ToString();
    }
    public void PatientEnteredHospital(PatientScript patient)
    {
        patientList.Add(patient);
        patient.transform.position = new Vector3(waitingRoom.x + patientList.Count, waitingRoom.y, 0);
        patient.movedToWaitingRoom = true;
    }
}
