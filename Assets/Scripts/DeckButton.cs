using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DeckButton : MonoBehaviour
{
    private Solitaire solitaire;
    [SerializeField]
    private bool hasCardBeenDrawn = false;
    public bool isDeckAndDiscardPileEmpty = false;
    public Image topOfDeck;
    public Image replayIcon;
    public GameObject discardPile;
    public GameObject cardPrefab;
    public GameObject drawCardLocation;
    public float xOffSet;
    private float zOffSet;

    public GameObject topOfDiscardPile;

    public List<string> discardPileList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        solitaire = GameObject.Find("SolitaireGame").GetComponent<Solitaire>();
        zOffSet = solitaire.zOffsetIncrement;
        xOffSet = solitaire.xOffsetIncrement;
        Sprite cardBackChoice = Options.cardBackChoice;
        topOfDeck.sprite = cardBackChoice;
    }

    public void ClickHandler()
    {
        if (isDeckAndDiscardPileEmpty) { return; }
        if(!hasCardBeenDrawn)
        {
            TurnOverDeck(solitaire.deck);
            hasCardBeenDrawn = true;

            if (Options.drawAmount == 1)
            {
                DrawSingleCard(solitaire.deck);
            }
            else if (Options.drawAmount == 3)
            {
                DrawThreeCards(solitaire.deck);
            }
            Solitaire.SetCanUndo(true, "Deck");
        }
        else if(solitaire.deck.Count > 0)
        {
            if (Options.drawAmount == 1)
            {
                DrawSingleCard(solitaire.deck);
            }
            else if (Options.drawAmount == 3)
            {
                DrawThreeCards(solitaire.deck);
            }
            Solitaire.SetCanUndo(true, "Deck");
        }
        else
        {
            HideDiscardPile();
        }
    }

    private void DrawSingleCard(List<string> deck)
    {
        if (discardPileList.Count == 0) //if (discardPile.transform.childCount == 0)
        {
            drawCardLocation = discardPile;
        }
        else
        {
            drawCardLocation = topOfDiscardPile;
        }

        GameObject newCard = Instantiate(cardPrefab,
                                 new Vector3(drawCardLocation.transform.position.x,
                                 drawCardLocation.transform.position.y,
                                 drawCardLocation.transform.position.z + zOffSet),
                                 Quaternion.identity,
                                 drawCardLocation.transform);
        newCard.name = deck.Last<string>();
        newCard.GetComponent<CardInfo>().faceUp = true;
        newCard.tag = "Face Up Discard Pile";
        discardPileList.Add(deck.Last<string>());
        deck.RemoveAt(deck.Count - 1);
        topOfDiscardPile = newCard;
        drawCardLocation.GetComponent<BoxCollider2D>().enabled = false;

        if (deck.Count == 0)
        {
            topOfDeck.enabled = false;
        }
    }

    private void DrawThreeCards(List<string> deck)
    {
        int cardsRemainingInDeck = deck.Count;
        int cardsToDraw;
        if (cardsRemainingInDeck / 3 < 1)
        {
            cardsToDraw = deck.Count % 3; // This will tell us how many cards can be drawn (0 = 3, 2 = 2, 1 = 1)
            if (cardsToDraw == 0)
            {
                cardsToDraw = 3;
            }
        }
        else
        {
            cardsToDraw = 3;
        }

        if (discardPile.transform.childCount == 0)
        {
            drawCardLocation = discardPile;
        }
        else
        {
            GameObject discardPileTemp = discardPile;
            while(discardPileTemp.transform.childCount > 0)
            {
                discardPileTemp.transform.GetChild(0).gameObject.transform.position = new Vector3(discardPileTemp.transform.position.x,
                                                                                                  discardPileTemp.transform.position.y,
                                                                                                  discardPileTemp.transform.position.z + zOffSet);
                discardPileTemp = discardPileTemp.transform.GetChild(0).gameObject;
                                
            }

            zOffSet = solitaire.zOffsetIncrement;

            drawCardLocation = topOfDiscardPile;
        }

        //for loop based on cardsToDraw (i = cardstodraw; i>0; i--)
        for (int j = cardsToDraw; j > 0; j--)
        {
            if(j == cardsToDraw)
            {
                xOffSet = 0;
            }
            else
            {
                xOffSet = solitaire.xOffsetIncrement;
            }
            GameObject newCard = Instantiate(cardPrefab,
                         new Vector3(drawCardLocation.transform.position.x + xOffSet,
                         drawCardLocation.transform.position.y,
                         drawCardLocation.transform.position.z + zOffSet),
                         Quaternion.identity,
                         drawCardLocation.transform);
            newCard.name = deck.Last<string>();
            newCard.GetComponent<CardInfo>().faceUp = true;
            if (j != 1) // if there's more cards to draw disable hitbox
            {
                newCard.tag = "Staggered Discard Pile";
            }
            else
            {
                newCard.tag = "Face Up Discard Pile";
            }
            discardPileList.Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
            topOfDiscardPile = newCard;
            drawCardLocation = newCard;
            drawCardLocation.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (deck.Count == 0)
        {
            topOfDeck.enabled = false;
        }
    }

    public void HideDiscardPile()
    {
        if (!isDeckAndDiscardPileEmpty)
        {
            topOfDeck.enabled = true;
            discardPile.transform.GetChild(0).gameObject.SetActive(false);
            hasCardBeenDrawn = false;
            Solitaire.SetCanUndo(true, "Deck");
        }
    }
    
    public void TurnOverDeck(List<string> deck) 
    {
        if (isDeckAndDiscardPileEmpty) { return; }
        for (int i = discardPileList.Count -1; i >= 0; i--)
        {
            deck.Add(discardPileList[i]);
        }

        discardPileList.Clear();

        if(discardPile.transform.childCount > 0)
        {
            Destroy(discardPile.transform.GetChild(0).gameObject);
            discardPile.transform.DetachChildren();
        }
    }

    /// <summary>
    /// Removes the most recently added discardPile card and sets the previous card as the top of the discard pile
    /// </summary>
    /// <param name="originalParent"></param>
    public void PlayCardFromDiscardPile(GameObject originalParent)
    {
        topOfDiscardPile = originalParent;
        originalParent.tag = "Face Up Discard Pile";
        discardPileList.RemoveAt(discardPileList.Count - 1); //Remove the last card from the list rather than searching for the card in the list
        if(discardPileList.Count + solitaire.deck.Count == 0)
        {
            isDeckAndDiscardPileEmpty = true;
            replayIcon.enabled = false;
        }
    }

    public void UndoDraw()
    {
        if(Options.drawAmount == 1)
        {
            if (hasCardBeenDrawn)
            {
                topOfDiscardPile = solitaire.GetLastChild(discardPile).transform.parent.gameObject;
                topOfDiscardPile.GetComponent<BoxCollider2D>().enabled = true;
                solitaire.deck.Add(solitaire.GetLastChild(discardPile).name);
                Destroy(solitaire.GetLastChild(discardPile));
                discardPileList.RemoveAt(discardPileList.Count - 1);

                if (!topOfDeck.enabled)
                {
                    topOfDeck.enabled = true;
                }
            }
            else
            {
                hasCardBeenDrawn = true;
                topOfDeck.enabled = false;
                discardPile.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else if(Options.drawAmount == 3)
        {
            if (hasCardBeenDrawn)
            {
                for(int i = 0; i < 3; i++)
                {
                    solitaire.deck.Add(solitaire.GetLastChild(discardPile).name);
                    Destroy(solitaire.GetLastChild(discardPile));
                    solitaire.GetLastChild(discardPile).transform.parent.DetachChildren();
                    discardPileList.RemoveAt(discardPileList.Count - 1);
                }

                topOfDiscardPile = solitaire.GetLastChild(discardPile);
                topOfDiscardPile.GetComponent<BoxCollider2D>().enabled = true;

                if (!topOfDeck.enabled)
                {
                    topOfDeck.enabled = true;
                }

                if (discardPileList.Count > 2)
                {
                    solitaire.GetLastChild(discardPile).transform.parent.position += new Vector3(xOffSet, 0, 0);
                    solitaire.GetLastChild(discardPile).transform.position += new Vector3(xOffSet, 0, 0);
                }
                else if (discardPileList.Count == 2)
                {
                    solitaire.GetLastChild(discardPile).transform.position += new Vector3(xOffSet, 0, 0);
                }
            }
            else
            {
                hasCardBeenDrawn = true;
                topOfDeck.enabled = false;
                discardPile.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void UndoMoveFromDiscardPile(GameObject cardMoved)
    {
        if (Options.drawAmount == 3)
        {
            if(discardPileList.Count == 0)
            {
                cardMoved.GetComponent<DragDrop>().SetPositionOnTop(topOfDiscardPile);
            }
            else
            {
                cardMoved.GetComponent<DragDrop>().SetPositionStaggeredHorizontal(topOfDiscardPile);
            }
        }
        else if (Options.drawAmount == 1)
        {
            cardMoved.GetComponent<DragDrop>().SetPositionOnTop(topOfDiscardPile);
        }

        //stop the previous card from being dragged
        if(topOfDiscardPile != discardPile)
        {
            topOfDiscardPile.tag = "Staggered Discard Pile";
        }

        //Add the card back to the discardpilelist
        discardPileList.Add(solitaire.GetLastChild(discardPile).name);
        cardMoved.tag = "Face Up Discard Pile";
        topOfDiscardPile = cardMoved;
        isDeckAndDiscardPileEmpty = false;
        replayIcon.enabled = true;
    }
}
