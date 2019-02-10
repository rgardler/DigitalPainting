using System;
using UnityEngine;
using UnityEngine.Playables;
using wizardscode.ability;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;
using wizardscode.inventory;

namespace wizardscode.interaction
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