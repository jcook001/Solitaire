using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFunctions : MonoBehaviour
{
    Solitaire solitaire = null;
    public Camera mainCamera;
    public Text physicsButtonText;
    int physicsPressCount = 0;
    public static bool showTransparentCollision = false;

    public PhysicsMaterial2D cardMaterial;

    private void Start()
    {
        solitaire = GameObject.Find("SolitaireGame").GetComponent<Solitaire>();

        Debug.Log("Default gravity value: " + Physics2D.gravity);
    }

    public void toggleShowTransparentCollision()
    {
        showTransparentCollision = !showTransparentCollision;
    }

    public void ChangePhysics()
    {
        if (physicsPressCount == 0)
        {
            Physics2D.gravity = Vector2.zero;
            physicsButtonText.text = "No Gravity";
            physicsPressCount++;
        }
        else if (physicsPressCount == 1)
        {
            Physics2D.gravity = new Vector2(1, 1);
            physicsButtonText.text = "1 , 1";
            physicsPressCount++;
        }
        else if (physicsPressCount == 2)
        {
            Physics2D.gravity = new Vector2(0, 1);
            physicsButtonText.text = "0 , -1";
            physicsPressCount++;
        }
        else if (physicsPressCount == 3)
        {
            Physics2D.gravity = new Vector2(1, 0);
            physicsButtonText.text = "-10 , -10";
            physicsPressCount++;
        }
        else if (physicsPressCount == 4)
        {
            Physics2D.gravity = new Vector2(10, 10);
            physicsButtonText.text = "10 , 10";
            physicsPressCount = 0;
        }

    }

    public Text cameraButtonText;
    int cameraPressCount = 0;
    public void ChangeCameraUpdate()
    {
        if (cameraPressCount == 0)
        {
            mainCamera.clearFlags = CameraClearFlags.Nothing;
            cameraButtonText.text = "Nothing";
            cameraPressCount++;
        }
        else if (cameraPressCount == 1)
        {
            mainCamera.clearFlags = CameraClearFlags.Color;
            cameraButtonText.text = "Color";
            cameraPressCount++;
        }
        else if (cameraPressCount == 2)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            cameraButtonText.text = "SolidColor";
            cameraPressCount++;
        }
        else if (cameraPressCount == 3)
        {
            mainCamera.clearFlags = CameraClearFlags.Depth;
            cameraButtonText.text = "Depth";
            cameraPressCount++;
        }
        else if (cameraPressCount == 4)
        {
            mainCamera.clearFlags = CameraClearFlags.Skybox;
            cameraButtonText.text = "Skybox";
            cameraPressCount = 0;
        }

    }

    public void UnlockAllCards()
    {
        StartCoroutine(UnlockOneByOne());
    }

    public IEnumerator UnlockOneByOne()
    {
        List<GameObject> cardsInPlay = new List<GameObject>();

        foreach (GameObject area in solitaire.playArea)
        {
            if (area.transform.childCount > 0)
            {
                GameObject child = solitaire.GetLastChild(area);
                while (child != area)
                {
                    cardsInPlay.Add(child);
                    child = child.transform.parent.gameObject;
                }
            }
        }

        for (int i = 13; i > 0; i--)
        {
            foreach (GameObject area in solitaire.goalArea)
            {
                if (area.transform.childCount > 0)
                {
                    GameObject nextchild = area;
                    //Debug.Log("area is " + area);
                    //Debug.Log("nextchild is " + nextchild);
                    for(int j = i; j > 0; j--)
                    {
                        //Debug.Log(j);
                        nextchild = nextchild.transform.GetChild(0).gameObject; //Child out of bounds?
                    }
                    Debug.Log("card to be added is " + nextchild);
                    cardsInPlay.Add(nextchild);
                    //yield return new WaitForSeconds(0.5f);
                }
            }
        }

        foreach(GameObject playArea in solitaire.playArea)
        {
            playArea.transform.DetachChildren();
            Destroy(playArea);
        }

        Destroy(GameObject.Find("DeckArea"));
        Destroy(GameObject.Find("DiscardPile"));

        foreach (GameObject card in cardsInPlay)
        {
            card.GetComponent<BoxCollider2D>().enabled = true;
            card.GetComponent<Rigidbody2D>().constraints = UnityEngine.RigidbodyConstraints2D.FreezeRotation;
            card.GetComponent<Rigidbody2D>().gravityScale = 300.0f;
            PhysicsMaterial2D newMaterial = new PhysicsMaterial2D("BouncyCardMaterial");
            newMaterial.friction = 0.0f;
            newMaterial.bounciness = Random.Range(0.85f, 0.93f);
            card.GetComponent<BoxCollider2D>().sharedMaterial = newMaterial;
            card.transform.SetParent(solitaire.canvas.transform, true);
            int cardForce = Random.Range(12000, 16000);
            if (Random.value > 0.5)
            {
                cardForce = cardForce * -1;
            }
            card.GetComponent<Rigidbody2D>().AddForce(new Vector2(cardForce, 0));
            card.layer = 11;
            yield return new WaitForSeconds(2.0f);
        }

    }
}
