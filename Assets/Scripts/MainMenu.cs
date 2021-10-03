using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsManager;
    public Animator transition;
    public Animation menuClose;

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
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
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

    private IEnumerator SceneTransition(int sceneIndex)
    {
        float transitionStartTime = Time.time;
        //start the transition
        transition.SetTrigger("Menu Close");
        //Find the length of the anim and wait that long
        float transitionLength = transition.GetCurrentAnimatorClipInfo(0).Length;
        
        while (Time.time < transitionStartTime + transitionLength)
        {
            yield return null;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public void PlayLevel1()
    {
        StartCoroutine(SceneTransition(1));
        Options.canPlaceAnyCardInEmptySpace = true;
    }

    public void PlayLevel2()
    {
        StartCoroutine(SceneTransition(2));
    }

    public void PlayLevel3()
    {
        StartCoroutine(SceneTransition(3));
        Options.canPlaceAnyCardInEmptySpace = true;
    }
}
