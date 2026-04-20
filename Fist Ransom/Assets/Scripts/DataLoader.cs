using UnityEngine;
using System.IO;
public class DataLoader : MonoBehaviour
{
    [System.Serializable]
    public class GameData {
    public int gameWon;
    }

    public bool SetTrue = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    if (!SetTrue)
        {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            GameData loadedData = JsonUtility.FromJson<GameData>(json);
            }
        }
    else
        {
            GameData data = new GameData {gameWon = 1};
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }
}
