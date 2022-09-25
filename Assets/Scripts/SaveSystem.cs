using UnityEngine;
using UnityEditor;
using System.IO;

public static class SaveSystem
{
    public static void SaveGame(Solitaire solitaire)
    {
        string path = Application.persistentDataPath + "/LastGameState.txt";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

        StreamWriter writer = new StreamWriter(stream); //problem is here

        writer.WriteLine("Test");

        writer.Close();

        FileStream streamAgain = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamReader reader = new StreamReader(streamAgain);

        Debug.Log(reader.ReadToEnd());

        reader.Close();

        //TODO make a save game class and start saving out files as jsons for easier importing

    }
}
