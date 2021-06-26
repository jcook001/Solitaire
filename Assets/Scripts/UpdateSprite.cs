using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private Image cardDisplay;
    private CardInfo cardInfo;
    private Solitaire solitaire;
    private Options options;

    // Start is called before the first frame update
    void Start()
    {
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
        options = FindObjectOfType<Options>();

        bool cardFaceIsSet = false;

        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                if (options.darkMode)
                {
                    cardFace = solitaire.cardFacesDark[i];
                }
                else
                {
                    cardFace = solitaire.cardFaces[i];
                }
                cardFaceIsSet = true;
                this.gameObject.GetComponent<Image>().color = Color.white;
                break;
            }
            i++;
        }

        if (!cardFaceIsSet)
        {
            cardFace = solitaire.cardFaces[52]; //set the card to be a joker if it can't find it's card face
        }

        cardBack = Options.cardBackChoice;

        cardDisplay = GetComponent<Image>();
        cardInfo = GetComponent<CardInfo>();
        if(cardInfo.faceUp == true)
        {
            cardDisplay.sprite = cardFace;
        }
        else
        {
            cardDisplay.sprite = cardBack;
        }
    }

    private void Update()
    {
        //Debug show collisions
        if (DebugFunctions.showTransparentCollision)
        {
            if (DebugFunctions.showTransparentCollision)
            {
                if (GetComponent<BoxCollider2D>().isActiveAndEnabled)
                {
                    var tempColor = cardDisplay.color;
                    tempColor.a = 1.0f;
                    cardDisplay.color = tempColor;
                }
                else if (!GetComponent<BoxCollider2D>().isActiveAndEnabled)
                {
                    var tempColor = cardDisplay.color;
                    tempColor.a = 0.25f;
                    cardDisplay.color = tempColor;
                }
            }
            else
            {
                var tempColor = cardDisplay.color;
                tempColor.a = 1.0f;
                cardDisplay.color = tempColor;
            }
        }
        else
        {
            var tempColor = cardDisplay.color;
            tempColor.a = 1.0f;
            cardDisplay.color = tempColor;
        }
    }
}
