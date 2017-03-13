using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof (WireframeTesseract.FaceParams))]
public class PlaneParamDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var colorRect = new Rect(position.x, position.y, 100, position.height);
        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("faceColor"), GUIContent.none);

        var groupNameRect = new Rect(position.x + 110, position.y, 80, position.height);
        EditorGUI.LabelField(groupNameRect, "Cube Group");
        var groupRect = new Rect(position.x + 200, position.y, 50, position.height);
        EditorGUI.PropertyField(groupRect, property.FindPropertyRelative("faceGroup"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(WireframeTesseract.CubeParams))]
public class CubeParamDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var colorRect = new Rect(position.x, position.y, 100, position.height);
        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("cubeColor"), GUIContent.none);

        var enabledLabelRect = new Rect(position.x + 110, position.y, 80, position.height);
        EditorGUI.LabelField(enabledLabelRect, "DrawCube");
        var enabledRect = new Rect(position.x + 200, position.y, 50, position.height);
        EditorGUI.PropertyField(enabledRect, property.FindPropertyRelative("drawCube"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}