using UnityEngine.Timeline;

namespace wizardscode.interaction
{
    [TrackClipType(typeof(InteractableControlAsset))]
    [TrackBindingType(typeof(Interactable))]
    public class InteractableControlTrack : TrackAsset { }
}