using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Telemetry
{
    public class InstructionManager : MonoBehaviour
    {
        private InstructionsData instructionsData;
        [SerializeField] private InstructionsData instructionsData_spanish;
        [SerializeField] private InstructionsData instructionsData_english;


        private List<InstructionsData.instruction> instructions;
        private int currentInstruction;

        private DataManager dataManager;
        [SerializeField] private Text text;

        private InstructionStates currentState;
        private string currentExpectedResult;


        private enum InstructionStates
        {
            lenguajeSelection,
            start,
            instruction,
            solved,
            unsolvedInstruction
        }

        // Start is called before the first frame update
        private void Start()
        {
            currentState = InstructionStates.lenguajeSelection;
            currentInstruction = -1;
            text.text = instructionsData_english.GetLanguajeMessage() + "\n" + instructionsData_spanish.GetLanguajeMessage();

            dataManager = DataManager.dataManager;

        }

        // Update is called once per frame
        private void Update()
        {
            if (dataManager == null) dataManager = DataManager.dataManager;
            //CheckCurrentInstruction();
        }


        public void GreenButtonActivated()
        {
            SendAction(DataManager.Actions.greenButton);

            switch (currentState)
            {
                case InstructionStates.lenguajeSelection:
                    SetLanguage(instructionsData_spanish);
                    SetStart();
                    break;

                case InstructionStates.start:
                    NextInstruction();
                    break;

                case InstructionStates.instruction:
                    SolvedInstruction();
                    break;

                case InstructionStates.solved:
                    SendAction(DataManager.Actions.instructionSolved);
                    NextInstruction();
                    break;

                case InstructionStates.unsolvedInstruction:
                    SendAction(DataManager.Actions.instructionUnsolved);
                    NextInstruction();
                    break;

                default:
                    break;
            }   

            /*SendAction(DataManager.Actions.greenButton);

            if(currentState == InstructionStates.wantsToPass)
            {
                SendAction(DataManager.Actions.instructionUnsolved);
                NextInstruction();
            }
            else if(currentState == InstructionStates.solved)
            {
                NextInstruction();
            }
            else if(currentState == InstructionStates.start)
            {
                NextInstruction();
            }*/
        }

        public void RedButtonActivated()
        {
            SendAction(DataManager.Actions.redButton);

            switch (currentState)
            {
                case InstructionStates.lenguajeSelection:
                    SetLanguage(instructionsData_english);
                    SetStart();
                    break;

                case InstructionStates.start:
                    currentState = InstructionStates.lenguajeSelection;
                    text.text = instructionsData.GetLanguajeMessage() + "\n" + instructionsData_spanish.GetLanguajeMessage();
                    break;

                case InstructionStates.instruction:
                    UnsolvedInstruction();
                    break;

                case InstructionStates.solved:
                    PreviousInstruction();
                    break;

                case InstructionStates.unsolvedInstruction:
                    PreviousInstruction();
                    break;

                default:
                    break;
            }

            /*SendAction(DataManager.Actions.redButton);

            if(currentState == InstructionStates.instruction)
            {
                WantsToPassInstruction();
            }
            else if (currentState == InstructionStates.wantsToPass)
            {
                PreviousInstruction();
            }*/
        }


        private void CheckCurrentInstruction()
        {
            if (currentState == InstructionStates.instruction || currentState == InstructionStates.unsolvedInstruction)
            {
                PlayerData.Action lastAction;

                if (dataManager.data.runes == null) return;
                if (dataManager.data.runes.Count <= 1) return;

                lastAction = dataManager.data.runes[dataManager.data.runes.Count - 1];

                if (lastAction.result == currentExpectedResult)
                {
                    SolvedInstruction();
                }
            }
        }



        private void UnsolvedInstruction()
        {
            SendAction(DataManager.Actions.unsolvedInstruction);

            currentState = InstructionStates.unsolvedInstruction;
            text.text = instructionsData.GetNotResolvedMessage();
        }

        private void PreviousInstruction()
        {
            SendAction(DataManager.Actions.previousIntructions);
            currentState = InstructionStates.instruction;

            if (currentInstruction < instructions.Count)
            {
                text.text = instructions[currentInstruction].questionMessage;
                currentExpectedResult = instructions[currentInstruction].expectedResult;
            }


        }
        private void NextInstruction()
        {
            currentState = InstructionStates.instruction;

            currentInstruction++;

            if (currentInstruction < instructions.Count)
            {
                text.text = instructions[currentInstruction].questionMessage;
                currentExpectedResult = instructions[currentInstruction].expectedResult;
            }           
        }
        private void SolvedInstruction()
        {
            currentState = InstructionStates.solved;

            text.text = instructionsData.GetResolvedMessage();
        }

        private void SetStart()
        {
            currentState = InstructionStates.start;
            text.text = instructionsData.GetStartMessage();
        }

        private void SetLanguage(InstructionsData _instructionsData)
        {
            instructionsData = _instructionsData;
            instructions = instructionsData.GetInstructions();
        }

        private void SendAction(DataManager.Actions _action)
        {
            dataManager.AddAction(
                DataManager.Actors.player,
                DataManager.Actions.greenButton,
                "null",
                Time.time,
                Time.time,
                "null"
                );
        }

    }
}
