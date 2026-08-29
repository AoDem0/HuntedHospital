using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HowToPlayScript : MonoBehaviour
{
    public List<GameObject> pages = new List<GameObject>();
    public Button Prev;
    public Button Next;
    public Button BackToMenuBut;

    public GameObject currentPage;
    public int currentPageInt;
    [SerializeField] private soundManager soundMan;


    public void Awake()
    {
        soundMan = FindAnyObjectByType<soundManager>();

        foreach (var page in pages)
        {
            page.gameObject.SetActive(false);
        }

        pages[0].gameObject.SetActive(true);
        currentPageInt = 0;
        Prev.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentPageInt == 0)
        {
            Prev.gameObject.SetActive(false);
        }
        else
        {
            Prev.gameObject.SetActive(true);
        }

        if (currentPageInt == pages.Count - 1)
        {
            Next.gameObject.SetActive(false);
        }
        else
        {
            Next.gameObject.SetActive(true);
        }
    }

    public void NextPage()
    {
        soundMan.Play("uiclick");
        foreach (var page in pages)
        {
            page.gameObject.SetActive(false);
        }

        currentPageInt++;
        pages[currentPageInt].gameObject.SetActive(true);


    }

    public void PrevPage()
    {
        soundMan.Play("uiclick");
        foreach (var page in pages)
        {
            page.gameObject.SetActive(false);
        }
        currentPageInt--;
        pages[currentPageInt].gameObject.SetActive(true);
    }

    public void BackToMenu()
    {
        soundMan.Play("uiclick");
        SceneManager.LoadScene("Menu");
    }
}
