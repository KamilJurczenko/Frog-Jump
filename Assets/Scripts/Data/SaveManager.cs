using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static void SaveData(object saveData,string savePath)
    {
        string path = Application.persistentDataPath + "/" + savePath + ".dat";
        FileStream file = new FileStream(path, FileMode.OpenOrCreate); // path = Application.persistentDataPath + "/Player.dat"

        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(file, saveData);

        file.Close(); 
    }

    public static object LoadData(string loadPath)
    {
        string path = Application.persistentDataPath + "/" + loadPath + ".dat";
        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.OpenOrCreate);

            BinaryFormatter formatter = new BinaryFormatter();

            object dataTSave = formatter.Deserialize(file);
            file.Close();
            return dataTSave;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
