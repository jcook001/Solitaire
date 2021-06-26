using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string[] deck;
    public string[] discardpile;
    public string[] playArea0;
    public string[] playArea1;
    public string[] playArea2;
    public string[] playArea3;
    public string[] playArea4;
    public string[] playArea5;
    public string[] playArea6;
    public string[] playArea7;
    public string[] goalArea0;
    public string[] goalArea1;
    public string[] goalArea2;
    public string[] goalArea3;

    public GameData (Solitaire solitaire)
    {

    }
}
