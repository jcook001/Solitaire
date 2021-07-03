using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsManager;
    private void Start()
    {
        if (FindObjectOfType<Options>())
        {
            optionsManager = FindObjectOfType<Options>().gameObject;
        }
        else
        {
            optionsManager = Instantiate(optionsManager);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        this.enabled = false;

    }

    public void DrawOne()
    {
        optionsManager.GetComponent<Options>().DrawOne();
    }

    public void DrawThree()
    {
        optionsManager.GetComponent<Options>().DrawThree();
    }

    public void DarkModeOn()
    {
        optionsManager.GetComponent<Options>().DarkModeOn();
    }

    public void DarkModeOff()
    {
        optionsManager.GetComponent<Options>().DarkModeOff();
    }

    public void CardBackToggle()
    {
        optionsManager.GetComponent<Options>().CardBackSelect();
    }

    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level 1");
        Options.canPlaceAnyCardInEmptySpace = true;
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void PlayLevel3()
    {
        SceneManager.LoadScene("Level 3");
        Options.canPlaceAnyCardInEmptySpace = true;
    }
}
