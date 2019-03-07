using UnityEngine;
using UnityEditor;
using wizardscode.interaction;

public abstract class EditorWithSubEditors<TEditor, TTarget> : Editor
    where TEditor : Editor
    where TTarget : Object
{
    protected TEditor[] subEditors;

    internal Editor parentEditor;

    // This should be called in OnEnable and at the start of OnInspectorGUI.
    protected void CheckAndCreateSubEditors(TTarget[] subEditorTargets)
    {
        if (subEditors != null && subEditors.Length == subEditorTargets.Length)
            return;

        CleanupEditors();
        subEditors = new TEditor[subEditorTargets.Length];

        for (int i = 0; i < subEditors.Length; i++)
        {
            subEditors[i] = CreateEditor(subEditorTargets[i]) as TEditor;
            SubEditorSetup(subEditors[i]);
        }
    }

    // This should be called in OnDisable.
    protected void CleanupEditors()
    {
        if (subEditors == null)
            return;

        for (int i = 0; i < subEditors.Length; i++)
        {
            DestroyImmediate(subEditors[i]);
        }

        subEditors = null;
    }

    protected abstract void SubEditorSetup(TEditor editor);
}
