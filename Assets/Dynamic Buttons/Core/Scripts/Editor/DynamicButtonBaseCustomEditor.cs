using UnityEditor;
using UnityEditor.UI;

namespace DynamicButtons {

    public abstract class DynamicButtonBaseCustomEditor : SelectableEditor {

        SerializedProperty iconField;
        SerializedProperty imageBackgroundField;
        SerializedProperty proceduralBackgroundField;
        SerializedProperty interactable;
        SerializedProperty backgroundType;
        SerializedProperty transitionType;
        SerializedProperty textProperties;
        SerializedProperty iconProperties;
        SerializedProperty imageBackgroundProperties;
        SerializedProperty proceduralBackgroundProperties;
        SerializedProperty animationTriggers;
        SerializedProperty navigationType;
        SerializedProperty onClickEvent;
        SerializedProperty onStateChangedEvent;

        protected override void OnEnable () {
            iconField = serializedObject.FindProperty ("iconField");
            imageBackgroundField = serializedObject.FindProperty ("imageBackgroundField");
            proceduralBackgroundField = serializedObject.FindProperty ("proceduralBackgroundField");
            interactable = serializedObject.FindProperty ("m_Interactable");
            backgroundType = serializedObject.FindProperty ("backgroundType");
            transitionType = serializedObject.FindProperty ("buttonTransitionType");
            textProperties = serializedObject.FindProperty ("textProperties");
            iconProperties = serializedObject.FindProperty ("iconProperties");
            imageBackgroundProperties = serializedObject.FindProperty ("imageBackgroundProperties");
            proceduralBackgroundProperties = serializedObject.FindProperty ("proceduralBackgroundProperties");
            animationTriggers = serializedObject.FindProperty ("m_AnimationTriggers");
            navigationType = serializedObject.FindProperty ("m_Navigation");
            onClickEvent = serializedObject.FindProperty ("onClickEvent");
            onStateChangedEvent = serializedObject.FindProperty ("onStateChangedEvent");
        }

        protected virtual void DrawChildFields () { }

        public override void OnInspectorGUI () {
            serializedObject.Update ();

            DrawChildFields ();
            EditorGUILayout.PropertyField (iconField);

            if (backgroundType.enumValueIndex == (int) DynamicButtonBase.DynamicButtonBackgroundType.IMAGE) {
                EditorGUILayout.PropertyField (imageBackgroundField);
            } else if (backgroundType.enumValueIndex == (int) DynamicButtonBase.DynamicButtonBackgroundType.PROCEDURAL) {
                EditorGUILayout.PropertyField (proceduralBackgroundField);
            }

            EditorGUILayout.PropertyField (interactable);
            EditorGUILayout.PropertyField (backgroundType);
            EditorGUILayout.PropertyField (transitionType);

            ++EditorGUI.indentLevel;

            if (transitionType.enumValueIndex == (int) DynamicButtonBase.DynamicButtonTransitionType.PROPERTY) {
                EditorGUILayout.PropertyField (textProperties, true);
                EditorGUILayout.PropertyField (iconProperties, true);
                if (backgroundType.enumValueIndex == (int) DynamicButtonBase.DynamicButtonBackgroundType.IMAGE) {
                    EditorGUILayout.PropertyField (imageBackgroundProperties, true);
                } else if (backgroundType.enumValueIndex == (int) DynamicButtonBase.DynamicButtonBackgroundType.PROCEDURAL) {
                    EditorGUILayout.PropertyField (proceduralBackgroundProperties, true);
                }
            } else if (transitionType.enumValueIndex == (int) DynamicButtonBase.DynamicButtonTransitionType.ANIMATION) {
                EditorGUILayout.PropertyField (animationTriggers);
            }

            --EditorGUI.indentLevel;

            EditorGUILayout.PropertyField (navigationType);
            EditorGUILayout.Space ();

            EditorGUILayout.PropertyField (onClickEvent);
            EditorGUILayout.PropertyField (onStateChangedEvent);

            serializedObject.ApplyModifiedProperties ();
        }
    }
}