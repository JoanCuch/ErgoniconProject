using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomConsole
{
    public class CustomConsole : MonoBehaviour
    {
#if CHEATS_ENABLED
        [Space, SerializeField] private Text _text;
        [SerializeField] private KeyCode _clearKey;
		[SerializeField] private int _maxLogCount;

		private string _myLog;
        private readonly Queue<string> _myLogQueue = new Queue<string>();
        
        private void Update()
        {
            if(Input.GetKeyDown(_clearKey))
            {
                Clear();
            }

			if (_myLogQueue.Count >= _maxLogCount)
			{
				_myLogQueue.Dequeue();
			}
		}

        private void OnEnable()
        {
            Application.logMessageReceived += PrintDebugMessage;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= PrintDebugMessage;
        }

        private void PrintDebugMessage(string logString, string stackTrace, LogType type)
        {
            _myLog = logString;
            var newString = "\n [" + type + "] : " + _myLog;
            _myLogQueue.Enqueue(newString);
            if (type == LogType.Exception)
            {
                newString = "\n" + stackTrace;
                _myLogQueue.Enqueue(newString);
            }
            _myLog = string.Empty;
            foreach(var myLog in _myLogQueue){
                _myLog += myLog;
            }

            _text.text = _myLog;
        }

        private void Clear()
        {
            _text.text = "";
            _myLog = "";
            _myLogQueue.Clear();
        }
#endif
    }
}