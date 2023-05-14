using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject TitleText;
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
    public Sprite[] cardBackChoices;
    public GameObject cardBackRightButton;
    private Vector2 cardBackRightButtonPosition;
    public GameObject cardBackLeftButton;
    private Vector2 cardBackLeftButtonPosition;
    public GameObject cardDrawButton;
    private Vector2 cardDrawButtonPosition;
    public GameObject cardDrawAmountText;
    public GameObject darkModeButton;
    private Vector2 darkModeButtonPosition;
    public TextMeshProUGUI darkModeOptionText;
    public GameObject musicInformation;
    private Vector2 musicInformationPosition;
    public TextMeshProUGUI musicInformationText;
    public GameObject audioPauseButton;
    public Sprite audioPlayIcon;
    public Sprite audioPauseIcon;

    private bool mainMenuIsActive = true;
    private float menuTransitionTime = 0.5f;

    public GameObject optionsManager;
    public GameObject audioManagerGameObject;
    private AudioManager audioManager;
    private AudioSource audioSource;
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
        audioSource = audioManager.GetComponent<AudioSource>();

        //Save the start position of the menu buttons
        playButtonPosition = playButton.transform.position;
        optionsButtonPosition = optionsButton.transform.position;
        quitButtonPosition = quitButton.transform.position;
        backButtonPosition = backButton.transform.position;
        cardBackPosition = cardBack.transform.position;
        cardBackLeftButtonPosition = cardBackLeftButton.transform.position;
        cardBackRightButtonPosition = cardBackRightButton.transform.position;
        cardDrawButtonPosition = cardDrawButton.transform.position;
        darkModeButtonPosition = darkModeButton.transform.position;
        musicInformationPosition = musicInformation.transform.position;

        //Find the music name text
        musicInformationText = musicInformation.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
                QuitGame();
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

        //fade out the title text because phone screens don't have enough height to accomodate everything
    #if PLATFORM_ANDROID
        TitleText.GetComponent<CanvasGroup>().LeanAlpha(0, 1.0f);
    #endif

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
#if PLATFORM_ANDROID //TODO set this up in a way that can handle various screen sizes instead of coding multiple paths
        cardBack.transform.LeanMoveY(cardBack.transform.position.y + Screen.height / 1.5f, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardBackRightButton.transform.LeanMoveY(cardBackRightButton.transform.position.y + Screen.height / 1.5f, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardBackLeftButton.transform.LeanMoveY(cardBackLeftButton.transform.position.y + Screen.height / 1.5f, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        backButton.transform.LeanMoveY(backButton.transform.position.y + Screen.height / 1.5f, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        musicInformation.transform.LeanMoveY(musicInformation.transform.position.y + Screen.height / 1.5f, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardDrawButton.transform.LeanMoveY(cardDrawButton.transform.position.y + Screen.height / 1.5f, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        darkModeButton.transform.LeanMoveY(darkModeButton.transform.position.y + Screen.height / 1.5f, menuTransitionTime).setEaseInOutBack();

#else 
        cardBack.transform.LeanMoveLocalY(cardBack.transform.position.y + Screen.height / 4, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardBackRightButton.transform.LeanMoveLocalY(cardBackRightButton.transform.position.y + Screen.height / 4, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardBackLeftButton.transform.LeanMoveLocalY(cardBackLeftButton.transform.position.y + Screen.height / 4, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        backButton.transform.LeanMoveLocalY(backButton.transform.position.y + Screen.height / 4, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        musicInformation.transform.LeanMoveLocalY(musicInformation.transform.position.y + Screen.height / 4, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardDrawButton.transform.LeanMoveLocalY(cardDrawButton.transform.position.y + Screen.height / 4, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        darkModeButton.transform.LeanMoveLocalY(darkModeButton.transform.position.y + Screen.height / 4, menuTransitionTime).setEaseInOutBack();

#endif
    }

    private IEnumerator SlideAndFadeButtonsIn()
    {
        //Send options buttons off the screen
        cardBack.transform.LeanMoveY(cardBackPosition.y, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardBackLeftButton.transform.LeanMoveY(cardBackLeftButtonPosition.y, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardBackRightButton.transform.LeanMoveY(cardBackRightButtonPosition.y, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        backButton.transform.LeanMoveY(backButtonPosition.y, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        musicInformation.transform.LeanMoveY(musicInformationPosition.y, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        cardDrawButton.transform.LeanMoveY(cardDrawButtonPosition.y, menuTransitionTime).setEaseInOutBack();
        yield return new WaitForSeconds(0.25f);
        darkModeButton.transform.LeanMoveY(darkModeButtonPosition.y, menuTransitionTime).setEaseInOutBack();

        //Fade the title text back in
#if PLATFORM_ANDROID
        TitleText.GetComponent<CanvasGroup>().LeanAlpha(1, 1.0f);
#endif

        //Bring menu buttons back
        mainMenuIsActive = true;
        playButton.transform.LeanMoveX(playButtonPosition.x, menuTransitionTime);
        playButtonCanvas.LeanAlpha(1, 0.5f);
        yield return new WaitForSeconds(0.25f);
        optionsButton.transform.LeanMoveX(optionsButtonPosition.x, menuTransitionTime);
        optionsButtonCanvas.LeanAlpha(1, 0.5f);
        yield return new WaitForSeconds(0.25f);
        quitButton.transform.LeanMoveX(quitButtonPosition.x, menuTransitionTime);
        quitButtonCanvas.LeanAlpha(1, 0.5f);
        yield return null;
    }

    public void AudioPreviousTrack()
    {
        audioManager.PreviousTrack();
        UpdateAudioIcon();
    }

    public void AudioNextTrack()
    {
        audioManager.NextTrack();
        UpdateAudioIcon();
    }

    public void AudioPlayPauseTrack()
    {
        audioManager.PlayPause();
        UpdateAudioIcon();
    }

    private void UpdateAudioIcon()
    {
        if (audioSource.isPlaying)
        {
            audioPauseButton.GetComponent<Image>().sprite = audioPauseIcon;
        }
        else
        {
            audioPauseButton.GetComponent<Image>().sprite = audioPlayIcon;
        }
    }

    public void UpdateTrackName(string trackName)
    {
        musicInformationText.text = trackName;
    }

    public void ToggleDrawAmount()
    {
        if(Options.drawAmount == 3)
        {
            optionsManager.GetComponent<Options>().DrawOne();
            cardDrawAmountText.GetComponent<TextMeshProUGUI>().text = "1";
        }
        else
        {
            optionsManager.GetComponent<Options>().DrawThree();
            cardDrawAmountText.GetComponent<TextMeshProUGUI>().text = "3";
        }
    }

    public void ToggleDarkMode()
    {
        if (optionsManager.GetComponent<Options>().darkMode)
        {
            optionsManager.GetComponent<Options>().darkMode = false;
            darkModeOptionText.text = "Off";
        }
        else
        {
            optionsManager.GetComponent<Options>().darkMode = true;
            darkModeOptionText.text = "On";
        }
    }

    public void CycleCardBack(string direction)
    {
        if(direction == "right")
        {
            //Make the new card back and move it to it's hidden position
            GameObject newCardBack = Instantiate(cardBack, cardBack.transform);
            newCardBack.transform.SetParent(cardBack.transform.parent);
            //newCardBack.GetComponent<CanvasGroup>().alpha = 0.0f; //TODO make this work rather than the card staying invisible after the function
            newCardBack.transform.position = new Vector2(cardBack.transform.position.x - Screen.width / 4, cardBack.transform.position.y);
            //Move the visible card out of the way and fade it out
            cardBack.LeanMoveX(cardBack.transform.position.x + Screen.width / 4, 0.25f).setEaseInOutQuad();
            cardBack.LeanAlpha(0.0f, 0.25f);
            //Move the new card in and fade it in
            newCardBack.LeanMoveX(cardBackPosition.x, 0.25f).setEaseInOutQuad();
            newCardBack.LeanAlpha(1.0f, 0.25f);
            //destroy the old card and make it reference the new card
            Destroy(cardBack);
            cardBack = newCardBack;

        }
        else if(direction == "left")
        {
            cardBack.LeanMoveX(cardBack.transform.position.x - Screen.width / 4, 0.25f).setEaseInOutQuad();
        }
        //move card to the right and fade out at the same time
        //start bringing the next card into view and fade in
    }
}
