using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    public static DataManager dataManager;

    public PlayerData data;

    public string file = "player.json";

    private void Start()
    {
        dataManager = this;
        data = new PlayerData();
    }


    #region JSON conversor

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        WriteToFile(file, json);

    }

    public void Load()
    {
        data = new PlayerData();
        string json = ReadFromFile(file);
        JsonUtility.FromJsonOverwrite(json, data);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);

        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }

        Debug.Log("saved information on path: " + path);
    }


    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using(StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found");
            return "";
        }
    }
    
    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    #endregion

    #region adders

    public enum Actors
    {
        player,
        game
    }
    public enum Actions
    {
        draw,
        teleport,
        interact,
        changeState,
        runeCreation,
    }

    public void AddAction(Actors _actor, Actions _type, string _result, string _extraInfo)
    {
        PlayerData.Action newAction = new PlayerData.Action(_actor.ToString("g"), _type.ToString("g"), _result, _extraInfo);
        data.actions.Add(newAction);
        Save();
    }




    #endregion

}

