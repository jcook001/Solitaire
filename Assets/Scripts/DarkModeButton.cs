using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DarkModeButton : MonoBehaviour
{
    private Solitaire solitaire;
    private Options options;
    public TextMeshPro text;
    private bool hasBeenClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        options = FindObjectOfType<Options>();
        text = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
        Debug.Log(text);
    }

    public void OnClick()
    {
        if (hasBeenClicked)
        {
            text.text = "";
        }
    }
}
