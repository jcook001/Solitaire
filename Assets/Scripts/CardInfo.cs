using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
    public bool faceUp = false;

    private string valueString;
    public int value;
    public char suit;
    public char suitColour;

    private DragDrop dragDrop;

    // Start is called before the first frame update
    void Start()
    {
        suit = transform.name[0];

        if (suit == 'C' || suit == 'S')
        {
            suitColour = 'B';
        }
        else if (suit == 'D' || suit == 'H')
        {
            suitColour = 'R';
        }
        else
        {
            Debug.LogError("Somthing's gone wrong with determining a card's colour!");
        }

        for (int i = 1; i < transform.name.Length; i++) //because the value goes up to 10 and we use letters we can't just test the second char easily
        {
            char c = transform.name[i];
            valueString = valueString + c.ToString();
        }

        if (int.TryParse(valueString, out value))
        {

        }
        else if (valueString == "A")
        {
            value = 1;
        }
        else if (valueString == "J")
        {
            value = 11;
        }
        else if (valueString == "Q")
        {
            value = 12;
        }
        else if (valueString == "K")
        {
            value = 13;
        }

        dragDrop = GetComponent<DragDrop>();
    }

    public void OnClick()
    {
        if(faceUp == false && this.transform.childCount == 0)
        {
            faceUp = true;
            GetComponent<Image>().sprite = GetComponent<UpdateSprite>().cardFace;
            this.GetComponent<BoxCollider2D>().enabled = true;
            this.tag = "Face Up Play Area";
            Solitaire.SetCanUndo(false);
            dragDrop.AutoCompleteCheck();
        }
    }
}
