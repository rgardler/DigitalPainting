using UnityEngine;
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
            textManager.DisplayMessage(message, textColor, delay);
        }
    }
}