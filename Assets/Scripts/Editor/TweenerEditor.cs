using UnityEditor;
using UnityEngine;

namespace Demo.Editor
{
    [CustomEditor(typeof(Tweener), true)]
    public class TweenerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI ()
        {
            GUILayout.Space(6f);
            EditorTools.SetLabelWidth(110f);
            base.OnInspectorGUI();
            DrawCommonProperties();
        }

        protected void DrawCommonProperties ()
        {
            Tweener tw = target as Tweener;

            if (EditorTools.DrawHeader("Tweener"))
            {
                EditorTools.BeginContents();
                EditorTools.SetLabelWidth(110f);

                GUI.changed = false;

                Tweener.Style style = (Tweener.Style)EditorGUILayout.EnumPopup("Play Style", tw.PlayStyle);
                AnimationCurve curve = EditorGUILayout.CurveField("ExplotionAnimator Curve", tw.PlayAnimationCurve, GUILayout.Width(170f), GUILayout.Height(62f));
                Tweener.Method method = (Tweener.Method)EditorGUILayout.EnumPopup("Play Method", tw.PlayMethod);

                GUILayout.BeginHorizontal();
                float dur = EditorGUILayout.FloatField("Duration", tw.Duration, GUILayout.Width(170f));
                GUILayout.Label("seconds");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                float del = EditorGUILayout.FloatField("Start Delay", tw.Delay, GUILayout.Width(170f));
                GUILayout.Label("seconds");
                GUILayout.EndHorizontal();

                if (GUI.changed)
                {
                    EditorTools.RegisterUndo("Tween Change", tw);
                    tw.PlayAnimationCurve = curve;
                    tw.PlayMethod = method;
                    tw.PlayStyle = style;
                    tw.Duration = dur;
                    tw.Delay = del;
                }
                EditorTools.EndContents();
            }

            //EditorTools.SetLabelWidth(80f);
            //EditorTools.DrawEvents("On Finished", tw, tw.onFinished);
        }
    }
}