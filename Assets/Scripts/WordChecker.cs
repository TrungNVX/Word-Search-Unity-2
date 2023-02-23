using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordChecker : MonoBehaviour
{
    public GameData currentData;
    public GameLevelData levelData;

    private string word;

    private int assignedPoints = 0;
    private int completedWords = 0;
    private Ray rayUp, rayDown;
    private Ray rayRight, rayLeft;
    private Ray rayRightUp, rayLeftUp;
    private Ray rayRightDown, rayLeftDown;
    private Ray currentRay = new Ray();
    private Vector3 rayStartPosition;
    private List<int> correctList = new List<int>();
    private void OnEnable()
    {
        GameEvents.OnCheckSquare += OnCheckSquare;
        GameEvents.OnClearSelection += OnClearSelection;
        GameEvents.OnLoadNextLevel += OnLoadNextLevel;
    }
    private void OnDisable()
    {
        GameEvents.OnCheckSquare -= OnCheckSquare;
        GameEvents.OnClearSelection -= OnClearSelection;
        GameEvents.OnLoadNextLevel -= OnLoadNextLevel;
    }
    void Start()
    {
        //Every time start, the old data will clear
        this.currentData.selBoardData.ClearData();
        this.assignedPoints = 0;
        this.completedWords = 0;
    }
    void Update()
    {
        
    }
    private void OnCheckSquare(string letter, Vector3 pos, int index)
    {
        if(this.assignedPoints == 0)
        {
            this.rayStartPosition = pos;
            this.correctList.Add(index);
            this.word += letter;

            this.rayUp = new Ray(new Vector2(pos.x, pos.y), Vector2.up);
            this.rayDown = new Ray(new Vector2(pos.x, pos.y), Vector2.down);
            this.rayLeft = new Ray(new Vector2(pos.x, pos.y), Vector2.left);
            this.rayRight = new Ray(new Vector2(pos.x, pos.y), Vector2.right);
            this.rayRightUp = new Ray(new Vector2(pos.x, pos.y), new Vector2(1, 1));
            this.rayRightDown = new Ray(new Vector2(pos.x, pos.y), new Vector2(1, -1));
            this.rayLeftUp = new Ray(new Vector2(pos.x, pos.y), new Vector2(-1, 1));
            this.rayLeftDown = new Ray(new Vector2(pos.x, pos.y), new Vector2(-1, -1));
        }
        else if(this.assignedPoints == 1)
        {
            this.correctList.Add(index);
            this.currentRay = this.SelRay(this.rayStartPosition, pos);
            GameEvents.SelectSquareMethod(pos);
            this.word += letter;
            this.CheckWord();
        }
        else
        {
            if(IsPointOnTheRay(this.currentRay, pos))
            {
                this.correctList.Add(index);
                GameEvents.SelectSquareMethod(pos);
                this.word += letter;
                this.CheckWord();
            }
        }
        this.assignedPoints++;
    }
    private void CheckWord()
    {
        foreach(var searchingWord in currentData.selBoardData.SearchWords)
        {
            if((this.word == searchingWord.word || this.word == Utilities.ReverseWord(searchingWord.word)) && !searchingWord.isFound)
            {
                searchingWord.isFound = true;
                GameEvents.CorrectWordMethod(this.word, this.correctList);
                this.completedWords++;
                this.word = string.Empty;
                this.correctList.Clear();
                this.CheckBoardCompleted();
                return;
            }
        }
    }
    private bool IsPointOnTheRay(Ray currentRay, Vector3 point)
    {
        var hits = Physics.RaycastAll(currentRay, 100.0f);
        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point)
                return true;
        }
        return false;
    }
    /// <summary>
    /// Create the Ray
    /// </summary>
    /// <param name="firstPoint"></param>
    /// <param name="secondPoint"></param>
    /// <returns></returns>
    private Ray SelRay(Vector2 firstPoint, Vector2 secondPoint)
    {
        var dir = (secondPoint - firstPoint).normalized;
        float tolerance = 0.01f;
        if(Math.Abs(dir.x) < tolerance && Math.Abs(dir.y - 1f) < tolerance)
        {
            return this.rayUp;
        }
        if(Math.Abs(dir.x) < tolerance && Math.Abs(dir.y - (-1f)) < tolerance)
        {
            return this.rayDown;
        }
        if(Math.Abs(dir.x - 1f) < tolerance && Math.Abs(dir.y) < tolerance)
        {
            return this.rayRight;
        }
        if(Math.Abs(dir.x - (-1f)) < tolerance && Math.Abs(dir.y) < tolerance)
        {
            return this.rayLeft;
        }
        if(dir.x > 0f && dir.y > 0f)
        {
            return this.rayRightUp;
        }
        if(dir.x > 0f && dir.y < 0f)
        {
            return this.rayRightDown;
        }
        if(dir.x < 0f && dir.y > 0f)
        {
            return this.rayLeftUp;
        }
        if(dir.x < 0f && dir.y < 0f)
        {
            return this.rayLeftDown;
        }
        return this.rayDown;
    }
    private void OnClearSelection()
    {
        this.assignedPoints = 0;
        this.correctList.Clear();
        this.word = string.Empty;
    }
    public void CheckBoardCompleted()
    {
        bool loadNextCat = false;

        if(currentData.selBoardData.SearchWords.Count == this.completedWords)
        {
            //Save current level progress
            var catName = currentData.selCategoryName;
            var curBoardIndex = DataSaver.ReadCatIndex(catName);
            var nextBoardIndex = -1;
            var curCatIndex = 0;
            bool readNextLevel = false;
            for(int index = 0; index < this.levelData.data.Count; index++) 
            { 
                if(readNextLevel)
                {
                    nextBoardIndex = DataSaver.ReadCatIndex(levelData.data[index].categoryName);
                    readNextLevel = false;
                }
                if(levelData.data[index].categoryName == catName)
                {
                    readNextLevel = true;
                    curCatIndex = index;
                }
            }
            var curLevelSize = levelData.data[curCatIndex].boardData.Count;
            if(curBoardIndex < curLevelSize)
                curBoardIndex++;
            DataSaver.SaveCatData(catName, curBoardIndex);

            //Unlock next category
            if(curBoardIndex >= curLevelSize)
            {
                curCatIndex++;
                if(curCatIndex < this.levelData.data.Count) //if this is not the last cat
                {
                    catName = this.levelData.data[curCatIndex].categoryName;
                    curBoardIndex = 0;
                    loadNextCat = true;

                    if(nextBoardIndex <= 0)
                        DataSaver.SaveCatData(catName, curBoardIndex);
                }
                else
                {
                    SceneManager.LoadScene("SelectCategory");
                }
            }
            else
            {
                GameEvents.BoardCompletedMethod();
            }

            if(loadNextCat)
            {
                GameEvents.UnlockNextCatMethod();
            }
        }
    }
    private void OnLoadNextLevel()
    {
        SceneManager.LoadScene("GameScene");
    }
}
