using UnityEngine;
using UnityEditor;
using wizardscode.condition.inventory;

namespace wizardscode.interaction
{
    [CustomEditor(typeof(ConditionCollection))]
    public class ConditionCollectionEditor : Editor
    {
        public string[] AllConditionDescriptions
        {
            get
            {
                // If the description array doesn't exist yet, set it.
                if (allConditionDescriptions == null)
                {
                    SetAllConditionDescriptions();
                }
                return allConditionDescriptions;
            }
            private set { allConditionDescriptions = value; }
        }

        private static string[] allConditionDescriptions;

        private ConditionEditor[] conditionEditors;
        private ConditionCollection collection;
        private string newConditionDescription = "New Condition";

        private const string creationPath = "Assets/Resources/ConditionCollection.asset";
        private const float buttonWidth = 30f;

        int typeIndex = 0;
        string[] typeNames;

        private void OnEnable()
        {
            collection = (ConditionCollection)target;

            if (collection.conditions == null)
                collection.conditions = new Condition[0];

            if (conditionEditors == null)
            {
                CreateEditors();
            }

            typeNames = new string[] { "Boolean", "Ability", "Storage Capacity" };
        }


        private void OnDisable()
        {
            for (int i = 0; i < conditionEditors.Length; i++)
            {
                DestroyImmediate(conditionEditors[i]);
            }

            conditionEditors = null;
        }


        private void SetAllConditionDescriptions()
        {
            AllConditionDescriptions = new string[TryGetConditionsLength()];

            for (int i = 0; i < AllConditionDescriptions.Length; i++)
            {
                AllConditionDescriptions[i] = TryGetConditionAt(i).Description;
            }
        }

        public override void OnInspectorGUI()
        {
            if (conditionEditors.Length != TryGetConditionsLength())
            {
                for (int i = 0; i < conditionEditors.Length; i++)
                {
                    DestroyImmediate(conditionEditors[i]);
                }

                CreateEditors();
            }

            for (int i = 0; i < conditionEditors.Length; i++)
            {
                conditionEditors[i].OnInspectorGUI();
            }

            if (TryGetConditionsLength() > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.BeginHorizontal();

            typeIndex = EditorGUILayout.Popup(typeIndex, typeNames);
            
            newConditionDescription = EditorGUILayout.TextField(GUIContent.none, newConditionDescription);

            if (GUILayout.Button("+", GUILayout.Width(buttonWidth)))
            {
                AddCondition(newConditionDescription);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void CreateEditors()
        {
            conditionEditors = new ConditionEditor[collection.conditions.Length];

            for (int i = 0; i < conditionEditors.Length; i++)
            {
                conditionEditors[i] = CreateEditor(TryGetConditionAt(i)) as ConditionEditor;
                conditionEditors[i].editorType = ConditionEditor.EditorType.AllConditionsAsset;
                conditionEditors[i].parentEditor = this;
            }
        }


        private void AddCondition(string description)
        {
            Condition newCondition = null;
            switch (typeIndex)
            {
                case 0:
                    newCondition = CreateInstance<Condition>();
                    break;
                case 1:
                    newCondition = CreateInstance<AbilityCondition>();
                    break;
                case 2:
                    newCondition = CreateInstance<InventoryCapacityCondition>();
                    break;
                default:
                    Debug.LogError("Attempt to create a condition of unknown type");
                    break;
            }
            newCondition.Description = description;
            newCondition.name = description;

            AssetDatabase.Refresh();
            Undo.RecordObject(newCondition, "Created new Condition");
            AssetDatabase.AddObjectToAsset(newCondition, collection);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newCondition));
            ArrayUtility.Add(ref collection.conditions, newCondition);
            EditorUtility.SetDirty(collection);
            AssetDatabase.SaveAssets();

            SetAllConditionDescriptions();
        }


        public void RemoveCondition(Condition condition)
        {
            AssetDatabase.Refresh();
            Undo.RecordObject(collection, "Removing condition");
            ArrayUtility.Remove(ref collection.conditions, condition);
            DestroyImmediate(condition, true);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(collection);
            SetAllConditionDescriptions();
        }
        
        public int TryGetConditionIndex(Condition condition)
        {
            if (condition == null)
            {
                return -1;
            }
            for (int i = 0; i < TryGetConditionsLength(); i++)
            {
                if (TryGetConditionAt(i).Hash == condition.Hash)
                    return i;
            }

            return -1;
        }


        public Condition TryGetConditionAt(int index)
        {
            Condition[] allConditions = collection.conditions;

            if (allConditions == null || allConditions[0] == null)
                return null;

            if (index >= allConditions.Length)
                return allConditions[0];

            return allConditions[index];
        }


        public int TryGetConditionsLength()
        {
            if (collection.conditions == null)
                return 0;
            return collection.conditions.Length;
        }
    }
}