using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class BoardData : ScriptableObject
{
    [System.Serializable]
    public class SearchingWords
    {
        [HideInInspector]
        public bool isFound = false;
        public string word;
    }
    [System.Serializable]
    public class BoardRow
    {
        public int size;
        public string[] Row;

        public BoardRow(int size) 
        {
            CreateRow(size);
        }
        /// <summary>
        /// Create an array of rows
        /// </summary>
        /// <param name="size"> number of rows </param>
        public void CreateRow(int size)
        {
            this.size = size;
            this.Row = new string[this.size];
            ClearRow();
        }
        /// <summary>
        /// Clear the Row with empty string
        /// </summary>
        public void ClearRow()
        {
            for(int i = 0; i < this.size; i++)
            {
                Row[i] = string.Empty;
            }
        }
    }
    public float timeInSeconds; //time to solve the puzzle in game
    public int cols = 0;
    public int rows = 0;
    public BoardRow[] Board;

    public List<SearchingWords> SearchWords = new List<SearchingWords>();

    public void ClearData()
    {
        foreach(var word in SearchWords)
        {
            word.isFound = false;
        }
    }
    public void ClearWithEmptyString()
    {
        for(int i = 0; i < cols; i++)
        {
            Board[i].ClearRow();
        }
    }
    public void CreateNewBoard()
    {
        Board = new BoardRow[cols];
        for(int i = 0; i < cols; i++)
        {
            Board[i] = new BoardRow(rows);
        }
    }
}
