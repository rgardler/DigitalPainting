using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace wizardscode.interaction.agent
{
    [System.Serializable]
    public class SelectFromInventoryControlAsset : PlayableAsset
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            var playable = ScriptPlayable<SelectFromInventoryControlBehaviour>.Create(graph);

            SelectFromInventoryControlBehaviour behaviour = playable.GetBehaviour();
            return playable;
        }
    }
}
