using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static void SaveGame(Solitaire solitaire)
    {
        string path = Application.persistentDataPath + "/LastGameState.json";
        FileStream stream = new FileStream(path, FileMode.Create);


    }
}
