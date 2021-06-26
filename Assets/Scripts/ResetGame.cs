using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetGame : MonoBehaviour
{
    public GameObject dialoguePromptPrefab;
    private GameObject prompt = null;
    public Canvas canvas;

    private Button[] buttons;
    private Button confirmButton;
    private Button cancelButton;
    private Button extraButton;
    private Text[] texts;
    private Text dialogueText;
    private Text confirmButtonText;
    private Text cancelButtonText;
    private Text extraButtonText;

    public void OnButtonClick() //TODO prevent background clicks when on screen
    {
        if (prompt != null) { Destroy(prompt); return; }
        prompt = Instantiate(dialoguePromptPrefab, new Vector3(Screen.width / 2, Screen.height / 2, 1), Quaternion.identity, canvas.transform);
        prompt.transform.localScale = new Vector3(0.5f, 0.5f, 0);

        buttons = prompt.transform.GetComponentsInChildren<Button>();

        foreach (Button B in buttons)
        {
            if(B.name == "ConfirmButton")
            {
                confirmButton = B;
            }
            else if(B.name == "CancelButton")
            {
                cancelButton = B;
            }
            else if(B.name == "ExtraButton")
            {
                extraButton = B;
            }
        }

        confirmButton.onClick.AddListener(() => ConfirmButton());
        cancelButton.onClick.AddListener(() => CancelButton());
        extraButton.onClick.AddListener(() => ShuffleDeck());

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

        dialogueText.text = "Are you sure you want to restart the game?";

        confirmButtonText.text = "Yeah, this game is impossible :(";

        cancelButtonText.text = "Nah, I got this";

        extraButtonText.text = "Reshuffle";
    }

    public UnityEngine.Events.UnityAction ConfirmButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        return null; //TODO find out if this should actually return something
    }

    public UnityEngine.Events.UnityAction CancelButton()
    {
        if (prompt)
        {
            Destroy(prompt);
        }
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
        }
        return null;
    }
}