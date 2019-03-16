using System;
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.ability;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction.agent
{
    public class PickupItemControlAsset : PlayableAsset
    {        
        public Ability ability;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PickupItemControlBehaviour>.Create(graph);

            PickupItemControlBehaviour behaviour = playable.GetBehaviour();
            behaviour.ability = ability;

            return playable;
        }
    }
}