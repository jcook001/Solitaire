using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePromptPrefab;
    private GameObject prompt = null;
    public Canvas canvas;
    public Image dialogueOverlay;

    private Button[] buttons;
    private Button confirmButton;
    private Button cancelButton;
    private Button extraButton;
    private Text[] texts;
    private Text dialogueText;
    private Text confirmButtonText;
    private Text cancelButtonText;
    private Text extraButtonText;
    private bool canPromptBeClosed = true;

    private bool CreateDialogue()
    {
        if (prompt != null) { Destroy(prompt); dialogueOverlay.gameObject.SetActive(false); return false; }
        prompt = Instantiate(dialoguePromptPrefab, new Vector3(Screen.width / 2, Screen.height / 2, 1), Quaternion.identity, canvas.transform);
        prompt.transform.localScale = new Vector3(0.5f, 0.5f, 0);

        buttons = prompt.transform.GetComponentsInChildren<Button>();

        foreach (Button B in buttons)
        {
            if (B.name == "ConfirmButton")
            {
                confirmButton = B;
            }
            else if (B.name == "CancelButton")
            {
                cancelButton = B;
            }
            else if (B.name == "ExtraButton")
            {
                extraButton = B;
            }
        }

        texts = prompt.transform.GetComponentsInChildren<Text>();

        foreach (Text T in texts)
        {
            if (T.name == "Dialogue")
            {
                dialogueText = T;
            }
            else if (T.name == "ConfirmButtonText")
            {
                confirmButtonText = T;
            }
            else if (T.name == "CancelButtonText")
            {
                cancelButtonText = T;
            }
            else if (T.name == "ExtraButtonText")
            {
                extraButtonText = T;
            }

        }

        dialogueOverlay.gameObject.SetActive(true);
        return true;
    }
    
    public void RestartGame() //TODO prevent background clicks when on screen
    {
        if (CreateDialogue())
        {
            confirmButton.onClick.AddListener(() => ReloadScene());
            cancelButton.onClick.AddListener(() => CancelButton());
            extraButton.onClick.AddListener(() => ShuffleDeck());

            dialogueText.text = "Are you sure you want to restart the game?";

            confirmButtonText.text = "Yeah, this game is impossible :(";

            cancelButtonText.text = "Nah, I got this";

            extraButtonText.text = "Reshuffle";

            canPromptBeClosed = true;
        }
    }

    public void WinGame()
    {
        if (CreateDialogue())
        {
            confirmButton.onClick.AddListener(() => ReloadScene());
            cancelButton.onClick.AddListener(() => QuitGame());
            GameObject extra = GameObject.Find("ExtraButton");
            Destroy(extra);

            dialogueText.text = "Congratulations for wasting your time! \nWould you like to waste even more time?";

            confirmButtonText.text = "Yeah, I fucking LOVE Solitaire";

            cancelButtonText.text = "NO, Jamie's not that good a friend to me so I don't want to play again";

            canPromptBeClosed = false;
        }
    }

    public void QuitGamePrompt()
    {
        if (CreateDialogue())
        {
            confirmButton.onClick.AddListener(() => CancelButton());
            cancelButton.onClick.AddListener(() => QuitToMenu());
            GameObject extra = GameObject.Find("ExtraButton");
            Destroy(extra);

            dialogueText.text = "Are you sure you want to close the game?";

            confirmButtonText.text = "No";

            cancelButtonText.text = "Actually, yeah. This game is shit";

            canPromptBeClosed = true;
        }
    }

    public UnityEngine.Events.UnityAction ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        return null; //TODO find out if this should actually return something
    }

    public UnityEngine.Events.UnityAction CancelButton()
    {
        if (prompt)
        {
            Destroy(prompt);
            dialogueOverlay.gameObject.SetActive(false);
        }
        return null;
    }

    public UnityEngine.Events.UnityAction QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

    #else
        Application.Quit();

    #endif

        return null;
    }

    public UnityEngine.Events.UnityAction QuitToMenu()
    {
        SceneManager.LoadScene("Menu");
        return null;
    }

    public UnityEngine.Events.UnityAction ShuffleDeck()
    {
        Solitaire solitaire = GameObject.Find("SolitaireGame").GetComponent<Solitaire>();
        DeckButton deckButton = GameObject.Find("DeckArea").GetComponent<DeckButton>();
        deckButton.TurnOverDeck(solitaire.deck);
        solitaire.ShuffleCards(solitaire.deck);

        if (prompt)
        {
            Destroy(prompt);
            dialogueOverlay.gameObject.SetActive(false);
        }
        return null;
    }

    /// <summary>
    /// Closes the open dialogue prompt if there is one and it can be closed
    /// set bool to true to override the prompt not being closable
    /// </summary>
    /// <param name="shouldOverrideCanBeClosed"></param>
    /// <returns></returns>
    public void ClosePrompt(bool shouldOverrideCanBeClosed)
    {
        if (prompt.gameObject.activeSelf)
        {
            if (canPromptBeClosed || shouldOverrideCanBeClosed)
            {
                Destroy(prompt);
                dialogueOverlay.gameObject.SetActive(false);
            }
        }
    }
}
