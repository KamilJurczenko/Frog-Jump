using UnityEditor;

[CustomEditor(typeof(ObjectRotation))]
public class ObjectRotationEditor : Editor
{
    static string[] dispOptions = { "normal", "left", "right", "up" };

    private SerializedProperty rotation;

    private void OnEnable()
    {
        rotation = serializedObject.FindProperty("rotation");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.UpdateIfRequiredOrScript();

        rotation.intValue = EditorGUILayout.Popup("Rotation", rotation.intValue, dispOptions);

        serializedObject.ApplyModifiedProperties();
    }
}
