using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerClickHandler
{
    Vector3 originalPosition;
    GameObject originalParent;
    Vector3 MousePosition;
    Vector3 MouseOffset;

    public bool preventDragging = false;
    bool isDragging = false;
    bool isOverLapping = false;

    GameObject overlappedObject = null;
    public GameObject canvas;
    DeckButton deckButton;

    Solitaire solitaire;
    CardInfo thisCard;

    float lastEndDragTime; //attempt to stop multiple enddrag calls

    bool isAutoCompletePossible = false;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        solitaire = GameObject.Find("SolitaireGame").GetComponent<Solitaire>();
        thisCard = GetComponent<CardInfo>();
        deckButton = GameObject.Find("DeckArea").GetComponent<DeckButton>();
        lastEndDragTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            if (Input.GetMouseButton(0))
            {
                //move card to mouse position offset by the distance of the mouse from the centre of the card
                transform.position = new Vector2(Input.mousePosition.x - MouseOffset.x, Input.mousePosition.y - MouseOffset.y);
            }
        }
    }

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        int clickCount = pointerEventData.clickCount;
        int tapCount = Input.GetTouch(0).tapCount;

        if (clickCount == 2 || tapCount == 2)
        {
            StartCoroutine(DoubleClick());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverLapping = true;
        overlappedObject = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject == overlappedObject)
        {
            isOverLapping = false; //There's a lot of 
            overlappedObject = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) //TODO May be able to remove OnCollisionEnter and Exit now
    {
        overlappedObject = collision.gameObject;
    }

    public IEnumerator DoubleClick()
    {
        isDragging = false;
        string optionHit;
        if(gameObject.transform.childCount > 0) { yield break; }
        originalParent = transform.parent.gameObject;
        List<GameObject> goalAreas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Empty Goal Area"));
        List<GameObject> goalAreaCards = new List<GameObject>();

        foreach (GameObject goalArea in goalAreas)
        {
            goalAreaCards.Add(solitaire.GetLastChild(goalArea));
        }

        foreach (GameObject potentialGoalCard in goalAreaCards)
        {
            if (potentialGoalCard.CompareTag("Face Up Goal Area"))
            {
                if (potentialGoalCard.GetComponent<CardInfo>().value == thisCard.value - 1
                  && potentialGoalCard.GetComponent<CardInfo>().suit == thisCard.suit)
                {
                    optionHit = "DoubleClick1";
                    Debug.Log(thisCard.tag + " " + thisCard.name + " was dropped on: " + potentialGoalCard.tag + " " + potentialGoalCard.name +
                            "\n Held card came from: " + originalParent.tag + " " + originalParent.name + " option hit was: " + optionHit);

                    potentialGoalCard.GetComponent<BoxCollider2D>().enabled = false;
                    yield return StartCoroutine(solitaire.MoveCardToPointDoubleClick(transform, potentialGoalCard, 0, 0, solitaire.cardMoveSpeed));

                    if (thisCard.CompareTag("Face Up Discard Pile"))
                    {
                        deckButton.PlayCardFromDiscardPile(originalParent);
                        AutoCompleteCheck(); //Check for autocomplete here because playing from the play area will not make it possible
                    }

                    if (originalParent.transform.childCount < 1)
                    {
                        originalParent.GetComponent<BoxCollider2D>().enabled = true;
                    }

                    tag = "Face Up Goal Area";

                    //Setup Undo
                    solitaire.previousParent = originalParent;
                    solitaire.cardMoved = this.gameObject;
                    Solitaire.SetCanUndo(true, "CardMove");

                    if (thisCard.value == 13) // maybe we've won the game
                    {
                        if (solitaire.GameWinCheck())
                        {
                            yield return null;
                        }
                    }

                    yield break;
                }
            }
            else if (potentialGoalCard.tag == "Empty Goal Area")
            {
                // Check if the card is an Ace (1)
                if (thisCard.value == 1)
                {
                    optionHit = "DoubleClick2";
                    Debug.Log(thisCard.tag + " " + thisCard.name + " was dropped on: " + potentialGoalCard.tag + " " + potentialGoalCard.name +
                                "\n Held card came from: " + originalParent.tag + " " + originalParent.name + " option hit was: " + optionHit);

                    potentialGoalCard.GetComponent<BoxCollider2D>().enabled = false;
                    yield return StartCoroutine(solitaire.MoveCardToPointDoubleClick(transform, potentialGoalCard, 0, 0, solitaire.cardMoveSpeed));

                    if (thisCard.CompareTag("Face Up Discard Pile"))
                    {
                        deckButton.PlayCardFromDiscardPile(originalParent);
                        AutoCompleteCheck();
                    }

                    if (originalParent.transform.childCount < 1)
                    {
                        originalParent.GetComponent<BoxCollider2D>().enabled = true;
                    }

                    tag = "Face Up Goal Area";

                    //Setup Undo
                    solitaire.previousParent = originalParent;
                    solitaire.cardMoved = this.gameObject;
                    Solitaire.SetCanUndo(true, "CardMove");

                    yield break;
                }
            }
        }

    }

    public IEnumerator AutoCompleteDoubleClick(float moveSpeed)
    {
        isDragging = false;
        if (this.gameObject.transform.childCount > 0) { yield break; }
        originalParent = transform.parent.gameObject;
        List<GameObject> goalAreas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Empty Goal Area"));
        List<GameObject> goalAreaCards = new List<GameObject>();

        foreach (GameObject goalArea in goalAreas)
        {
            GameObject goalAreaToBeChecked = solitaire.GetLastChild(goalArea);
            goalAreaCards.Add(goalAreaToBeChecked);
        }

        foreach (GameObject potentialGoalCard in goalAreaCards)
        {
            //TODO check if the card is an Ace so it can be put in an empty area
            if (potentialGoalCard.CompareTag("Face Up Goal Area"))
            {
                if (potentialGoalCard.GetComponent<CardInfo>().value == thisCard.value - 1
                  && potentialGoalCard.GetComponent<CardInfo>().suit == thisCard.suit)
                {
                    overlappedObject = potentialGoalCard;
                    tag = "Face Up Goal Area";
                    yield return StartCoroutine(solitaire.MoveCardToPointDoubleClick(transform, overlappedObject, 0, 0, moveSpeed));

                    if (solitaire.isGameWon)
                    {
                        yield break;
                    }

                    if (thisCard.tag == "Face Up Discard Pile")
                    {
                        deckButton.PlayCardFromDiscardPile(originalParent);
                    }

                    if (originalParent.transform.childCount < 1)
                    {
                        originalParent.GetComponent<BoxCollider2D>().enabled = true;
                    }

                    break;
                }
            }
            else if (potentialGoalCard.tag == "Empty Goal Area")
            {
                // Check if the card is an Ace (1)
                if (thisCard.value == 1)
                {
                    overlappedObject = potentialGoalCard;
                    yield return StartCoroutine(solitaire.MoveCardToPointDoubleClick(transform, overlappedObject, 0, 0, moveSpeed));

                    if (thisCard.tag == "Face Up Discard Pile")
                    {
                        deckButton.PlayCardFromDiscardPile(originalParent);
                    }

                    if (originalParent.transform.childCount < 1)
                    {
                        originalParent.GetComponent<BoxCollider2D>().enabled = true;
                    }

                    tag = "Face Up Goal Area";

                    break;
                }
            }

        }
        yield return null;
    }

    //This is called by a card. See the card prefab in Unity for more info
    public void StartDragging() //TODO look into better click or drag detection? Test time mouse is held
    {
        if (Time.time - lastEndDragTime < 0.1f) { return; } //Prevent unwanted doubleclick functionality
        if (preventDragging) { return; }
        if (Input.GetMouseButton(0) && isDragging) //If the user right clicks while dragging, end dragging
        {
            EndDragging();
            return;
        }
        if (!thisCard.CompareTag("Face Up Play Area") && 
            !thisCard.CompareTag("Face Up Goal Area") && 
            !thisCard.CompareTag("Face Up Discard Pile")) { return; }

        //save mouse position for later to offset mouse from the centre of the dragged card
        //start drag has a delay after clicking before becoming active
        MousePosition = Input.mousePosition;
        MouseOffset.x = Input.mousePosition.x - transform.position.x;
        MouseOffset.y = Input.mousePosition.y - transform.position.y;

        originalPosition = transform.position;
        originalParent = transform.parent.gameObject;
        transform.SetParent(canvas.transform, true);
        GetComponent<BoxCollider2D>().enabled = true;
        isDragging = true;
    }

    public void EndDragging()
    {
        if (!isDragging) { return; }
        if (overlappedObject == null) { isDragging = false; ReturnToGrabPostion(); return; }
        int optionHit;

        if (Time.time - lastEndDragTime < 0.1f) { return; } //TODO work out why this method is called twice when dropping a card

        if (!thisCard.CompareTag("Face Up Play Area") && !thisCard.CompareTag("Face Up Goal Area") && !thisCard.CompareTag("Face Up Discard Pile")) { return; }
        isDragging = false;
        if (isOverLapping) //TODO make sure we turn off BoxCollider2D when we don't want something to be dropped on and only change tags on area change
        {
            if (overlappedObject != canvas && overlappedObject.tag != "Untagged")
            {
                if (overlappedObject.tag == "Face Up Play Area") 
                {
                    // Check if the card if the held card is 1 lower and the opposite suit colour
                    if (overlappedObject.GetComponent<CardInfo>().value == thisCard.value + 1 
                        && overlappedObject.GetComponent<CardInfo>().suitColour != thisCard.suitColour)
                    {
                        SetPositionStaggeredVertical(overlappedObject);

                        if (originalParent.transform.childCount < 1)
                        {
                            originalParent.GetComponent<BoxCollider2D>().enabled = true;
                        }

                        GameObject tempOverlappedObject = overlappedObject; //save the game object so when it gets nulled by enable we still have it
                        overlappedObject.GetComponent<BoxCollider2D>().enabled = false; //this sets the overlapped to null :(
                        overlappedObject = tempOverlappedObject; //TODO find a way to not need to work around this

                        if (CompareTag("Face Up Discard Pile"))
                        {
                            deckButton.PlayCardFromDiscardPile(originalParent);
                        }

                        if (transform.childCount != 0)
                        {
                            GetComponent<BoxCollider2D>().enabled = false; //TODO this makes something null
                        }

                        tag = "Face Up Play Area";

                        //Setup Undo
                        solitaire.previousParent = originalParent;
                        solitaire.cardMoved = this.gameObject;
                        Solitaire.SetCanUndo(true, "CardMove");


                        optionHit = 1;
                    }
                    else
                    {
                        ReturnToGrabPostion();

                        optionHit = 2;
                    }
                }
                else if (overlappedObject.tag == "Face Up Goal Area")
                {
                    // Check if the held card is the same suit and 1 value higher
                    if (overlappedObject.GetComponent<CardInfo>().value == thisCard.value - 1
                        && overlappedObject.GetComponent<CardInfo>().suit == thisCard.suit)
                    {
                        SetPositionOnTop(overlappedObject);

                        if (thisCard.tag == "Face Up Discard Pile")
                        {
                            deckButton.PlayCardFromDiscardPile(originalParent);
                        }

                        GameObject tempOverlappedObject = overlappedObject; //save the game object so when it gets nulled by enable we still have it
                        overlappedObject.GetComponent<BoxCollider2D>().enabled = false; //this sets the overlapped to null :(
                        overlappedObject = tempOverlappedObject; //TODO find a way to not need to work around this

                        if (originalParent.transform.childCount < 1)
                        {
                            originalParent.GetComponent<BoxCollider2D>().enabled = true;
                        }

                        tag = "Face Up Goal Area";

                        if (thisCard.value == 13) // maybe we've won the game
                        {
                            if (solitaire.GameWinCheck())
                            {
                                optionHit = 30;
                                Debug.Log(thisCard.tag + " " + thisCard.name + " was dropped on: " + overlappedObject.tag + " " + overlappedObject.name +
                                "\n Held card came from: " + originalParent.tag + " " + originalParent.name + " option hit was: " + optionHit);
                                return;
                            }
                        }

                        //Setup Undo
                        solitaire.previousParent = originalParent;
                        solitaire.cardMoved = this.gameObject;
                        Solitaire.SetCanUndo(true, "CardMove");

                        optionHit = 3;
                    }
                    else
                    {
                        ReturnToGrabPostion();

                        optionHit = 4;
                    }
                }
                else if (overlappedObject.tag == "Empty Play Area")
                {
                    // Check if the held card is a King (13)
                    if(thisCard.value == 13 || Options.canPlaceAnyCardInEmptySpace == true)
                    {
                        SetPositionOnTop(overlappedObject);

                        GameObject tempOverlappedObject = overlappedObject; //save the game object so when it gets nulled by enable we still have it
                        overlappedObject.GetComponent<BoxCollider2D>().enabled = false; //this sets the overlapped to null :(
                        overlappedObject = tempOverlappedObject; //TODO find a way to not need to work around this

                        if (transform.childCount != 0)
                        {
                            GetComponent<BoxCollider2D>().enabled = false; //TODO this makes something null
                        }

                        if (thisCard.tag == "Face Up Discard Pile")
                        {
                            deckButton.PlayCardFromDiscardPile(originalParent);
                        }
                        else
                        {
                            if (originalParent.transform.childCount < 1)
                            {
                                originalParent.GetComponent<BoxCollider2D>().enabled = true;
                            }
                        }

                        tag = "Face Up Play Area";

                        //Setup Undo
                        solitaire.previousParent = originalParent;
                        solitaire.cardMoved = this.gameObject;
                        Solitaire.SetCanUndo(true, "CardMove");

                        optionHit = 5;
                    }
                    else
                    {
                        ReturnToGrabPostion();

                        optionHit = 6;
                    }

                }
                else if (overlappedObject.tag == "Empty Goal Area")
                {
                    // Check if the card is an Ace (1)
                    if (thisCard.value == 1)
                    {
                        SetPositionOnTop(overlappedObject);

                        if (thisCard.tag == "Face Up Discard Pile")
                        {
                            deckButton.PlayCardFromDiscardPile(originalParent);
                        }

                        if (originalParent.transform.childCount < 1)
                        {
                            originalParent.GetComponent<BoxCollider2D>().enabled = true;
                        }

                        tag = "Face Up Goal Area";

                        GameObject tempOverlappedObject = overlappedObject; //save the game object so when it gets nulled by enable we still have it
                        overlappedObject.GetComponent<BoxCollider2D>().enabled = false; //this sets the overlapped to null :(
                        overlappedObject = tempOverlappedObject; //TODO find a way to not need to work around this

                        //Setup Undo
                        solitaire.previousParent = originalParent;
                        solitaire.cardMoved = this.gameObject;
                        Solitaire.SetCanUndo(true, "CardMove");

                        optionHit = 7;
                    }
                    else
                    {
                        ReturnToGrabPostion();

                        optionHit = 8;
                    }
                }
                else
                {
                    Debug.Log("We shouldn't ever get to this point? Tag is " + tag);
                    ReturnToGrabPostion();

                    optionHit = 9;
                }
            }
            else
            {
                ReturnToGrabPostion();

                optionHit = 10;
            }
        }
        else
        {
            ReturnToGrabPostion();

            optionHit = 11;
        }

        if (overlappedObject == null) 
        {
            Debug.Log(thisCard.tag + " " + thisCard.name + " was dropped on: [OverlappedObject is null]" +
            "\n Held card came from: " + originalParent.tag + " " + originalParent.name + " option hit was: " + optionHit);
        }
        else if (originalParent == null)
        {
            Debug.Log(thisCard.tag + " " + thisCard.name + " was dropped on: " + overlappedObject.tag + " " + overlappedObject.name +
            "\n Held card came from: [originalParent is null]" + " option hit was: " + optionHit);

        }
        else
        {
            Debug.Log(thisCard.tag + " " + thisCard.name + " was dropped on: " +  overlappedObject.tag + " " + overlappedObject.name +
            "\n Held card came from: " + originalParent.tag + " " + originalParent.name + " option hit was: " + optionHit);
        }

        AutoCompleteCheck();
        lastEndDragTime = Time.time;

    }

    public void SetPositionStaggeredVertical(GameObject newParent)
    {
        transform.SetParent(newParent.transform, true);
        transform.position = new Vector3(
            newParent.transform.position.x,
            newParent.transform.position.y - solitaire.yOffsetIncrement,
            newParent.transform.position.z + solitaire.zOffsetIncrement);
    }

    public void SetPositionStaggeredHorizontal(GameObject newParent)
    {
        transform.SetParent(newParent.transform, true);
        transform.position = new Vector3(
            newParent.transform.position.x + solitaire.xOffsetIncrement,
            newParent.transform.position.y,
            newParent.transform.position.z + solitaire.zOffsetIncrement);
    }

    public void SetPositionOnTop(GameObject newParent)
    {
        transform.SetParent(newParent.transform, true); //transform.SetParent(overlappedObject.transform, true);
        transform.position = new Vector3(
            newParent.transform.position.x,
            newParent.transform.position.y,
            newParent.transform.position.z + solitaire.zOffsetIncrement);
    }

    private void ReturnToGrabPostion()
    {
        if(transform.childCount > 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        transform.SetParent(originalParent.transform, true);
        transform.position = originalPosition;
    }

    public void AutoCompleteCheck()
    {
        if (isAutoCompletePossible) { return; }
        if (deckButton.isDeckAndDiscardPileEmpty) //check deck/discard pile are empty
        {
            //check each card in the play area is face up (we're assuming there will be 52 cards total and no other weirdness)
            List<GameObject> potentialPlayableCards = new List<GameObject>(GameObject.FindGameObjectsWithTag("Empty Play Area"));
            foreach(GameObject area in potentialPlayableCards)
            {
                if(area.transform.childCount > 0)
                {
                    GameObject childCard = area;
                    while(childCard.transform.childCount > 0)
                    {
                        if (!childCard.transform.GetChild(0).gameObject.GetComponent<CardInfo>().faceUp)
                        {
                            return; //A card is not face up so we can't auto complete
                        }

                        if(childCard.transform.childCount > 0)
                        {
                            childCard = childCard.transform.GetChild(0).gameObject;
                        }
                    }

                }
            }
            Debug.Log("Autocomplete is possible!!!");
            isAutoCompletePossible = true;
            solitaire.autoCompleteButton.SetActive(true);
            return;
        }
        return;
    }
}
