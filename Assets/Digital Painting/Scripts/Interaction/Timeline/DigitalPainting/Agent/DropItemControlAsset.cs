using UnityEngine;
using UnityEngine.Playables;
using wizardscode.ability;

namespace wizardscode.interaction.agent
{
    public class DropItemControlAsset : PlayableAsset
    {        
        public Ability ability;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<DropItemControlBehaviour>.Create(graph);

            DropItemControlBehaviour behaviour = playable.GetBehaviour();
            behaviour.ability = ability;

            return playable;
        }
    }
}