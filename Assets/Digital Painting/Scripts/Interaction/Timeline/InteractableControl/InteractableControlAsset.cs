using UnityEngine;
using UnityEngine.Playables;

namespace wizardscode.interaction
{
    public class InteractableControlAsset : PlayableAsset
    {
        public bool activeState;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<InteractableControlBehaviour>.Create(graph);

            InteractableControlBehaviour behaviour = playable.GetBehaviour();
            behaviour.activeState = activeState;

            return playable;
        }
    }
}