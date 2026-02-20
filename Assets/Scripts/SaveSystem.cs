using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static string SAVING_PATH = Path.Combine(Application.persistentDataPath, "save.data");

    private static SaveSystem instance;

    private SavedData currentSavedData;

    public static SaveSystem GetInstance()
    {
        if (instance == null)
        {
            instance = new SaveSystem();
        }

        return instance;
    }
    public SavedData CurrentSavedData
    {
        get { return currentSavedData; }
        set { currentSavedData = value; }
    }


    private SaveSystem()
    {
        Load();
    }

    private void Load()
    {
        if (File.Exists(SAVING_PATH))
        {
            string readData = File.ReadAllText(SAVING_PATH);
            currentSavedData = JsonUtility.FromJson<SavedData>(readData);
        }
        else
        {
            currentSavedData = new SavedData();
            Save();
        }
    }

    public void Save()
    {
        string jsonData = JsonUtility.ToJson(currentSavedData);
        File.WriteAllText(SAVING_PATH, jsonData);
    }
}
