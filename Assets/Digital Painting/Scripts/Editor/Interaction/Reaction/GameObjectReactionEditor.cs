using UnityEditor;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(GameObjectReaction))]
    public class GameObjectReactionEditor : ReactionEditor
    {
        protected override string GetFoldoutLabel()
        {
            return "Game Object Reaction";
        }
    }
}
