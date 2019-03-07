using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(InteractionCollection))]
    public class InteractionCollectionEditor : Editor
    {
        public string[] AllInteractionDescriptions
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
        private InteractionCollection collection;
        private string newInteractionDescription = "New Interaction";

        private const string creationPath = "Assets/Resources/InteractionsCollection.asset";
        private const float buttonWidth = 30f;

        private void OnEnable()
        {
            collection = (InteractionCollection)target;

            if (collection.interactions == null)
                collection.interactions = new Interaction[0];

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

        private void SetAllInteractionDescriptions()
        {
            AllInteractionDescriptions = new string[TryGetInteractionsLength()];

            for (int i = 0; i < AllInteractionDescriptions.Length; i++)
            {
                AllInteractionDescriptions[i] = TryGetInteractionAt(i).Description;
            }
        }

        public override void OnInspectorGUI()
        {
            if (interactionEditors == null || interactionEditors.Length != TryGetInteractionsLength())
            {
                for (int i = 0; i < interactionEditors.Length; i++)
                {
                    DestroyImmediate(interactionEditors[i]);
                }

                CreateEditors();
            }

            EditorGUILayout.LabelField("Interaction Collection");

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
            }
            EditorGUILayout.EndHorizontal();
        }

        private void CreateEditors()
        {
            interactionEditors = new InteractionEditor[collection.interactions.Length];

            for (int i = 0; i < interactionEditors.Length; i++)
            {
                interactionEditors[i] = CreateEditor(TryGetInteractionAt(i)) as InteractionEditor;
                interactionEditors[i].editorType = InteractionEditor.EditorType.AllInteractionsAsset;
                interactionEditors[i].parentEditor = this;
            }
        }

        internal void AddInteraction(string description)
        {
            Interaction newInteraction = CreateInstance<Interaction>();
            newInteraction.Description = description;
            newInteraction.name = description;

            AssetDatabase.Refresh();
            Undo.RecordObject(newInteraction, "Created new Interaction");
            AssetDatabase.AddObjectToAsset(newInteraction, collection);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newInteraction));
            ArrayUtility.Add(ref collection.interactions, newInteraction);
            EditorUtility.SetDirty(collection);
            AssetDatabase.SaveAssets();

            SetAllInteractionDescriptions();
        }

        public void RemoveInteraction(Interaction interaction)
        {
            AssetDatabase.Refresh();
            Undo.RecordObject(collection, "Removing Interaction");
            ArrayUtility.Remove(ref collection.interactions, interaction);
            DestroyImmediate(interaction, true);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(collection);

            SetAllInteractionDescriptions();
        }

        public int TryGetInteractionIndex(Interaction interaction)
        {
            if (interaction == null)
            {
                return -1;
            }
            for (int i = 0; i < TryGetInteractionsLength(); i++)
            {
                if (TryGetInteractionAt(i).Hash == interaction.Hash)
                {
                    return i;
                }
            }

            return -1;
        }

        public Interaction TryGetInteractionAt(int index)
        {
            Interaction[] allInteractions = collection.interactions;

            if (allInteractions == null || allInteractions[0] == null)
                return null;

            if (index >= allInteractions.Length)
                return allInteractions[0];

            return allInteractions[index];
        }

        public int TryGetInteractionsLength()
        {
            if (collection.interactions == null)
                return 0;
            return collection.interactions.Length;
        }

    }
}
