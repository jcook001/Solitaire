using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public string playerName;
    public int lives;
    public float health;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
