#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Gameplay.TextAnimation.Editor
{
    [CustomEditor(typeof(TypewriterSettings))]
    public sealed class TypewriterSettingsDrawer : UnityEditor.Editor
    {
        private const float INDENT_SPACE = 20f;
        
        private bool _showMovementFoldout;
        private bool _showMovementXFoldout;
        private bool _showMovementYFoldout;
        private bool _showMovementZFoldout;

        private bool _showRotationFoldout;
        private bool _showRotationXFoldout;
        private bool _showRotationYFoldout;
        private bool _showRotationZFoldout;

        private bool _showScaleFoldout;
        private bool _showScaleXFoldout;
        private bool _showScaleYFoldout;
        private bool _showScaleZFoldout;

        private bool _showColorFoldout;
        private bool _showColorSettingsFoldout;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawBaseSettings();
            GUILayout.Space(10);
            DrawMovementSettings();
            DrawRotationSettings();
            DrawScaleSettings();
            DrawColorSettings();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawBaseSettings()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_baseSpeed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_charactersPerSecond"));
        }

        private void DrawMovementSettings()
        {
            _showMovementFoldout = EditorGUILayout.Foldout(_showMovementFoldout, "Movement", true);
            if (!_showMovementFoldout) return;

            var movement = serializedObject.FindProperty("_movement");
            DrawAxisSettings(movement, "_isXEnabled", "_xMovement", ref _showMovementXFoldout, "Movement X");
            DrawAxisSettings(movement, "_isYEnabled", "_yMovement", ref _showMovementYFoldout, "Movement Y");
            DrawAxisSettings(movement, "_isZEnabled", "_zMovement", ref _showMovementZFoldout, "Movement Z");
        }

        private void DrawRotationSettings()
        {
            _showRotationFoldout = EditorGUILayout.Foldout(_showRotationFoldout, "Rotation", true);
            if (!_showRotationFoldout) return;

            var rotation = serializedObject.FindProperty("_rotation");
            DrawAxisSettings(rotation, "_isXEnabled", "_xRotation", ref _showRotationXFoldout, "Rotation X", true);
            DrawAxisSettings(rotation, "_isYEnabled", "_yRotation", ref _showRotationYFoldout, "Rotation Y", true);
            DrawAxisSettings(rotation, "_isZEnabled", "_zRotation", ref _showRotationZFoldout, "Rotation Z", true);
        }

        private void DrawScaleSettings()
        {
            _showScaleFoldout = EditorGUILayout.Foldout(_showScaleFoldout, "Scale", true);
            if (!_showScaleFoldout) return;

            var scale = serializedObject.FindProperty("_scale");
            DrawAxisSettings(scale, "_isXEnabled", "_xScale", ref _showScaleXFoldout, "Scale X", true);
            DrawAxisSettings(scale, "_isYEnabled", "_yScale", ref _showScaleYFoldout, "Scale Y", true);
            DrawAxisSettings(scale, "_isZEnabled", "_zScale", ref _showScaleZFoldout, "Scale Z", true);
        }

        private void DrawColorSettings()
        {
            _showColorFoldout = EditorGUILayout.Foldout(_showColorFoldout, "Color", true);
            if (!_showColorFoldout) return;

            var colorSettings = serializedObject.FindProperty("_color");
            DrawColorSettingsUI(colorSettings);
        }

        private void DrawAxisSettings(SerializedProperty parent, string enabledPropertyName, 
            string settingsPropertyName, ref bool foldout, string label, bool includeAnchor = false)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(INDENT_SPACE);
            
            var enabledProperty = parent.FindPropertyRelative(enabledPropertyName);
            GUI.enabled = enabledProperty.boolValue;
            foldout = EditorGUILayout.Foldout(foldout, label, true);
            GUI.enabled = true;
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.PropertyField(enabledProperty, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            if (!enabledProperty.boolValue || !foldout) return;

            var settings = parent.FindPropertyRelative(settingsPropertyName);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(INDENT_SPACE * 2);
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.PropertyField(settings.FindPropertyRelative("_curve"));
            if (includeAnchor)
            {
                string anchorProp = "_anchor";
                EditorGUILayout.PropertyField(settings.FindPropertyRelative(anchorProp));
            }
            
            EditorGUILayout.PropertyField(settings.FindPropertyRelative("_yMultiplier"));
            EditorGUILayout.PropertyField(settings.FindPropertyRelative("_speedMultiplier"));
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawColorSettingsUI(SerializedProperty colorSettings)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(INDENT_SPACE);
            
            var enabledProperty = colorSettings.FindPropertyRelative("_isEnabled");
            GUI.enabled = enabledProperty.boolValue;
            _showColorSettingsFoldout = EditorGUILayout.Foldout(_showColorSettingsFoldout, "Color Settings", true);
            GUI.enabled = true;
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.PropertyField(enabledProperty, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            if (!enabledProperty.boolValue || !_showColorSettingsFoldout) return;

            var settings = colorSettings.FindPropertyRelative("_colorAnimation");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(INDENT_SPACE * 2);
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.PropertyField(settings.FindPropertyRelative("_gradient"));
            EditorGUILayout.PropertyField(settings.FindPropertyRelative("_speedMultiplier"));
            EditorGUILayout.PropertyField(settings.FindPropertyRelative("_useOnlyAlpha"));
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif