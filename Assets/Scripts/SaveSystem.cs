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
        if (VirtualMountPoint.Exists(SAVING_PATH))
        {
            VirtualMountPoint.Read(SAVING_PATH, out string readData);
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
        VirtualMountPoint.Write(SAVING_PATH, jsonData);
        VirtualMountPoint.UpdateInDisk();
    }
}
