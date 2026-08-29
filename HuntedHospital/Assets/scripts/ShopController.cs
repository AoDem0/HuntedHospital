using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class ShopController : MonoBehaviour
{
    [SerializeField]private soundManager soundMan;
    public List<DeathDealPanel> dealPanels = new List<DeathDealPanel>();
    public List<BuffsSO> buffsToUse = new List<BuffsSO>();
    public GameObject GhostWarningObject;
    public GameObject GhostDisplay;
    public TextMeshProUGUI GhostCountDisplay;
    public GameObject GhostWarning;
    public int GhostCountForTest;

    public Coroutine WarningCoroutine;
    void Awake()
    {
        soundMan = FindAnyObjectByType<soundManager>();
        RandomizeBuffTier(dealPanels);
        RefreshSceneObjects();
        GhostCountForTest = RoundController.Instance.currentGhostCount;
        GhostCountDisplay.text = GhostCountForTest.ToString();
    }

    private void RandomizeBuffTier(List<DeathDealPanel> panels)
    {
        foreach (var panel in panels)
        {
            var buff = panel.buff;
            float randomNum = Random.Range(0, 10) / 3;
            int tier = 1;

            if (randomNum < 2)
            {
                tier = 1;
            }
            else if (randomNum >= 2 && randomNum < 3)
            {
                tier = 2;
            }
            else if (randomNum >= 3)
            {
                tier = 3;
            }
            buff.buffTier = tier;
            buff.RecalculateStats();
            panel.SetBuffOnDealPanel(buff);
        }
    }

    public void TriggerNotEnoguhGhosts()
    {
        if (WarningCoroutine != null)
        {
            StopCoroutine(WarningCoroutine);
        }

        WarningCoroutine = StartCoroutine(ShowGhostWarning(5f));
    }

    private IEnumerator ShowGhostWarning(float time)
    {
        Transform found = GhostWarningObject.transform.Find("GhostWarning");
        if (transform != null)
        {
            GhostWarning = found.gameObject;
        }
        GhostWarning.SetActive(true);
        yield return new WaitForSeconds(time);
        GhostWarning.SetActive(false);
    }

    public void EndDealPhase()
    {
        soundMan.Play("uiclick");
        RoundController.Instance.EndDealPhase();
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
        if (scene.name == "deathshop")
        {
            RefreshSceneObjects();
        }
    }
    
    private void RefreshSceneObjects()
    {
        GhostWarningObject = GameObject.Find("GhostWarningObject");
        GhostDisplay = GameObject.Find("GhostDisplayAtShop");
        GhostCountDisplay = GhostDisplay.transform.Find("GhostNumDisplayAtShop").GetComponent<TextMeshProUGUI>();
    }
    #endregion
}
