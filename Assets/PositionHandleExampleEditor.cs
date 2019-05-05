using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GizmoTest)), CanEditMultipleObjects]
public class PositionHandleExampleEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        GizmoTest example = (GizmoTest)target;

        EditorGUI.BeginChangeCheck();
        Vector3 newTargetPosition = Handles.PositionHandle(example.targetPosition+example.transform.position, Quaternion.identity)-example.transform.position;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(example, "Change Look At Target Position");
            example.targetPosition = newTargetPosition.normalized*10.0f;
            example.Update();
        }
    }
}