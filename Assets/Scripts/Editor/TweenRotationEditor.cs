using UnityEditor;
using UnityEngine;

namespace Demo.Editor
{
    [CustomEditor(typeof(TweenRotation))]
    public class TweenRotationEditor : TweenerEditor
    {
        public override void OnInspectorGUI ()
        {
            GUILayout.Space(6f);
            EditorTools.SetLabelWidth(120f);

            TweenRotation tw = target as TweenRotation;
            GUI.changed = false;

            Vector3 from = EditorGUILayout.Vector3Field("From", tw.From);
            Vector3 to = EditorGUILayout.Vector3Field("To", tw.To);

            if (GUI.changed)
            {
                EditorTools.RegisterUndo("Tween Change", tw);
                tw.From = from;
                tw.To = to;
            }

            DrawCommonProperties();
        }
    }
}