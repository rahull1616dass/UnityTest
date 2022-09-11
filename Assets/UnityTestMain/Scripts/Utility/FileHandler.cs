using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class FileHandler
{
    private static bool IsSaved(string FileName)
    {
        return Directory.Exists(Application.persistentDataPath + "/" + FileName);
    }

    public static void SaveData(string FileName, object Data)
    {
        if (!IsSaved(FileName))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + FileName);
        }
        if (Data == null)
            return;
        BinaryFormatter BF = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + FileName + "/" + FileName + ".txt");
        var json = JsonUtility.ToJson(Data);
        BF.Serialize(file, json);
        file.Close();
    }

    public static object LoadData(string FileName, object data)
    {
        object Data = data;
        if (!IsSaved(FileName))
        {
            return null;
        }
        else if (!File.Exists(Application.persistentDataPath + "/" + FileName + "/" + FileName + ".txt"))
        {
            return null;
        }
        else
        {
            BinaryFormatter BF = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + FileName + "/" + FileName + ".txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)BF.Deserialize(file), Data);
            file.Close();
            return Data;
        }
    }

    internal static bool ReadFromFile(object p, out byte[] t_ImageBytes)
    {
        throw new NotImplementedException();
    }
}