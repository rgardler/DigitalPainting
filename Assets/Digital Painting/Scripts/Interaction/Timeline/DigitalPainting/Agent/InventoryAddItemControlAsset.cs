using System;
using UnityEngine;
using UnityEngine.Playables;

namespace wizardscode.interaction.agent
{
    public class InventoryAddItemControlAsset : PlayableAsset
    {

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<InventoryAddItemControlBehaviour> playable = ScriptPlayable<InventoryAddItemControlBehaviour>.Create(graph);

            return playable;
        }
    }
}