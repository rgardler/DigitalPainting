using UnityEngine;
using UnityEditor;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(AllInteractions))]
    public class AllInteractionsEditor : Editor
    {
        public static string[] AllInteractionDescriptions
        {
            get
            {
                if (allInteractionDescriptions == null)
                {
                    SetAllInteractionDescriptions();
                }
                return allInteractionDescriptions;
            }
            private set { allInteractionDescriptions = value; }
        }

        private static string[] allInteractionDescriptions;

        private InteractionEditor[] interactionEditors;
        private AllInteractions allInteractions;
        private string newInteractionDescription = "New Interaction";

        private const string creationPath = "Assets/Resources/AllInteractions.asset";
        private const float buttonWidth = 30f;


        private void OnEnable()
        {
            allInteractions = (AllInteractions)target;

            if (allInteractions.interactions == null)
                allInteractions.interactions= new Interaction[0];

            if (interactionEditors == null)
            {
                CreateEditors();
            }
        }


        private void OnDisable()
        {
            for (int i = 0; i < interactionEditors.Length; i++)
            {
                DestroyImmediate(interactionEditors[i]);
            }

            interactionEditors = null;
        }


        private static void SetAllInteractionDescriptions()
        {
            AllInteractionDescriptions = new string[TryGetInteractionsLength()];

            for (int i = 0; i < AllInteractionDescriptions.Length; i++)
            {
                AllInteractionDescriptions[i] = TryGetInteractionAt(i).Description;
            }
        }

        public override void OnInspectorGUI()
        {
            if (interactionEditors.Length != TryGetInteractionsLength())
            {
                for (int i = 0; i < interactionEditors.Length; i++)
                {
                    DestroyImmediate(interactionEditors[i]);
                }

                CreateEditors();
            }

            for (int i = 0; i < interactionEditors.Length; i++)
            {
                interactionEditors[i].OnInspectorGUI();
            }

            if (TryGetInteractionsLength() > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.BeginHorizontal();
                        
            newInteractionDescription = EditorGUILayout.TextField(GUIContent.none, newInteractionDescription);

            if (GUILayout.Button("+", GUILayout.Width(buttonWidth)))
            {
                AddInteraction(newInteractionDescription);
                newInteractionDescription = "New Interaction";
            }
            EditorGUILayout.EndHorizontal();
        }


        private void CreateEditors()
        {
            interactionEditors = new InteractionEditor[allInteractions.interactions.Length];

            for (int i = 0; i < interactionEditors.Length; i++)
            {
                interactionEditors[i] = CreateEditor(TryGetInteractionAt(i)) as InteractionEditor;
                interactionEditors[i].editorType = InteractionEditor.EditorType.AllInteractionsAsset;
            }
        }


        internal void AddInteraction(string description)
        {
            if (!AllInteractions.Instance)
            {
                Debug.LogError("AllInteractions has not been created yet.");
                return;
            }

            Interaction newInteraction = InteractionEditor.CreateInteraction(description);
            newInteraction.name = description;

            Undo.RecordObject(newInteraction, "Created new Interaction");
            AssetDatabase.AddObjectToAsset(newInteraction, AllInteractions.Instance);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newInteraction));
            ArrayUtility.Add(ref AllInteractions.Instance.interactions, newInteraction);
            EditorUtility.SetDirty(AllInteractions.Instance);

            SetAllInteractionDescriptions();
        }


        public static void RemoveInteraction(Interaction interaction)
        {
            if (!AllInteractions.Instance)
            {
                Debug.LogError("AllInteractions has not been created yet.");
                return;
            }

            Undo.RecordObject(AllInteractions.Instance, "Removing Interaction");
            ArrayUtility.Remove(ref AllInteractions.Instance.interactions, interaction);
            DestroyImmediate(interaction, true);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(AllConditions.Instance);
            SetAllInteractionDescriptions();
        }

        public static int TryGetInteractionIndex(Interaction interaction)
        {
            for (int i = 0; i < TryGetInteractionsLength(); i++)
            {
                if (interaction == null)
                {
                    return -1;
                }
                else if (TryGetInteractionAt(i).Hash == interaction.Hash)
                {
                    return i;
                }
            }

            return -1;
        }


        public static Interaction TryGetInteractionAt(int index)
        {
            Interaction[] allInteractions = AllInteractions.Instance.interactions;

            if (allInteractions == null || allInteractions[0] == null)
                return null;

            if (index >= allInteractions.Length)
                return allInteractions[0];

            return allInteractions[index];
        }


        public static int TryGetInteractionsLength()
        {
            if (AllInteractions.Instance.interactions == null)
                return 0;
            return AllInteractions.Instance.interactions.Length;
        }
    }
}