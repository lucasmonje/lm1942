#if UNITY_EDITOR
using System;
using Core.Utils;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomPropertyDrawer(typeof(StringInList))]
    public class StringInListDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var stringInList = attribute as StringInList;
            if (stringInList == null) return;
            var list = stringInList.List;
            
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    var index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
                    index = EditorGUI.Popup(position, property.displayName, index, list);

                    property.stringValue = list[index];
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, list);
                    break;
                default:
                    base.OnGUI(position, property, label);
                    break;
            }
        }
    }
}
#endif