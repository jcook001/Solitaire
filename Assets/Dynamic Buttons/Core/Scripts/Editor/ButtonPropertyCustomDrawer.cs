using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DynamicButtons {

    public abstract class ButtonPropertyCustomDrawer : PropertyDrawer {

        private const float removeButtonWidth = 20f;
        private const float horizontalSpacing = 4f;

        private bool isExpanded = false;
        private int stateSelectionIndex = 0;
        private List<string> availableStatesList = new List<string> ();
        private List<SerializedProperty> availableStatesPropertiesList = new List<SerializedProperty> ();

        public override void OnGUI (UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label) {
            EditorGUI.BeginProperty (position, label, property);

            position.height = EditorGUIUtility.singleLineHeight;

            isExpanded = EditorGUI.Foldout (position, isExpanded, label, true);

            if (isExpanded) {
                EditorGUI.indentLevel++;
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawPropertyMainContent (position, property);
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty ();
        }

        private void drawPropertyMainContent (UnityEngine.Rect position, SerializedProperty property) {
            SerializedProperty highlightedStateEnabledProperty = property.FindPropertyRelative ("highlightedEnabled");
            SerializedProperty selectedStateEnabledProperty = property.FindPropertyRelative ("selectedEnabled");
            SerializedProperty pressedStateEnabledProperty = property.FindPropertyRelative ("pressedEnabled");
            SerializedProperty disabeldStateEnabledProperty = property.FindPropertyRelative ("disabledEnabled");

            setupStatesAvailableToAdd (highlightedStateEnabledProperty,
                selectedStateEnabledProperty,
                pressedStateEnabledProperty,
                disabeldStateEnabledProperty);

            Rect drawRect = position;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField (drawRect, property.FindPropertyRelative ("defaultValue"), new GUIContent ("Default"));
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (highlightedStateEnabledProperty.boolValue) {
                addRemoveablePropertyField (property, highlightedStateEnabledProperty, drawRect, "highlightedValue", "Highlighted");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            if (pressedStateEnabledProperty.boolValue) {
                addRemoveablePropertyField (property, pressedStateEnabledProperty, drawRect, "pressedValue", "Pressed");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
#if UNITY_2019 || UNITY_2020
            if (selectedStateEnabledProperty.boolValue) {
                addRemoveablePropertyField (property, selectedStateEnabledProperty, drawRect, "selectedValue", "Selected");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
#endif
            if (disabeldStateEnabledProperty.boolValue) {
                addRemoveablePropertyField (property, disabeldStateEnabledProperty, drawRect, "disabledValue", "Disabled");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            if (availableStatesList.Count > 0) {
                addNewStateCreator (drawRect);
            }
        }

        private void setupStatesAvailableToAdd (
            SerializedProperty highlighted,
            SerializedProperty selected,
            SerializedProperty pressed,
            SerializedProperty disabled) {

            availableStatesList.Clear ();
            availableStatesPropertiesList.Clear ();

            if (!highlighted.boolValue) {
                availableStatesList.Add ("Highlighted");
                availableStatesPropertiesList.Add (highlighted);
            }
            if (!pressed.boolValue) {
                availableStatesList.Add ("Pressed");
                availableStatesPropertiesList.Add (pressed);
            }
#if UNITY_2019 || UNITY_2020
            if (!selected.boolValue) {
                availableStatesList.Add ("Selected");
                availableStatesPropertiesList.Add (selected);
            }
#endif
            if (!disabled.boolValue) {
                availableStatesList.Add ("Disabled");
                availableStatesPropertiesList.Add (disabled);
            }
        }

        private void setPropertyStateEnabled (SerializedProperty property, string name, bool enabled) {
            SerializedProperty stateProperty = property.FindPropertyRelative (name);
            stateProperty.boolValue = enabled;
        }

        private void addRemoveablePropertyField (SerializedProperty property,
            SerializedProperty propertyToDisable,
            Rect rect,
            string propertyName,
            string title
        ) {
            Rect fieldRect = new Rect (rect);
            fieldRect.width -= removeButtonWidth + horizontalSpacing;

            Rect removeButtonRect = new Rect (rect);
            removeButtonRect.x = fieldRect.x + fieldRect.width + horizontalSpacing;
            removeButtonRect.width = removeButtonWidth;

            EditorGUI.PropertyField (
                fieldRect,
                property.FindPropertyRelative (propertyName),
                new GUIContent (title)
            );

            if (GUI.Button (removeButtonRect, "X", EditorStyles.miniButton)) {
                propertyToDisable.boolValue = false;
            }
        }

        private void addNewStateCreator (Rect rect) {
            Rect popupRect = new Rect (rect);
            popupRect.width /= 2;

            Rect buttonRect = new Rect (rect);
            buttonRect.width = popupRect.width - horizontalSpacing;
            buttonRect.x = popupRect.x + popupRect.width + horizontalSpacing;

            stateSelectionIndex = EditorGUI.Popup (popupRect, stateSelectionIndex, availableStatesList.ToArray ());

            if (GUI.Button (buttonRect, "Add state value", EditorStyles.miniButton)) {
                availableStatesPropertiesList[stateSelectionIndex].boolValue = true;
                stateSelectionIndex = 0;
            }
        }

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
            SerializedProperty highlightedStateEnabledProperty = property.FindPropertyRelative ("highlightedEnabled");
            SerializedProperty selectedStateEnabledProperty = property.FindPropertyRelative ("selectedEnabled");
            SerializedProperty pressedStateEnabledProperty = property.FindPropertyRelative ("pressedEnabled");
            SerializedProperty disabeldStateEnabledProperty = property.FindPropertyRelative ("disabledEnabled");

            int linesCount = 1;

            if (isExpanded) {
                linesCount++;

                bool addStateButtonVisible = false;

                if (highlightedStateEnabledProperty.boolValue)
                    linesCount++;
                else
                    addStateButtonVisible = true;

                if (selectedStateEnabledProperty.boolValue)
                    linesCount++;
                else
                    addStateButtonVisible = true;

                if (pressedStateEnabledProperty.boolValue)
                    linesCount++;
                else
                    addStateButtonVisible = true;

                if (disabeldStateEnabledProperty.boolValue)
                    linesCount++;
                else
                    addStateButtonVisible = true;

                if (addStateButtonVisible)
                    linesCount++;
            }

            return linesCount * EditorGUIUtility.singleLineHeight + (linesCount - 1) * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}