using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TapTap.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            bool enabled = GUI.enabled;
            GUI.enabled &= !EditorApplication.isPlaying;
            EditorGUI.PropertyField(rect, property, label, true);
            GUI.enabled = enabled;

            EditorGUI.EndProperty();
        }
    }
}
