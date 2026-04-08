using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using TD.Util;

namespace TD.Util {
    [CustomEditor(typeof(FXManager))]
    public class FXManagerEditor : Editor {

        SerializedObject _class;
        ReorderableList reorderableList;

        private void OnEnable() {
            _class = new SerializedObject((FXManager)target);
            SerializedProperty _mergeItems = _class.FindProperty("fxPrefabs");
            reorderableList = new ReorderableList(_class, _mergeItems, true, true, true, true);
            SetUpListCallBacks();
        }

        private void SetUpListCallBacks() {
            //
            float elementWidth1 = 120;
            float elementWidth3 = 40;
            reorderableList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, elementWidth1, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("prefabObj"), GUIContent.none);
                EditorGUI.PropertyField(
                    new Rect(rect.x + elementWidth1, rect.y, rect.width - elementWidth1 - elementWidth3, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("prefabType"), GUIContent.none);
                EditorGUI.PropertyField(
                    new Rect(rect.x + rect.width - elementWidth3, rect.y, elementWidth3, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("prefabPoolCount"), GUIContent.none);
            };

            reorderableList.drawHeaderCallback = (Rect rect) => {
                DropAreaGUI("FX Pool", rect);
                RightClickArea(rect);
            };
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            reorderableList.DoLayoutList();
            _class.ApplyModifiedProperties();
        }

        // Custom
        public void DropAreaGUI(string label, Rect rect) {
            EditorGUI.LabelField(rect, "FX Pool");
            Event evt = Event.current;
            Rect drop_area = rect;
            //GUI.Box(drop_area, label);

            switch (evt.type) {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!drop_area.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform) {
                        DragAndDrop.AcceptDrag();

                        int itemLevel = 1;
                        foreach (Object dragged_object in DragAndDrop.objectReferences) {
                            // Do On Drag Stuff here
                            AddToList(dragged_object, itemLevel);
                            itemLevel++;
                        }
                    }
                    break;
            }
        }

        public void RightClickArea(Rect rect) {
            Event e = Event.current;
            if (rect.Contains(e.mousePosition) && e.type == EventType.ContextClick) {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Reset list"), false, ClearList);
                menu.ShowAsContext();
                e.Use();
            }
        }

        private void ClearList() {
            reorderableList.serializedProperty.arraySize = 0;
        }

        private void AddToList(Object obj, int itemLevel = 1) {
            var index = reorderableList.serializedProperty.arraySize;
            reorderableList.serializedProperty.arraySize++;
            reorderableList.index = index;
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("prefabObj").objectReferenceValue = obj as GameObject;
            element.FindPropertyRelative("prefabType").enumValueIndex = 0;
            element.FindPropertyRelative("prefabPoolCount").intValue = itemLevel;
        }

    }
}