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

    public float baseFeastSpeed = 1;

    [Header("Statystyki")]
    public int currentDay;
    public float bloodInBank = 0;
    private soundManager soundMan;
    public int hunger;
    public float neededBlood = 12f;
    public int currentGhostCount;
    public int patientsToSpawn = 5;
    public int patientsInHospital = 0;
    public int totalPatientsSpawned = 0;
    public int totalPatientsKilled = 0;
    public float totalBlood = 0;
    public Verdict verdict;

    [Header("Ustawienia")]

    public Vector2 waitingRoom = new Vector2(100, 100);
    public int hungerToGameOver = 3;
    public int maxDaysToWin = 15;

    [Header("Obiekty")]
    public GameObject patientPrefab;
    public GameObject ghostPrefab;
    public uiController UI;
    public GameObject HospitalDoors;

    [Header("Listy")]
    [SerializeField] public List<PatientScript> patientList = new List<PatientScript>();
    [SerializeField] public List<GhostScript> ghostList = new List<GhostScript>();
    public List<BuffsSO> activeBuffs = new List<BuffsSO>();
    public List<GameObject> spawnPoints = new List<GameObject>();
    public enum RoundPhases
    {
        DayStartPhase,
        FeastPhase,
        DealPhase
    }
    public enum Verdict
    {
        None,
        Win,
        Lose,
    }


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

    void Start()
    {
        hunger = 0;
    }

    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
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
    }

    #region ------ RoundHandler ------

    private void RoundManager()
    {
        if (canStartNewDayPhase)
        {
            canStartNewDayPhase = false;
            StartDayPhase();
        }

        if (patientList.Count == totalPatientsToSpawn && canEndNewDayPhase)
        {
            EndDayPhase();            
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
        currentDay += 1;
        patientList.Clear();
        buffManager.DecreaseBuffTimeWithRound();
        
        if(bloodInBank < neededBlood)
        {
            hunger += 1;
        }

        if(hunger >= hungerToGameOver)
        {
            LoseTheGame();
        }

        if(currentDay > maxDaysToWin)
        {
            WinTheGame();
        }
        bloodInBank = 0;
    }

    public void StartDayPhase()
    {
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
        UI.gameObject.SetActive(false);
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
        UI.gameObject.SetActive(true);
    }

    #endregion ------------------------------------------

    private IEnumerator SpawnPatients(float time)
    {
        totalPatientsToSpawn = Mathf.RoundToInt((patientsToSpawn + extraPatientCount) * patientSpawnMultiplier);
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
            totalPatientsSpawned++;

            yield return new WaitForSeconds(time);
        }
        canEndNewDayPhase = true;
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

    #region ------ END GAME LOGIC ------
    private void LoseTheGame()
    {
        verdict = Verdict.Lose;
        SceneManager.LoadScene("GameOverScene", LoadSceneMode.Additive);

    }

    private void WinTheGame()
    {
        verdict = Verdict.Win;   
        SceneManager.LoadScene("GameOverScene", LoadSceneMode.Additive);
    }

    #endregion
}
