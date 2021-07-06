using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugGetCard : MonoBehaviour
{
    Solitaire solitaire;
    DeckButton deckButton;
    public GameObject cardPrefab;
    public GameObject Canvas;

    public InputField inputField;

    static char[] suits = new char[] { 'C', 'D', 'H', 'S' };
    static char[] values = new char[] { 'A', '2', '3', '4', '5', '6', '7', '8', '9', '1', 'J', 'Q', 'K' };

    // Start is called before the first frame update
    void Start()
    {
        solitaire = GameObject.Find("SolitaireGame").GetComponent<Solitaire>();
        deckButton = GameObject.Find("DeckArea").GetComponent<DeckButton>();
        Canvas = GameObject.Find("Canvas");
    }

    public void MakeCard() //called when text input field is submitted by hitting enter or clicking off the field
    {
        //What should we do?
        string name = inputField.textComponent.text.ToUpper();
        if (name == "END") { DebugEndGame(); return; } //Deal cards in order with last card face down
        if (name == "END1") { DebugEndGame(false); return; } //Deal cards in order with last cards face up
        if (name == "END2") { DebugEndGame(true); return; } //Deal cards in order and then autocomplete
        if (name == "END3") { DebugEndGame(true); //Deal cards in order super quick and autocomplete super quick
                              solitaire.autoCompleteCardMoveSpeed = 0.01f;
                              solitaire.winAnimationcardForceMin = 500000;
                              solitaire.winAnimationCardForceMax = 500000; return; }

        //Check input is valid
        if (name == "" || name.Length == 1) { return; }

        bool firstCharIsCorrect = false;
        bool secondCharIsCorrect = false;

        char firstChar = name[0];
        char secondChar = name[1];

        foreach (char c in suits)
        {
            if (firstChar == c)
            {
                firstCharIsCorrect = true;
            }
        }

        foreach (char c in values)
        {
            if (secondChar == c)
            {
                secondCharIsCorrect = true;
            }
        }

        //check for the requested card in the deck
        foreach (string card in solitaire.deck)
        {
            if(card == name)
            {
                Debug.Log("card found in deck");
                solitaire.deck.Remove(name);
                break;
            }
        }

        //check for the requested card in the discard pile
        foreach (string card in deckButton.discardPileList)
        {
            if(card == name)
            {
                Debug.Log("card found in discard pile");
                List<GameObject> discardedCards = new List<GameObject>();
                GameObject tempDiscardedCard = deckButton.discardPile;
                while(tempDiscardedCard.transform.childCount > 0)
                {
                    discardedCards.Add(tempDiscardedCard.transform.GetChild(0).gameObject);
                    tempDiscardedCard = tempDiscardedCard.transform.GetChild(0).gameObject;
                }

                foreach(GameObject discardedCard in discardedCards)
                {
                    if(discardedCard.name == name)
                    {
                        GameObject parent = discardedCard.transform.parent.gameObject;
                        if (discardedCard.transform.childCount > 0)
                        {
                            GameObject child = discardedCard.transform.GetChild(0).gameObject;
                            child.transform.SetParent(parent.transform, false);
                            child.transform.position = discardedCard.transform.position;
                        }
                        else
                        {
                            parent.GetComponent<BoxCollider2D>().enabled = true;
                            parent.tag = "Face Up Discard Pile";
                            deckButton.topOfDiscardPile = parent;
                        }
                        Destroy(discardedCard);
                    }
                }
                deckButton.discardPileList.Remove(name);
                Solitaire.SetCanUndo(false);
                break;
            }
        }

        //check for the requested card in the playArea
        List<GameObject> playAreaCards = new List<GameObject>();
        
        foreach (GameObject playArea in solitaire.playArea)
        {
            if(playArea.transform.childCount > 0)
            {
                GameObject tempCard = playArea.transform.GetChild(0).gameObject;
                while (tempCard.transform.childCount > 0)
                {
                    playAreaCards.Add(tempCard);
                    tempCard = tempCard.transform.GetChild(0).gameObject;
                }
                playAreaCards.Add(tempCard);
            }
        }

        foreach (GameObject card in playAreaCards)
        {
            if(card.name == name)
            {
                Debug.Log("Card found in play area");
                GameObject parent = card.transform.parent.gameObject;
                if (card.transform.childCount > 0)
                {
                    GameObject child = card.transform.GetChild(0).gameObject;
                    child.transform.SetParent(parent.transform, false);
                    child.transform.position = card.transform.position;
                }
                else
                {
                    parent.GetComponent<BoxCollider2D>().enabled = true;
                    parent.tag = "Face Up Play Area";
                }
                Destroy(card);
                Solitaire.SetCanUndo(false);
                break;
            }
        }

        //No need to check for the card in the goal area

        //Make the card
        if (firstCharIsCorrect && secondCharIsCorrect)
        {
            GameObject newCard = Instantiate(cardPrefab,
                                 new Vector3(transform.position.x + 5,
                                 transform.position.y,
                                 transform.position.z + 1),
                                 Quaternion.identity,
                                 Canvas.transform);
            newCard.transform.localScale = new Vector3(1, 1, 1);

            newCard.name = name;
            newCard.GetComponent<CardInfo>().faceUp = true;
            newCard.tag = "Face Up Play Area";

            //Remove text from field
            inputField.Select();
            inputField.text = "";
        }
        else
        {
            inputField.Select();
            inputField.text = "Format: H10 or SA";
        }
    }

    void DebugEndGame()
    {
        solitaire.EndAllCoroutines();

        //clear the discard pile and deck
        if (deckButton.discardPile.transform.childCount > 0)
        {
            Destroy(deckButton.discardPile.transform.GetChild(0).gameObject);
            deckButton.transform.DetachChildren();
        }
        deckButton.discardPileList.Clear();
        solitaire.deck.Clear();

        //clear the play areas
        foreach (GameObject area in solitaire.playArea)
        {
            if (area.transform.childCount > 0)
            {
                Destroy(area.transform.GetChild(0).gameObject);
                area.transform.DetachChildren();
            }
        }

        //clear the goal areas
        foreach (GameObject area in solitaire.goalArea)
        {
            if (area.transform.childCount > 0)
            {
                Destroy(area.transform.GetChild(0).gameObject);
                area.transform.DetachChildren();
            }
        }

        //clear the playCards lists
        foreach (List<string> playCardList in solitaire.playCards)
        {
            playCardList.Clear();
        }

        //Make a new deck and deal it in order
        List<string> newDeck = Solitaire.GenerateDeckInValueOrder();
        solitaire.DebugSolitaireSort(newDeck);
        //StartCoroutine(solitaire.DebugDealCardsInOrder());

        //Disable the deck button
        deckButton.isDeckAndDiscardPileEmpty = true;
        deckButton.replayIcon.enabled = false;
        deckButton.topOfDeck.enabled = false;

        //Disable the undo button
        Solitaire.SetCanUndo(false);

        StartCoroutine(DealAndThenDisableAutoComplete());  
    }

    IEnumerator DealAndThenDisableAutoComplete()
    {
        yield return StartCoroutine(solitaire.DebugDealCardsInOrder(true));
        Solitaire.undoButton.SetActive(false); //TODO work out why this doesn't deactivate the button
    }

    void DebugEndGame(bool shouldAutoComplete)
    {
        solitaire.EndAllCoroutines();

        //clear the discard pile and deck
        if(deckButton.discardPile.transform.childCount > 0)
        {
            Destroy(deckButton.discardPile.transform.GetChild(0).gameObject);
            deckButton.transform.DetachChildren();
        }
        deckButton.discardPileList.Clear();
        solitaire.deck.Clear();

        //clear the play areas
        foreach(GameObject area in solitaire.playArea)
        {
            if(area.transform.childCount > 0)
            {
                Destroy(area.transform.GetChild(0).gameObject);
                area.transform.DetachChildren();
            }
        }

        //clear the goal areas
        foreach (GameObject area in solitaire.goalArea)
        {
            if (area.transform.childCount > 0)
            {
                Destroy(area.transform.GetChild(0).gameObject);
                area.transform.DetachChildren();
            }
        }

        //clear the playCards lists
        foreach(List<string> playCardList in solitaire.playCards)
        {
            playCardList.Clear();
        }

        //Make a new deck and deal it in order
        List<string> newDeck = Solitaire.GenerateDeckInValueOrder();
        solitaire.DebugSolitaireSort(newDeck);
        //StartCoroutine(solitaire.DebugDealCardsInOrder());

        //Disable the deck button
        deckButton.isDeckAndDiscardPileEmpty = true;
        deckButton.replayIcon.enabled = false;
        deckButton.topOfDeck.enabled = false;

        //Disable the undo button
        Solitaire.SetCanUndo(false);

        //Enable autocomplete button
        solitaire.autoCompleteButton.SetActive(true);

        if (shouldAutoComplete)
        {
            StartCoroutine(WaitAndThenAutoComplete());
        }
        else
        {
            StartCoroutine(solitaire.DebugDealCardsInOrder(false));
        }
    }

    IEnumerator WaitAndThenAutoComplete()
    {
        yield return StartCoroutine(solitaire.DebugDealCardsInOrder(false));
        solitaire.AutoComplete();
    }
}
