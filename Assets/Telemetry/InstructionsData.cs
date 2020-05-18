using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instructions", menuName = "Instructions")]
public class InstructionsData : ScriptableObject
{

    [SerializeField] private string languajeMessage;
    [SerializeField] private string startMessage;
    [SerializeField] private string nonResolvedMessage;
    [SerializeField] private string resolvedMessage;

    [SerializeField] private List<instruction> instructions;

    [System.Serializable]
    public struct instruction
    {
        public string questionMessage;
        public string expectedResult;

        public instruction(string _questionMessage, string _expectedResult)
        {
            questionMessage = _questionMessage;
            expectedResult = _expectedResult;
        }
    }

    public string GetLanguajeMessage() { return languajeMessage; }
    public string GetStartMessage() { return startMessage; }
    public string GetNotResolvedMessage() { return nonResolvedMessage; }
    public string GetResolvedMessage() { return resolvedMessage; }
    public List<instruction> GetInstructions() {return instructions; }
}
