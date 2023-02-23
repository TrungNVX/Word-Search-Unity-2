using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingWordsList : MonoBehaviour
{
    public GameData currentData;
    public GameObject wordPrefab;
    public float offset = 0.0f;
    public int maxCols = 5;
    public int maxRows = 3;


    private int cols = 2;
    private int rows;
    private int wordsNum;

    private List<GameObject> wordsList = new List<GameObject>();
    void Start()
    {
        this.wordsNum = currentData.selBoardData.SearchWords.Count;
        if(this.wordsNum < this.cols)
            this.rows = 1;
        else
            GetColAndRowNum();
        SpawnWordObj();
        SetPosition();
    }
    //Prevent create more rows number than maxRows,
    //increase cols number instead
    private void GetColAndRowNum()
    {
        do 
        {
            this.cols++;
            this.rows = this.wordsNum / this.cols;
        }
        while(this.rows >= this.maxRows);
        if(this.cols > this.maxCols)
        {
            this.cols = this.maxCols;
            this.rows = this.wordsNum / this.cols;
        }
    }
    private bool TryIncreaseColNum()
    {
        this.cols++;
        this.rows = this.wordsNum / this.cols;
        if(this.cols > this.maxCols)
        {
            this.cols = this.maxCols;
            this.rows = this.wordsNum / this.cols;
            return false;
        }
        if(this.wordsNum % this.cols > 0)
            rows++;
        return true;
    }
    private void SpawnWordObj()
    {
        var squareScale = GetSquareScale(new Vector3(1f, 1f, 0.1f));

        for(var i = 0; i < this.wordsNum; i++)
        {
            this.wordsList.Add(Instantiate(this.wordPrefab) as GameObject);
            this.wordsList[i].transform.SetParent(this.transform);
            this.wordsList[i].GetComponent<RectTransform>().localScale = squareScale;
            this.wordsList[i].GetComponent<RectTransform>().localPosition = Vector3.zero;
            this.wordsList[i].GetComponent<SearchingWord>().SetWord(this.currentData.selBoardData.SearchWords[i].word);
        }
    }
    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;
        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;
            if(finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;
                return finalScale;
            }
        }
        return finalScale;
    }
    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = this.wordPrefab.GetComponent<RectTransform>();
        var parentRect = this.GetComponent<RectTransform>();

        var squareSize = Vector2.zero;

        squareSize.x = squareRect.rect.width * targetScale.x + this.offset;
        squareSize.y = squareRect.rect.height * targetScale.x + this.offset;

        var totalHeight = squareSize.y * this.rows;
        //Make sure all the square fit in the parent rect area
        if(totalHeight > parentRect.rect.height)
        {
            while(totalHeight > parentRect.rect.height)
            {
                if(TryIncreaseColNum())
                    totalHeight = squareSize.y * this.rows;
                else
                    return true; //Continue scale down
            }
        }
        var totalWidth = squareSize.x * this.cols;

        if(totalWidth > parentRect.rect.width)
            return true;
        return false;
    }
    private void SetPosition()
    {
        var squareRect = this.wordsList[0].GetComponent<RectTransform>();
        var wordOffset = new Vector2
        {
            x = squareRect.rect.width * squareRect.transform.localScale.x + this.offset,
            y = squareRect.rect.height * squareRect.transform.localScale.y + this.offset
        };

        int colNum = 0;
        int rowNum = 0;
        var startPos = GetFirstSquarePos();

        foreach(var word in this.wordsList )
        {
            if(colNum + 1 > this.cols)
            {
                colNum = 0;
                rowNum++;
            }

            var PosX = startPos.x + wordOffset.x * colNum;
            var PosY = startPos.y - wordOffset.y * rowNum;

            word.GetComponent<RectTransform>().localPosition = new Vector2(PosX, PosY);
            colNum++;
        }
    }
    private Vector2 GetFirstSquarePos()
    {
        var startPos = new Vector2(0f, this.transform.position.y);
        var squareRect = this.wordsList[0].GetComponent<RectTransform>();
        var parentRect = this.GetComponent <RectTransform>();
        var squareSize = Vector2.zero;

        squareSize.x = squareRect.rect.width * squareRect.transform.localScale.x + this.offset;
        squareSize.y = squareRect.rect.height * squareRect.transform.localScale.y + this.offset;

        //Make sure it is in the center

        var shiftBy = (parentRect.rect.width - (squareSize.x * this.cols)) / 2;

        startPos.x = ((parentRect.rect.width - squareSize.x) / 2) * (-1);
        startPos.x += shiftBy;
        startPos.y = (parentRect.rect.height - squareSize.y) / 2;

        return startPos;
    }
}
