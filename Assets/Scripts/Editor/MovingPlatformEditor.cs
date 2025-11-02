using JetBrains.Annotations;
using UnityEditor;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    static string[] dispOptions = {"none", "slow", "medium", "fast" };

    private SerializedProperty distance;

    private SerializedProperty xIndex;
    private SerializedProperty yIndex;

    private SerializedProperty maxDistance;

    private void OnEnable()
    {
        distance = serializedObject.FindProperty("distance");
        xIndex = serializedObject.FindProperty("xIndex");
        yIndex = serializedObject.FindProperty("yIndex");
        maxDistance = serializedObject.FindProperty("maxDistance");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.UpdateIfRequiredOrScript();
        
        distance.floatValue = EditorGUILayout.Slider("Distance", distance.floatValue, 1, maxDistance.intValue);
        
        xIndex.intValue = EditorGUILayout.Popup("xSpeed", xIndex.intValue, dispOptions);
        yIndex.intValue = EditorGUILayout.Popup("ySpeed", yIndex.intValue, dispOptions);

        serializedObject.ApplyModifiedProperties();
    }

}
