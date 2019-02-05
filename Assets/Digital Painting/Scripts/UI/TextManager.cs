using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace wizardscode.interaction
{
    public class TextManager : MonoBehaviour
    {
        [Tooltip("The text UI element in which to display text.")]
        public Text text;
        public float displayTimePerCharacter = 0.1f;
        public float additionalDisplayTime = 0.5f;
        
        private List<Instruction> instructions = new List<Instruction>();
        private float clearTime;

        public struct Instruction
        {
            public string message;
            public Color textColor;
            public float startTime;
        }

        public void Start()
        {
            text.text = "";
        }

        private void Update()
        {
            if (instructions.Count > 0 && Time.time >= instructions[0].startTime)
            {
                text.text = instructions[0].message;
                text.color = instructions[0].textColor;

                instructions.RemoveAt(0);
            }
            else if (Time.time >= clearTime)
            {
                text.text = string.Empty;
            }
        }

        public void DisplayMessage(string message, Color textColor, float delay)
        {
            float startTime = Time.time + delay;

            float displayDuration = message.Length * displayTimePerCharacter + additionalDisplayTime;

            float newClearTime = startTime + displayDuration;

            if (newClearTime > clearTime)
                clearTime = newClearTime;

            Instruction newInstruction = new Instruction
            {
                message = message,
                textColor = textColor,
                startTime = startTime
            };

            instructions.Add(newInstruction);

            SortInstructions();
        }


        private void SortInstructions()
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                bool swapped = false;

                for (int j = 0; j < instructions.Count; j++)
                {
                    if (instructions[i].startTime > instructions[j].startTime)
                    {
                        Instruction temp = instructions[i];
                        instructions[i] = instructions[j];
                        instructions[j] = temp;

                        swapped = true;
                    }
                }

                if (!swapped)
                    break;
            }
        }
    }
}