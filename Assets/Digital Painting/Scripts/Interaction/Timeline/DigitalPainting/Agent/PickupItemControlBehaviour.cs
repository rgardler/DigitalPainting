using UnityEngine.Playables;
using wizardscode.ability;
using wizardscode.digitalpainting.agent;

namespace wizardscode.interaction.agent
{
    public class PickupItemControlBehaviour : BaseSpellPlayableBehaviour<LevitateItemSpell>
    {
        internal override void Initialize(Playable playable, FrameData info, object playerData)
        {
            base.Initialize(playable, info, playerData);

            spell.item = agent.Interactable;
            spell.endTransform = agent.equippedMountPoint.transform;
        }

        internal override void Complete(Playable playable, FrameData info, object playerData)
        {
            base.Complete(playable, info, playerData);
            agent.Using = agent.Interactable;
            agent.PointOfInterest = null;
        }
    }
}