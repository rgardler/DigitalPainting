﻿using UnityEngine;
using UnityEngine.Playables;

namespace wizardscode.interaction
{
    public class NarrationControlBehaviour : PlayableBehaviour
    {
        public string message;
        public Color textColor = Color.white;
        public float delay;
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            TextManager textManager = playerData as TextManager;
            if (textManager != null)
            {
                textManager.DisplayMessage(message, textColor, delay);
            }
            Debug.Log("Narrator: " + message);
        }
    }
}