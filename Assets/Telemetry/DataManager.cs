using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Telemetry
{
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
            //data = new PlayerData();
            //string json = ReadFromFile(file);
            //JsonUtility.FromJsonOverwrite(json, data);
        }

        private void WriteToFile(string fileName, string json)
        {
            string path = GetFilePath(fileName);

            FileStream fileStream = new FileStream(path, FileMode.Create);

            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }

            //Debug.Log("saved information on path: " + path);
        }


        private string ReadFromFile(string fileName)
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
            else
            {
                Debug.LogWarning("File not found");
                return "";
            }
        }

        private string GetFilePath(string fileName)
        {
            return Application.dataPath + "/" + fileName;
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
            select,
            greenButton,
            redButton,

            changeState,
            runeCreation,
            instructionSolved,
            instructionUnsolved,
            wantsToPass,
            previousIntructions          
        }

        public void AddAction(Actors _actor, Actions _type, string _result, float _startTime, float _endTime, string _extraInfo)
        {
            PlayerData.Action newAction = new PlayerData.Action(_actor.ToString("g"), _type.ToString("g"), _result, _startTime, _endTime, _extraInfo);

            data.actions.Add(newAction);

            if (_type == Actions.runeCreation)
            {
                data.runes.Add(newAction);
            }

            Save();
        }
        #endregion

    }
}

