using UnityEditor;

[CustomEditor(typeof(SpikeMoving))]
public class SpikeMovingEditor : Editor
{
    static string[] dispOptions = {"slow", "medium", "fast" };

    private SerializedProperty indexSpeed;

    private void OnEnable()
    {
        indexSpeed = serializedObject.FindProperty("indexSpeed");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.UpdateIfRequiredOrScript();

        indexSpeed.intValue = EditorGUILayout.Popup("Move Speed", indexSpeed.intValue, dispOptions);

        serializedObject.ApplyModifiedProperties();
    }

}
