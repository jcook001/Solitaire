using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Solitaire : MonoBehaviour
{
    public bool HasGameStarted = false;
    public bool isGameWon = false;
    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public List<string> deck;
    public Sprite[] cardFaces;
    public Sprite[] cardFacesDark;
    public Sprite cardBack;
    public Sprite cardBackDark;

    public GameObject cardPrefab;

    private GameObject deckArea;
    private GameObject discardPile;
    public Canvas canvas;
    public Camera mainCamera;

    public GameObject[] goalArea;
    public List<string>[] goalCards;

    private GameObject deckButton;
    public GameObject restartButton;
    public GameObject closeGameButton;
    
    public GameObject[] playArea;
    public List<string>[] playCards;
    private List<string> play0 = new List<string>();
    private List<string> play1 = new List<string>();
    private List<string> play2 = new List<string>();
    private List<string> play3 = new List<string>();
    private List<string> play4 = new List<string>();
    private List<string> play5 = new List<string>();
    private List<string> play6 = new List<string>();

    public float xOffsetIncrement;
    public float yOffsetIncrement;
    public float yOffsetIncrementFaceDown;
    public float zOffsetIncrement = 1.0f;

    public DialogueManager dialogueManager;

    public Animator transition;

    public GameObject autoCompleteButton;
    public float autoCompleteCardMoveSpeed = 0.25f;
    public float cardMoveSpeed = 0.25f;
    public int winAnimationCardForceMax = 16000;
    public int winAnimationCardForceMin = 12000;
    public BoxCollider2D floor;

    public static bool canUndo = false;
    public static string lastAction;
    public GameObject previousParent;
    public GameObject cardMoved;
    public GameObject undoButtonTemp;
    public static GameObject undoButton;

    // Start is called before the first frame update
    void Start()
    {
        deckArea = GameObject.Find("DeckArea");
        deckButton = GameObject.Find("DeckArea");
        discardPile = GameObject.Find("DiscardPile");
        undoButton = undoButtonTemp;
        playCards = new List<string>[] { play0, play1, play2, play3, play4, play5, play6 };
        xOffsetIncrement = xOffsetIncrement * Screen.width / 800;
        yOffsetIncrement = yOffsetIncrement * Screen.height / 600;

        //Autostart game
        PlayCards();
        HasGameStarted = true;
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        int i = 0;
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                newDeck.Add(s + v);
                i++;
            }
        }

        return newDeck;
    }

    public static List<string> GenerateDeckInValueOrder()
    {
        List<string> newDeck = new List<string>();
        int i = 0;
        foreach (string v in values)
        {
            foreach (string s in suits)
            {
                newDeck.Add(s + v);
                i++;
            }
        }

        return newDeck;
    }

    public void PlayCards()
    {
        if (HasGameStarted)
        {
            return;
        }

        deck = GenerateDeck();

        ShuffleCards(deck);

        SolitaireSort(deck);

        StartCoroutine(DealCards(deck));

    }

    public void ShuffleCards(List<string> unshuffledCards)
    {
        for (int i = 0; i < unshuffledCards.Count; i++)
        {
            string temp = unshuffledCards[i];
            int randomIndex = Random.Range(i, unshuffledCards.Count);
            unshuffledCards[i] = unshuffledCards[randomIndex];
            unshuffledCards[randomIndex] = temp;
        }
    }

    IEnumerator DealCards(List<string> shuffledCards)
    {
        //TODO make cards deal left to right rather than colmun by column

        GameObject previouscard = null;

        for (int i = 0; i < 7; i++) // i is the colmun being dealt to
        {
            float yOffset = 0.0f;
            float zOffset = 0.0f;

            foreach (string card in playCards[i])
            {
                GameObject newCard = Instantiate(cardPrefab, 
                                                 new Vector3(deckArea.transform.position.x, 
                                                 deckArea.transform.position.y, 
                                                 deckArea.transform.position.z), 
                                                 Quaternion.identity, 
                                                 playArea[i].transform);
                StartCoroutine(MoveCardToPoint(newCard.transform, playArea[i], yOffset, zOffset, 0.3f));

                if (playArea[i].transform.childCount > 1)
                {
                    newCard.transform.SetParent(previouscard.transform);
                }

                newCard.name = card;
                if (card == playCards[i][playCards[i].Count -1])
                {
                    newCard.GetComponent<CardInfo>().faceUp = true;
                    newCard.tag = "Face Up Play Area";
                }
                else
                {
                    newCard.tag = "Face Down Play Area";
                    //turn off colision so cards cannot be dropped on
                    newCard.GetComponent<BoxCollider2D>().enabled = false; //TODO maybe change this to IsTrigger instead?
                }

                previouscard = newCard;
                yOffset += yOffsetIncrementFaceDown;
                zOffset += zOffsetIncrement;

                yield return new WaitForSeconds(0.2f);

            }
            playArea[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        foreach (List<string> playArea in playCards)
        {
            playArea.Clear();
        }

    }

    public IEnumerator DebugDealCardsInOrder(bool includeFaceDownCards)
    {
        //TODO make cards deal left to right rather than colmun by column

        GameObject previouscard = null;

        for (int i = 0; i < 4; i++) // i is the colmun being dealt to
        {
            float yOffset = 0.0f;
            float zOffset = 0.0f;

            foreach (string card in playCards[i])
            {
                GameObject newCard = Instantiate(cardPrefab,
                                                 new Vector3(playArea[i].transform.position.x,
                                                 playArea[i].transform.position.y - yOffset,
                                                 playArea[i].transform.position.z + zOffset),
                                                 Quaternion.identity,
                                                 playArea[i].transform);
                //StartCoroutine(MoveCardToPoint(newCard.transform, playArea[i], yOffset, zOffset, 0.3f));

                if (playArea[i].transform.childCount > 1)
                {
                    newCard.transform.SetParent(previouscard.transform);
                }

                newCard.name = card;
                if (card == playCards[i][playCards[i].Count - 1])
                {
                    if (includeFaceDownCards)
                    {
                        newCard.tag = "Face Down Play Area";
                        //turn off colision so cards cannot be dropped on
                        newCard.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    else
                    {
                        newCard.GetComponent<CardInfo>().faceUp = true;
                        newCard.tag = "Face Up Play Area";
                    }
                }
                else
                {
                    newCard.tag = "Face Up Play Area";
                    newCard.GetComponent<CardInfo>().faceUp = true;
                    //turn off colision so cards cannot be dropped on
                    newCard.GetComponent<BoxCollider2D>().enabled = false; //TODO maybe change this to IsTrigger instead?
                }

                previouscard = newCard;
                yOffset += yOffsetIncrement;
                zOffset += zOffsetIncrement;

                //yield return new WaitForSeconds(0.05f);
                yield return new WaitForSeconds(0);

            }
            playArea[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        foreach (List<string> playArea in playCards)
        {
            playArea.Clear();
        }
        foreach (GameObject playArea in playArea)
        {
            if(playArea.transform.childCount < 1)
            {
                playArea.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    public IEnumerator MoveCardToPoint(Transform objectToMove, GameObject endPosition, float yOffset, float zOffset, float timeToTake)
    {
        float t = 0;
        Vector3 originalPosition = objectToMove.position;
        objectToMove.transform.SetParent(endPosition.transform, true);
        objectToMove.gameObject.GetComponent<DragDrop>().preventDragging = true;
        while (t < 1)
        {
            t += Time.deltaTime / timeToTake;
            objectToMove.position = Vector3.Lerp(originalPosition, new Vector3 (endPosition.transform.position.x, 
                                                                                endPosition.transform.position.y - yOffset,
                                                                                endPosition.transform.position.z + zOffset), t);
            yield return null;
        }
        objectToMove.gameObject.GetComponent<DragDrop>().preventDragging = false;
    }

    public IEnumerator MoveCardToPointDoubleClick(Transform objectToMove, GameObject endPosition, float yOffset, float zOffset, float timeToTake)
    {
        float t = 0;
        Vector3 originalPosition = objectToMove.position;
        objectToMove.transform.SetParent(canvas.transform, true); //make the card appear on top while moving
        objectToMove.gameObject.GetComponent<DragDrop>().preventDragging = true;
        while (t < 1)
        {
            t += Time.deltaTime / timeToTake;
            objectToMove.position = Vector3.Lerp(originalPosition, new Vector3(endPosition.transform.position.x,
                                                                                endPosition.transform.position.y - yOffset,
                                                                                endPosition.transform.position.z + zOffset), t);
            yield return null;
        }

        endPosition.transform.DetachChildren();
        objectToMove.transform.SetParent(endPosition.transform, true);
        objectToMove.gameObject.GetComponent<DragDrop>().preventDragging = false;
        if (objectToMove.gameObject.GetComponent<CardInfo>().value == 13)
        {
            if (GameWinCheck())
            {
                yield return null;
            }
        }
    }

    public void SolitaireSort(List<string> cards) //TODO incoroprate DealCards() into here to try and deal row by row
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = i; j < 7; j++)
            {
                playCards[j].Add(cards.Last<string>());
                cards.RemoveAt(cards.Count - 1);
            }
        }
    }

    public void DebugSolitaireSort(List<string> cards)
    {
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                playCards[j].Add(cards.Last<string>());
                cards.RemoveAt(cards.Count - 1);
            }
        }
    }

    public bool GameWinCheck()
    {
        List<GameObject> goalAreas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Empty Goal Area"));
        List<GameObject> goalAreaCards = new List<GameObject>();

        foreach (GameObject goalArea in goalAreas)
        {
            goalAreaCards.Add(GetLastChild(goalArea));
        }

        foreach (GameObject card in goalAreaCards)
        {
            if(card.CompareTag("Face Up Goal Area"))
            {
                if (card.GetComponent<CardInfo>().value != 13)
                {
                    isGameWon = false;
                    return false;
                }
                else
                {
                    isGameWon = true;
                }
            }
            else
            {
                isGameWon = false;
                return false;
            }

        }

        if (isGameWon)
        {
            dialogueManager.WinGame();
            StartCoroutine(GameWinAnimation());
            return true;
        }

        return false;
    }

    private IEnumerator GameWinAnimation()
    {
        floor.enabled = true;

        mainCamera.clearFlags = CameraClearFlags.Nothing; //Get that sweet sweet cascading effect
        
        List<GameObject> cardsInPlay = new List<GameObject>();

        //Add cards into a list in the order we want them to drop eg. k, k, k, k, q, q, q...
        for (int i = 13; i > 0; i--)
        {
            foreach (GameObject area in goalArea)
            {
                area.GetComponent<Image>().enabled = false;
                if (area.transform.childCount > 0)
                {
                    //get the next child i times for each goal area
                    GameObject nextchild = area;
                    for (int j = i; j > 0; j--)
                    {
                        nextchild = nextchild.transform.GetChild(0).gameObject;
                    }
                    cardsInPlay.Add(nextchild);
                    nextchild.GetComponent<Image>().enabled = false;
                }
            }
        }

        foreach (GameObject playArea in playArea)
        {
            playArea.transform.DetachChildren();
            Destroy(playArea);
        }

        Destroy(GameObject.Find("DeckArea"));
        Destroy(GameObject.Find("DiscardPile"));
        Destroy(restartButton);
        Destroy(closeGameButton);
        Destroy(undoButton);
        Destroy(autoCompleteButton);

        foreach (GameObject card in cardsInPlay)
        {
            card.GetComponent<DragDrop>().preventDragging = true;
        }

        foreach (GameObject card in cardsInPlay)
        {
            card.GetComponent<Image>().enabled = true;
            card.GetComponent<BoxCollider2D>().enabled = true;
            card.GetComponent<Rigidbody2D>().constraints = UnityEngine.RigidbodyConstraints2D.FreezeRotation;
            card.GetComponent<Rigidbody2D>().gravityScale = 300.0f;
            PhysicsMaterial2D newMaterial = new PhysicsMaterial2D("BouncyCardMaterial");
            newMaterial.friction = 0.0f;
            newMaterial.bounciness = Random.Range(0.8f, 0.91f);
            card.GetComponent<BoxCollider2D>().sharedMaterial = newMaterial;
            card.transform.SetParent(canvas.transform, true);
            int cardForce = Random.Range(winAnimationCardForceMin, winAnimationCardForceMax);
            cardForce = cardForce * Screen.width/1920;
            if (Random.value > 0.5)
            {
                cardForce = cardForce * -1;
            }
            card.GetComponent<Rigidbody2D>().AddForce(new Vector2(cardForce, 0));
            card.layer = 11;
            yield return StartCoroutine(DestroyOffscreenCard(card));
        }

        //If they've waited to the end of the animation...
        //Destroy dialogue manager (or just close the dialogue)
        dialogueManager.ClosePrompt(true);

        //play end scene animation
        transition.SetTrigger("End"); //TODO make fade out animation longer

        yield return new WaitForSeconds(1.0f);

        //Restore camera settings
        mainCamera.clearFlags = CameraClearFlags.Skybox;

        while (!Input.anyKeyDown)
        {
            //wait for user to click
            yield return null; 
            
            //TODO OR wait for 10 seconds and show "click to continue..."
        }

        SceneManager.LoadScene("Menu");
    }

    private IEnumerator DestroyOffscreenCard(GameObject card)
    {
        float xBoundaryRight = Screen.width + (card.GetComponent<Image>().rectTransform.rect.width * 2);
        float xBoundaryLeft = (card.GetComponent<Image>().rectTransform.rect.width) * -2;

        while(card.transform.position.x < xBoundaryRight && card.transform.position.x > xBoundaryLeft)
        {
            yield return null;
        }
        Destroy(card);
        yield return null;
    }

    public void AutoComplete()
    {
        List<GameObject> cardsInPlayArea = new List<GameObject>(GameObject.FindGameObjectsWithTag("Face Up Play Area"));
        List<GameObject> cardsInGoalArea = new List<GameObject>(GameObject.FindGameObjectsWithTag("Face Up Goal Area"));
        List<GameObject> totalCardsInPlay = cardsInGoalArea;

        //Check that there are no more or less than 52 cards 
        foreach (GameObject card in cardsInPlayArea)
        {
            totalCardsInPlay.Add(card);
        }
        if (totalCardsInPlay.Count != 52) { return; }
        SetCanUndo(false);
        foreach (GameObject card in cardsInPlayArea)
        {
            card.GetComponent<DragDrop>().preventDragging = true;
        }
        StartCoroutine(AutoCompleteCoroutine());
        autoCompleteButton.SetActive(false);
    }

    public IEnumerator AutoCompleteCoroutine()
    {
        //Check if there are cards in the play area
        int playAreaChildCount = 0;
        foreach(GameObject area in playArea)
        {
            if(area.transform.childCount > 0)
            {
                playAreaChildCount += 1;
            }
        }

        List<GameObject> playAreaListObjects = new List<GameObject>(playArea);

        while (playAreaChildCount > 0)
        {
            foreach (GameObject dropZone in playAreaListObjects)
            {
                if (!GetLastChild(dropZone)){ break; } //it's possible to get here when dropzone is null so check for that
                if(GetLastChild(dropZone).CompareTag("Face Up Play Area"))
                {
                    yield return GetLastChild(dropZone).GetComponent<DragDrop>().AutoCompleteDoubleClick(autoCompleteCardMoveSpeed);
                }
            }

            int newPlayAreaChildCount = 0;

            foreach (GameObject area in playArea)
            {
                if(!area) { break; } //it's possible to get here when area is null so check for that
                if (area.transform.childCount > 0)
                {
                    newPlayAreaChildCount += 1;
                }
            }

            playAreaChildCount = newPlayAreaChildCount;
        }

    }

    /// <summary>
    /// Returns the bottom most child of a GameObject assuming each previous child only has one child
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject GetLastChild(GameObject parent)
    {
        GameObject badObject = null;
        if(!parent) { Debug.Log("Something is trying to get the last child of a null object!"); return badObject; }
        if (parent.transform.childCount < 1) { return parent; }
        GameObject lastChild = parent;
        while(lastChild.transform.childCount > 0)
        {
            lastChild = lastChild.transform.GetChild(0).gameObject;
        }
        return lastChild;
    }

    public void Undo()
    {
        Debug.Log("Undo has been pressed");
        if (!canUndo) { return; }

        if(lastAction == "Deck") //Deck button was clicked
        {
            Debug.Log("the last action was Deck click");
            deckButton.GetComponent<DeckButton>().UndoDraw();

        }
        else if(lastAction == "CardMove") //Card was dragged or double clicked
        {
            Debug.Log("the last action was card move");
            cardMoved.transform.parent.GetComponent<BoxCollider2D>().enabled = true;
            if (previousParent.CompareTag("Face Up Discard Pile"))
            {
                deckButton.GetComponent<DeckButton>().UndoMoveFromDiscardPile(cardMoved);
            }
            else if (previousParent.CompareTag("Face Up Play Area") || previousParent.CompareTag("Face Down Play Area"))
            {
                cardMoved.GetComponent<DragDrop>().SetPositionStaggeredVertical(previousParent);
                Debug.Log("set position staggered to " + previousParent);
            }
            else if (previousParent.CompareTag("Face Up Goal Area") || previousParent.CompareTag("Empty Play Area") || previousParent.CompareTag("Empty Goal Area"))
            {
                cardMoved.GetComponent<DragDrop>().SetPositionOnTop(previousParent);
                cardMoved.tag = "Face Up Goal Area";
                Debug.Log("set position on top to " + previousParent);
            }

            previousParent.GetComponent<BoxCollider2D>().enabled = false;
        }

        SetCanUndo(false);

    }

    public static void SetCanUndo(bool newBool, string action)
    {
        canUndo = newBool;
        undoButton.SetActive(newBool);
        lastAction = action;
    }
    public static void SetCanUndo(bool newBool)
    {
        canUndo = newBool;
        undoButton.SetActive(newBool);
    }

    public void EndAllCoroutines()
    {
        StopAllCoroutines();
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }
}
