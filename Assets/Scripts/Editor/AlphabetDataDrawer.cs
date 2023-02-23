using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using System.Text.RegularExpressions;
using UnityEditor;

[CustomEditor(typeof(AlphabetData))]
[CanEditMultipleObjects]
[System.Serializable]
public class AlphabetDataDrawer : Editor
{
    private ReorderableList PlainList;
    private ReorderableList NomarlList;
    private ReorderableList HighlightList;
    private ReorderableList WrongList;

    private void OnEnable()
    {
        InitList(ref PlainList, "_plainList", "Plain List");
        InitList(ref NomarlList, "_normList", "Normal List");
        InitList(ref HighlightList, "_highList", "Highlight List");
        InitList(ref WrongList, "_wrongList", "Wrong List");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        PlainList.DoLayoutList();
        NomarlList.DoLayoutList();
        HighlightList.DoLayoutList();
        WrongList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
    private void InitList(ref ReorderableList list, string propertyName, string listLabel)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName),
            true, true, true, true);
        list.drawHeaderCallback = (Rect Rect) =>
        {
            EditorGUI.LabelField(Rect, listLabel);
        };
        var _list = list;

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => 
        {
            var element = _list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("letter"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 30 - 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("image"), GUIContent.none);
        };

    }
}

