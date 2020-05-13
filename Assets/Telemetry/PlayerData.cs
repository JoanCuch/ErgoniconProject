using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Telemetry
{
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
            public float startTime;
            public float endTime;
            public string extraInfo;


            public Action(string _actor, string _type, string _result, float _startTime, float _endTime, string _extraInfo)
            {
                actor = _actor;
                type = _type;
                result = _result;
                startTime = _startTime;
                endTime = _endTime;
                extraInfo = _extraInfo;

            }
        }


    }
}
