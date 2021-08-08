using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputListener : MonoBehaviour
{
    public GameObject cheatBox;

    // Start is called before the first frame update
    void Start()
    {
        //cheatBox = GameObject.Find("CheatInput");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.D) ||
           Input.GetKey(KeyCode.RightAlt) && Input.GetKey(KeyCode.D) || 
           Input.GetKey(KeyCode.AltGr) && Input.GetKey(KeyCode.D))
        {
            cheatBox.SetActive(true);
        }
    }
}
