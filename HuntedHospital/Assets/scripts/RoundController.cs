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
    public DebuffManager debuffManager { get; private set; }

    public List<GameObject> spawnPoints = new List<GameObject>();
    public int currentDay;
    public float baseFeastSpeed = 1;
    public float bloodInBank = 0;
    private soundManager soundMan;

    [Header("Obiekty")]
    public GameObject patientPrefab;
    public GameObject ghostPrefab;
    public uiController UI;
    public GameObject HospitalDoors;

    [Header("Listy")]
    [SerializeField] public List<PatientScript> patientList = new List<PatientScript>();
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
    public int extraPatientCount = 0;
    public float patientSpawnMultiplier = 1f;
    public int totalPatientsToSpawn;


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
        soundMan = FindAnyObjectByType<soundManager>();
        Instance = this;
        DontDestroyOnLoad(gameObject);
        buffManager = GetComponent<BuffManager>();
        debuffManager = GetComponent<DebuffManager>();
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

        if (patientList.Count == totalPatientsToSpawn && canEndNewDayPhase)
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
        patientList.Clear();
        buffManager.DecreaseBuffTimeWithRound();
        bloodInBank = 0;
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
        MovePatientsToOtherScene();
        SceneManager.LoadScene("SlaughterRoom");
    }

    public void EndFeastPhase()
    {
        canStartDealPhase = true;
    }

    public void StartDealPhase()
    {
        roundPhase = RoundPhases.DealPhase;
        SceneManager.LoadScene("deathshop");
        soundMan.ChangeMainMusic(1);
    }

    public void EndDealPhase()
    {
        SceneManager.LoadScene("MainHospitalScene");
        soundMan.ChangeMainMusic(0);
    }

    #endregion ------------------------------------------

    private IEnumerator SpawnPatients(float time)
    {
        totalPatientsToSpawn = (patientsToSpawn + extraPatientCount) * (int)patientSpawnMultiplier;
        for (int i = 0; i < totalPatientsToSpawn; i++)
        {
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject newPatient = Instantiate(patientPrefab, spawnPoint.transform.position, Quaternion.identity);
            PatientScript PS = newPatient.GetComponent<PatientScript>();

            if (spawnPoint.name == "SpawnPointLeft")
            {
                PS.SetSpriteForSide(PatientScript.PatientSpawnSide.Left);
            }
            else if (spawnPoint.name == "SpawnPointRight")
            {
                PS.SetSpriteForSide(PatientScript.PatientSpawnSide.Right);
            }

            PS.HospitalDoors = HospitalDoors.transform.position;

            yield return new WaitForSeconds(time);
        }
        canEndNewDayPhase = true;
    }

    private IEnumerator Feasting(float time)
    {
        for (int i = patientList.Count - 1; i >= 0; i--)
        {
            var patient = patientList[i];
            bloodInBank += (patient.bloodAmmount * bloodReceivedMultiplier);
            bloodInBank = Mathf.Round(bloodInBank * 100f) / 100f;
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
        patient.rb.linearVelocity = Vector2.zero;
    }

    public void MovePatientsToOtherScene()
    {
        foreach (var patient in patientList)
        {
            DontDestroyOnLoad(patient);
        }
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
[System.Serializable]
public class PatientInfoToMove
{
    public int currentBlood;
    public SpritePacjentów patientSpritesRight;
    public PatientCharacteristicsSO currentChar;

}