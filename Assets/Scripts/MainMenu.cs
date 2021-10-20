using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playButton;
    private Vector2 playButtonPosition;
    public CanvasGroup playButtonCanvas;
    public GameObject optionsButton;
    private Vector2 optionsButtonPosition;
    public CanvasGroup optionsButtonCanvas;
    public GameObject quitButton;
    private Vector2 quitButtonPosition;
    public CanvasGroup quitButtonCanvas;
    public GameObject backButton;
    private Vector2 backButtonPosition;
    public CanvasGroup backButtonCanvas;
    public GameObject cardBack;
    private Vector2 cardBackPosition;
    public CanvasGroup cardBackCanvas;
    public GameObject cardDrawButton;
    private Vector2 cardDrawButtonPosition;
    public GameObject darkModeButton;
    private Vector2 darkModeButtonPosition;
    public GameObject musicInformation;
    private Vector2 musicInformationPosition;

    private bool mainMenuIsActive = true;
    private float menuTransitionTime = 0.5f;

    public GameObject optionsManager;
    public GameObject audioManagerGameObject;
    private AudioManager audioManager;
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

        if (FindObjectOfType<AudioManager>())
        {
            audioManagerGameObject = FindObjectOfType<AudioManager>().gameObject;
        }
        else
        {
            audioManagerGameObject = Instantiate(audioManagerGameObject);
        }
        audioManager = audioManagerGameObject.GetComponent<AudioManager>();

        //Save the start position of the menu buttons
        playButtonPosition = playButton.transform.position;
        optionsButtonPosition = optionsButton.transform.position;
        quitButtonPosition = quitButton.transform.position;
        backButtonPosition = backButton.transform.position;
        cardBackPosition = cardBack.transform.position;
        cardDrawButtonPosition = cardDrawButton.transform.position;
        darkModeButtonPosition = darkModeButton.transform.position;
        musicInformationPosition = musicInformation.transform.position;
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

    public void MenuTransition()
    {
        if (mainMenuIsActive)
        {
            StartCoroutine(SlideAndFadeButtonsOut());
        }
        else
        {
            StartCoroutine(SlideAndFadeButtonsIn());
        }
    }

    private IEnumerator SlideAndFadeButtonsOut()
    {
        mainMenuIsActive = false;

        //Move the play, options and quit button off the screen and fade them out
        playButton.transform.LeanMoveX(playButton.transform.position.x + 500, menuTransitionTime); //TODO make the movement length relative to the screen size
        playButtonCanvas.LeanAlpha(0, 0.5f);
        //wait until the play button has reached half the width of the next button to start moving that button
        while(playButton.transform.position.x < optionsButton.transform.position.x + optionsButton.GetComponent<RectTransform>().rect.width/2)
        {
            yield return null;
        }
        optionsButton.transform.LeanMoveX(optionsButton.transform.position.x + 500, menuTransitionTime);
        optionsButtonCanvas.LeanAlpha(0, 0.5f);

        while (optionsButton.transform.position.x < quitButton.transform.position.x + quitButton.GetComponent<RectTransform>().rect.width/2)
        {
            yield return null;
        }
        quitButton.transform.LeanMoveX(quitButton.transform.position.x + 500, menuTransitionTime);
        quitButtonCanvas.LeanAlpha(0, 0.5f);
        yield return null;

        //Bring new buttons onto the screen
        cardBack.transform.LeanMoveLocalY(cardBack.transform.position.y + Screen.height/4, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        backButton.transform.LeanMoveLocalY(backButton.transform.position.y + Screen.height/4, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        musicInformation.transform.LeanMoveLocalY(musicInformation.transform.position.y + Screen.height / 4, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardDrawButton.transform.LeanMoveLocalY(cardDrawButton.transform.position.y + Screen.height / 4, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        darkModeButton.transform.LeanMoveLocalY(darkModeButton.transform.position.y + Screen.height / 4, menuTransitionTime).setEaseInOutBack();

    }

    private IEnumerator SlideAndFadeButtonsIn()
    {
        //Send options buttons off the screen
        cardBack.transform.LeanMoveY(cardBackPosition.y, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        backButton.transform.LeanMoveY(backButtonPosition.y, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        musicInformation.transform.LeanMoveY(musicInformationPosition.y, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardDrawButton.transform.LeanMoveY(cardDrawButtonPosition.y, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        darkModeButton.transform.LeanMoveY(darkModeButtonPosition.y, menuTransitionTime).setEaseInOutBack();

        //Bring menu buttons back
        mainMenuIsActive = true;
        playButton.transform.LeanMoveX(playButton.transform.position.x -500, menuTransitionTime);
        playButtonCanvas.LeanAlpha(1, 0.5f);
        yield return new WaitForSeconds(0.25f);
        optionsButton.transform.LeanMoveX(optionsButton.transform.position.x - 500, menuTransitionTime);
        optionsButtonCanvas.LeanAlpha(1, 0.5f);
        yield return new WaitForSeconds(0.25f);
        quitButton.transform.LeanMoveX(quitButton.transform.position.x - 500, menuTransitionTime);
        quitButtonCanvas.LeanAlpha(1, 0.5f);
        yield return null;
    }

    public void AudioPreviousTrack()
    {
        audioManager.PreviousTrack();
    }

    public void AudioNextTrack()
    {
        audioManager.NextTrack();
    }

    public void AudioPlayPauseTrack()
    {
        audioManager.PlayPause();
    }
}
