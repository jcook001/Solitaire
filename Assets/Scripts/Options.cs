using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static int drawAmount = 3;
    public ToggleGroup cardBackToggleGroup;
    public static Sprite cardBackChoice;
    public bool darkMode = true;
    public Sprite cardBackDefault;
    private Toggle activeToggle;
    public GameState gameState;

    public static bool canPlaceAnyCardInEmptySpace = false; //Set by the Main Menu script

    private void Start()
    {
        DontDestroyOnLoad(this);
        cardBackChoice = cardBackDefault;
    }

    public void DrawOne()
    {
        drawAmount = 1;
    }

    public void DrawThree()
    {
        drawAmount = 3;
    }

    public void CardBackSelect()
    {
        if (!cardBackToggleGroup)
        {
            cardBackToggleGroup = FindObjectOfType<ToggleGroup>();
            if (!cardBackToggleGroup) { return; }
        }
        activeToggle = cardBackToggleGroup.GetFirstActiveToggle();
        cardBackChoice = activeToggle.transform.parent.GetComponent<Image>().sprite;
    }

    public void NewCardBackSelect(Sprite card)
    {
        cardBackChoice = card;
    }

    public void DarkModeOn()
    {
        darkMode = true;
    }

    public void DarkModeOff()
    {
        darkMode = false;
    }

}
