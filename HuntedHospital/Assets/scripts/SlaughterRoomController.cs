using UnityEngine;
using UnityEngine.SceneManagement;

public class SlaughterRoomController : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject endPoint;
    public GameObject chair;
    public GameObject slaughterUI;
    public int currentPatient;
    public int patientsToServe;
    public SlaughterUiController uiController;
    public RoundController RC;
    public Vector3 moveTarget;
    private bool slaughterEnded = false;

    void Awake()
    {
        RefreshSceneObjects();
        currentPatient = 0;
        moveTarget = Vector3.zero;
        SpawnNextPatientInRoom();
        patientsToServe = RC.patientList.Count;
        slaughterEnded = false;
    }

    void Update()
    {
        if (patientsToServe <= 0 && !slaughterEnded)
        {
            RC.EndFeastPhase();
            slaughterEnded = true;
            return;
        }

        var patient = RC.patientList[currentPatient];

        if (moveTarget != Vector3.zero)
        {
            patient.MoveToTarget(moveTarget);
            Debug.Log($"Moving patient {currentPatient} to target {moveTarget}");
        }
        
        if (patient.gameObject.transform.position == chair.transform.position && moveTarget != Vector3.zero)
        {
            moveTarget = Vector3.zero;
            uiController.ToggleCharInfo();
            uiController.DisplayPatientInfo(patient);
            Debug.Log("Patient reached chair, displaying info");
        }
        else if (patient.canGoToExit)
        {
            Debug.Log("Patient blood drained, moving to end point");
            moveTarget = endPoint.transform.position;

            if(patient.gameObject.transform.position == endPoint.transform.position)
            {
                ClearPatient();
                Debug.Log("Cleared patient after moving to end point");
            }
        }

    }

    private void SpawnNextPatientInRoom()
    {
        var patient = RC.patientList[currentPatient];
        patient.transform.position = spawnPoint.transform.position;
        patient.SetSpriteForSide(PatientScript.PatientSpawnSide.Left); //to jest do zmiany fest
        moveTarget = chair.transform.position;
    }

    public void KillPatient()
    {
        var patient = RC.patientList[currentPatient];
        patient.currentChar.ApplyGlobalDebuffs();
        patient.currentChar.ApplyOnKillGlobal();
        RC.bloodInBank += (patient.bloodAmmount * RC.bloodReceivedMultiplier);
        RC.currentGhostCount++;
        Debug.Log($"Patient killed. Blood in bank: {RC.bloodInBank}, Current ghost count: {RC.currentGhostCount}");
        ClearPatient();
    }
    public void DrainBloodFromPatient()
    {
        var patient = RC.patientList[currentPatient];
        patient.currentChar.ApplyGlobalDebuffs();
        patient.currentChar.ApplyOnBloodDrainPersonal(patient);
        RC.bloodInBank += (patient.bloodAmmount * 0.4f ) * RC.bloodReceivedMultiplier;
        patient.canGoToExit = true;
        Debug.Log($"Drained blood from patient. Blood in bank: {RC.bloodInBank}");
    }

    public void LetPatientGo()
    {
        var patient = RC.patientList[currentPatient];
        patient.canGoToExit = true;
        Debug.Log($"Patient let go. Current ghost count: {RC.currentGhostCount}");
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
        Debug.Log($"Cleared patient and updated counters. Current patient index: {currentPatient}, Patients to serve: {patientsToServe}");
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
        slaughterUI = GameObject.Find("SlaughterUI");
    }
    #endregion
}