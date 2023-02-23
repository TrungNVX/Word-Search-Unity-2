using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Text.RegularExpressions;

[CustomEditor(typeof(BoardData),false)]
[CanEditMultipleObjects]
[System.Serializable]
public class BoardDataDrawer : Editor
{
    //Create instance of boardData and assigned it as the object are
    //being inspected by editor
    private BoardData boardData => target as BoardData;
    //List of searching word
    private ReorderableList _wordList;

    private void OnEnable()
    {
        //gameDataInstance = target as BoardData;
        InitList(ref _wordList, "SearchWords", "Searching Words");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        boardData.timeInSeconds = EditorGUILayout.FloatField("Max Game Time (in secs)", boardData.timeInSeconds);

        DrawInputFields();
        EditorGUILayout.Space();
        ConvertToUpperButton();
        if(boardData.Board != null && boardData.cols > 0 && boardData.rows > 0)
            DrawBoardTable();
        GUILayout.BeginHorizontal();
        ClearBoardButton();
        FillRandomLetterButton();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();    
        _wordList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        if(GUI.changed)
        {
            EditorUtility.SetDirty(boardData);
        }
    }
    /// <summary>
    /// Draw the columns and rows (input fields)
    /// </summary>
    private void DrawInputFields()
    {
        var colsTemp = boardData.cols;
        var rowsTemp = boardData.rows;

        boardData.cols = EditorGUILayout.IntField("Columns", boardData.cols);
        boardData.rows = EditorGUILayout.IntField("Rows", boardData.rows);

        if((boardData.cols != colsTemp || boardData.rows != rowsTemp)
            && boardData.cols > 0 && boardData.rows > 0)
        {
            boardData.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColStyle = new GUIStyle();
        headerColStyle.fixedWidth = 35;

        var colStyle = new GUIStyle();
        colStyle.fixedWidth = 50;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.fixedWidth = 40;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var txtFieldStyle = new GUIStyle();
        txtFieldStyle.normal.background = Texture2D.grayTexture;
        txtFieldStyle.normal.textColor = Color.white;
        txtFieldStyle.fontStyle = FontStyle.Bold;
        txtFieldStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.BeginHorizontal(tableStyle);
        for(var i = 0; i < boardData.cols; i++)
        {
            EditorGUILayout.BeginVertical(i == -1 ? headerColStyle : colStyle);
            for(var j = 0; j < boardData.rows; j++)
            {
                if(i >= 0 && j >= 0)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var character = (string)EditorGUILayout.TextArea(boardData.Board[i].Row[j],txtFieldStyle);
                    if (boardData.Board[i].Row[j].Length > 1)
                    {
                        character = boardData.Board[i].Row[j].Substring(0, 1);
                    }
                    boardData.Board[i].Row[j] = character;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    //Initialize the reorderable list
    private void InitList(ref ReorderableList list, string propertyName, string listLable)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName),
            true, true, true, true);
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLable);
        };

        var _list = list;

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = _list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), 
                element.FindPropertyRelative("word"), GUIContent.none);
        };
    }

    private void ConvertToUpperButton()
    {
        if(GUILayout.Button("To Upper"))
        {
            for(var i = 0; i < boardData.cols; i++)
            {
                for(var j = 0; j < boardData.rows; j++)
                {
                    var count = Regex.Matches(boardData.Board[i].Row[j], @"[a-z]").Count;
                    if(count > 0)
                    {
                        boardData.Board[i].Row[j] = boardData.Board[i].Row[j].ToUpper();
                    }
                }
            }
            foreach(var searchWord in boardData.SearchWords)
            {
                var count = Regex.Matches(searchWord.word, @"[a-z]").Count;
                if (count > 0)
                {
                    searchWord.word = searchWord.word.ToUpper();
                }
            }
        }
    }
    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            for (var i = 0; i < boardData.cols; i++)
            {
                for (var j = 0; j < boardData.rows; j++)
                {
                    boardData.Board[i].Row[j] = string.Empty;
                }
            }
        }
    }
    private void FillRandomLetterButton()
    {
        if (GUILayout.Button("Fill Random Letters"))
        {
            for (var i = 0; i < boardData.cols; i++)
            {
                for (var j = 0; j < boardData.rows; j++)
                {
                    string letter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    int index = UnityEngine.Random.Range(0, letter.Length);
                    if (boardData.Board[i].Row[j] == string.Empty)
                    {
                        boardData.Board[i].Row[j] = letter[index].ToString();
                    }
                }
            }
        }
    }
}
