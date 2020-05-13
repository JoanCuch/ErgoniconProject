using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerData
{

    public List<Action> actions = new List<Action>();
    
    [Serializable]
    public struct Action
    {
        public string actor;
        public string type;
        public string result;
        public string extraInfo; 

        
        public Action(string _actor, string _type, string _result, string _extraInfo)
        {
            actor = _actor;
            type = _type;
            result = _result;
            extraInfo = _extraInfo;
        }
    }


}
