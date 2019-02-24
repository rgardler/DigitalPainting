using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;
using wizardscode.digitalpainting;


namespace wizardscode.interaction
{
    [TrackColor(0f, 0.0602479f, 1f)]
    [TrackClipType(typeof(DropItemControlAsset))]
    [TrackClipType(typeof(InteractableControlAsset))]
    [TrackClipType(typeof(InventoryAddItemControlAsset))]
    [TrackClipType(typeof(PickupItemControlAsset))]
    [TrackBindingType(typeof(DigitalPaintingManager))]
    public class DigitalPaintingControlTrack : TrackAsset
    {
        // Please note this assumes only one component of type DigitalPaintingManager on the same gameobject.
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            DigitalPaintingManager trackBinding = director.GetGenericBinding(this) as DigitalPaintingManager;
            if (trackBinding == null)
                return;

#endif
            base.GatherProperties(director, driver);
        }
    }
}