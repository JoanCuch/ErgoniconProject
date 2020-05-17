using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Telemetry
{
    public class InstructionManager : MonoBehaviour
    {
        [SerializeField] private InstructionsData instructionsData;

        private List<InstructionsData.instruction> instructions;
        private int currentInstruction;

        private DataManager dataManager;
        [SerializeField] private Text text;

        private InstructionStates currentState;
        private string currentExpectedResult;


        private enum InstructionStates
        {
            start,
            instruction,
            solved,
            wantsToPass
        }

        // Start is called before the first frame update
        private void Start()
        {
            currentState = InstructionStates.start;
            currentInstruction = -1;
            text.text = instructionsData.GetStartMessage();
            instructions = instructionsData.GetInstructions();

            dataManager = DataManager.dataManager;

        }

        // Update is called once per frame
        private void Update()
        {
            if (dataManager == null) dataManager = DataManager.dataManager;
            CheckCurrentInstruction();
        }


        public void GreenButtonActivated()
        {
            SendAction(DataManager.Actions.greenButton);

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
            }
        }

        public void RedButtonActivated()
        {
            SendAction(DataManager.Actions.redButton);

            if(currentState == InstructionStates.instruction)
            {
                WantsToPassInstruction();
            }
            else if (currentState == InstructionStates.wantsToPass)
            {
                PreviousInstruction();
            }
        }


        private void CheckCurrentInstruction()
        {
            if (currentState == InstructionStates.instruction || currentState == InstructionStates.wantsToPass)
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



        private void WantsToPassInstruction()
        {
            SendAction(DataManager.Actions.wantsToPass);

            currentState = InstructionStates.wantsToPass;
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
            SendAction(DataManager.Actions.instructionSolved);
            currentState = InstructionStates.solved;

            text.text = instructionsData.GetResolvedMessage();
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
