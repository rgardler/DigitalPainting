using UnityEngine;
using UnityEngine.Playables;

namespace wizardscode.interaction
{
    public class NarrationControlAsset : PlayableAsset
    {
        public string message = "Hi from the timeline";
        public Color textColor = Color.white;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<NarrationControlBehaviour>.Create(graph);

            NarrationControlBehaviour narrationBehaviour = playable.GetBehaviour();
            narrationBehaviour.message = message;
            narrationBehaviour.textColor = textColor;

            return playable;
        }
    }
}