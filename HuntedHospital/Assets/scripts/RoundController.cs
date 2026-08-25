using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class RoundController : MonoBehaviour
{
    public static RoundController Instance { get; private set; }
    public BuffManager buffManager { get; private set; }

    public List<GameObject> spawnPoints = new List<GameObject>();
    public int currentDay;
    public float baseFeastSpeed = 1;
    public float bloodInBank = 0;

    [Header("Obiekty")]
    public GameObject patientPrefab;
    public GameObject ghostPrefab;
    public uiController UI;
    public GameObject HospitalDoors;

    [Header("Listy")]
    [SerializeField] private List<PatientScript> patientList = new List<PatientScript>();
    [SerializeField] public List<GhostScript> ghostList = new List<GhostScript>();
    public List<BuffsSO> activeBuffs = new List<BuffsSO>();
    public enum RoundPhases
    {
        DayStartPhase,
        FeastPhase,
        DealPhase
    }

    public int patientsToSpawn = 5;
    public int patientsInHospital = 0;
    public int currentGhostCount;
    public Vector2 spawnPoint;
    public Vector2 waitingRoom = new Vector2(100, 100);

    [Header("Stat Multipliers")]
    public float bloodReceivedMultiplier = 1f;
    public float feastSpeedMultiplier = 1f;
    public float patientCountMultiplier = 1f;

    [Header(("Fazy rundy"))]
    public RoundPhases roundPhase = RoundPhases.DayStartPhase;
    public bool canStartNewDayPhase = true;
    public bool canEndNewDayPhase = false;
    public bool canStartFeastPhase = false;
    public bool canStartDealPhase = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            //Debug.Log($"Duplikat singletonu ID {GetInstanceID()}, Instance ID {Instance.GetInstanceID()} został zniszczony!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        buffManager = GetComponent<BuffManager>();
        RefreshSceneObjects();

    }

    private void Update()
    {
        UI.DisplayValues();
        RoundManager();

        //Debug.Log($"Current Day: {currentDay}, Current Phase: {roundPhase}, Patients: {patientList.Count}, Ghosts: {ghostList.Count}, Blood Today: {bloodInBank}");
    }

    #region ------ RoundHandler ------

    private void RoundManager()
    {
        if (canStartNewDayPhase)
        {
            canStartNewDayPhase = false;
            StartDayPhase();
            //Debug.Log("Day phase started");
        }

        if (patientList.Count == patientsToSpawn && canEndNewDayPhase)
        {
            EndDayPhase();
            
        }

        if (canStartFeastPhase)
        {
            StartFeastPhase();
            canStartFeastPhase = false;
            //Debug.Log("Feast phase started");   
        }

        if (canStartDealPhase)
        {
            StartDealPhase();
            canStartDealPhase = false;
            //Debug.Log("Deal phase started");
        }
    }

    public void NextRound()
    {
        currentDay += 1;
        buffManager.DecreaseBuffTimeWithRound();
    }

    public void StartDayPhase()
    {
        Debug.Log($"StartDayPhase() wywołany. Wywołany przez: \n{System.Environment.StackTrace}");
        NextRound();
        roundPhase = RoundPhases.DayStartPhase;  
        StartCoroutine(SpawnPatients(0.5f));  
    }

    public void EndDayPhase()
    {
        canEndNewDayPhase = false;
        canStartNewDayPhase = false;
        canStartFeastPhase = true;
    }

    public void StartFeastPhase()
    {
        roundPhase = RoundPhases.FeastPhase;
        float feastSpeed = baseFeastSpeed * feastSpeedMultiplier;
        if(feastSpeedMultiplier < 1)
        {
            Debug.Log("SpeedMultiplier is lower than 1");
        }
        canStartFeastPhase = false;

        StartCoroutine(Feasting(feastSpeed));
    }

    public void EndFeastPhase()
    {
        canStartDealPhase = true;
    }

    public void StartDealPhase()
    {
        roundPhase = RoundPhases.DealPhase;
        SceneManager.LoadScene("deathshop");
    }

    public void EndDealPhase()
    {
        SceneManager.LoadScene("MainHospitalScene");
    }
    #endregion ------------------------------------------

    private IEnumerator SpawnPatients(float time)
    {
        for (int i = 0; i < patientsToSpawn; i++)
        {
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject newPatient = Instantiate(patientPrefab, spawnPoint.transform.position, Quaternion.identity);
            newPatient.GetComponent<PatientScript>().HospitalDoors = HospitalDoors.transform.position;

            yield return new WaitForSeconds(time);
        }
        canEndNewDayPhase = true;
    }

    private IEnumerator Feasting(float time)
    {
        List<PatientScript> patientsToRemove = new List<PatientScript>(patientList);
        foreach (var patient in patientList)
        {
            bloodInBank += (patient.bloodAmmount * bloodReceivedMultiplier);
            patientsInHospital -= 1;
            Destroy(patient);

            currentGhostCount++;

            yield return new WaitForSeconds(time);
        }
        patientList.Clear();
        EndFeastPhase();
    }

    
    public void PatientEnteredHospital(PatientScript patient)
    {
        patientList.Add(patient);
        patientsInHospital += 1;
        patient.transform.position = new Vector3(waitingRoom.x + patientList.Count, waitingRoom.y, 0);
        patient.movedToWaitingRoom = true;
    }

    #region ------ SCENE RELOAD ------
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainHospitalScene")
        {
            RefreshSceneObjects();
            canStartNewDayPhase = true;
        }
    }

    private void RefreshSceneObjects()
    {
        UI = uiController.Instance;
        HospitalDoors = GameObject.Find("Door");
        var spawnPointsInScene = GameObject.Find("SpawnPoints");
        spawnPoints.Clear();
        foreach (Transform child in spawnPointsInScene.transform)
        {
            spawnPoints.Add(child.gameObject);
        }
    }
    #endregion
}
