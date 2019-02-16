using UnityEngine.Timeline;

namespace wizardscode.interaction
{
    [TrackClipType(typeof(NarrationControlAsset))]
    [TrackBindingType(typeof(TextManager))]
    public class NarrationControlTrack : TrackAsset { }
}