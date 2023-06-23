using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using KrakenGamingTest.ScriptableObjects.Player;

public class DataManager : MonoBehaviour
{
    private SaveData _data;

    public SaveData GetData() => _data;

    private const string _file = "SaveData.text";

    private void Awake()
    {
        Load();
    }

    public void ResetData()
    {
        Load();
        _data.GeneralVolume = 0;
        _data.MusicVolume = 0;
        _data.EffectsVolume = 0;
        Save();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(_data);
        WriteToFile(_file, json);
    }

    public void Load()
    {
        _data = new SaveData();
        string json = ReadFromFile(_file);
        JsonUtility.FromJsonOverwrite(json, _data);
    }

    public void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    public string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);

        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else Debug.LogWarning("FILE NOT FOUND");

        return "";
    }

    public string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    private void OnDisable()
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}