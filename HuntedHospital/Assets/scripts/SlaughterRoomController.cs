using UnityEngine;
using UnityEngine.SceneManagement;

public class SlaughterRoomController : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject endPoint;
    public GameObject chair;
    public GameObject PatientsDump;
    public int currentPatient;
    public int patientsToServe;
    public SlaughterUiController uiController;
    public RoundController RC;
    public Vector3 moveTarget;
    private bool slaughterEnded = false;
    [SerializeField]private soundManager soundMan;

    void Awake()
    {
        RefreshSceneObjects();
        DragPatientsToScene();
        currentPatient = 0;
        moveTarget = Vector3.zero;
        SpawnNextPatientInRoom();
        patientsToServe = RC.patientList.Count;
        slaughterEnded = false;
        soundMan = FindAnyObjectByType<soundManager>();
    }

    void Update()
    {
        if (patientsToServe <= 0 && !slaughterEnded)
        {
            slaughterEnded = true;
            RC.EndFeastPhase();
            return;
        }

        var patient = RC.patientList[currentPatient];

        if (moveTarget != Vector3.zero)
        {
            patient.MoveToTarget(moveTarget);
            float distance = moveTarget.x - patient.transform.position.x;

            if (Mathf.Abs(distance) < 0.05f && moveTarget != Vector3.zero && !patient.canGoToExit)
            {
                moveTarget = Vector3.zero;
                uiController.ToggleCharInfo();
                uiController.ToggleButtons();
                uiController.DisplayPatientInfo(patient);
            }
            else if (patient.canGoToExit)
            {
                moveTarget = endPoint.transform.position;

                float distanceToEnd = moveTarget.x - patient.transform.position.x;
                if (Mathf.Abs(distanceToEnd) < 0.05f)
                {
                    ClearPatient();
                }
            }
        }
    }

    private void SpawnNextPatientInRoom()
    {
        var patient = RC.patientList[currentPatient];
        patient.spriteRenderer.sprite = patient.currentSpriteSet.wPrawo;
        patient.transform.position = spawnPoint.transform.position;
        patient.transform.parent = PatientsDump.transform;
        patient.SetSpriteForSide(PatientScript.PatientSpawnSide.Left);
        patient.canGoToExit = false;
        moveTarget = chair.transform.position;
        patient.moveSpeed += 2f;
    }

    public void KillPatient()
    {
        soundMan.Play("kill");
        var patient = RC.patientList[currentPatient];
        patient.currentChar.ApplyGlobalDebuffs();
        patient.currentChar.ApplyOnKillGlobal();
        var bloodToAdd = (patient.bloodAmmount * RC.bloodReceivedMultiplier);
        bloodToAdd = Mathf.Round(bloodToAdd * 100f) / 100f;
        RC.bloodInBank += bloodToAdd;
        RC.totalBlood += bloodToAdd;
        patient.currentChar.ApplyGlobalDebuffsAfterTakingBlood();
        RC.currentGhostCount++;
        RC.extraPatientCount--;
        ClearPatient();
        uiController.ToggleCharInfo();
        uiController.ToggleButtons();
        RC.totalPatientsKilled++;
    }

    public void DrainBloodFromPatient()
    {
        soundMan.Play("drain");
        var patient = RC.patientList[currentPatient];
        patient.currentChar.ApplyGlobalDebuffs();
        patient.currentChar.ApplyOnBloodDrainPersonal(patient);
        var bloodToAdd = (patient.bloodAmmount * 0.4f) * RC.bloodReceivedMultiplier;
        bloodToAdd = Mathf.Round(bloodToAdd * 100f) / 100f;
        RC.bloodInBank += bloodToAdd;
        RC.totalBlood += bloodToAdd;
        patient.currentChar.ApplyGlobalDebuffsAfterTakingBlood();
        patient.canGoToExit = true;
        moveTarget = endPoint.transform.position;
        uiController.ToggleCharInfo();
        uiController.ToggleButtons();
    }

    public void LetPatientGo()
    {
        soundMan.Play("letgo");
        var patient = RC.patientList[currentPatient];
        patient.canGoToExit = true;
        patient.canGoToExit = true;
        moveTarget = endPoint.transform.position;
        uiController.ToggleCharInfo();
        uiController.ToggleButtons();
    }

    private void ClearPatient()
    {
        RC.patientsInHospital -= 1;
        Destroy(RC.patientList[currentPatient].gameObject);
        currentPatient++;
        patientsToServe--;
        moveTarget = Vector3.zero;
        if (patientsToServe > 0)
        {
            SpawnNextPatientInRoom();
        }
    }
    
    public void DragPatientsToScene()
    {
        foreach (PatientScript patient in RC.patientList)
        {
            SceneManager.MoveGameObjectToScene(patient.gameObject, SceneManager.GetActiveScene());
        }
    }

    #region ------ SCENE RELOAD -------
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
        if (scene.name == "SlaughterRoom")
        {
            RefreshSceneObjects();
        }
    }

    private void RefreshSceneObjects()
    {
        RC = RoundController.Instance;
        spawnPoint = GameObject.Find("SpawnPointSlaughter");
        endPoint = GameObject.Find("EndPoint");
        chair = GameObject.Find("Chair");
        uiController = GameObject.Find("SlaughterUI").GetComponent<SlaughterUiController>();
        PatientsDump = GameObject.Find("PatientsDump");
    }
    #endregion
}