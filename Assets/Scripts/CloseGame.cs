using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseGame : MonoBehaviour
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

    public void OnButtonClick()
    {
        {
            if (prompt != null) { Destroy(prompt); return; }
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

            confirmButton.onClick.AddListener(() => ConfirmButton());
            cancelButton.onClick.AddListener(() => CancelButton());
            GameObject extra = GameObject.Find("ExtraButton");
            Destroy(extra);

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
            }

            dialogueText.text = "Are you sure you want to close the game?";

            confirmButtonText.text = "No";

            cancelButtonText.text = "Actually, yeah. This game is shit";
        }
    }

    public UnityEngine.Events.UnityAction ConfirmButton()
    {
         if (prompt)
         {
             Destroy(prompt);
         }
         return null;
    }


    public UnityEngine.Events.UnityAction CancelButton()
    {
        Application.Quit();
        return null;
    }

}