using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{
    public GameData currentData;
    public GameObject squarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float topPosition = 0.0f;

    private List<GameObject> _squareList = new List<GameObject>();
    private void Start()
    {
        SpawnSquares();
        SetSquarePosition();
    }
    //Spawn the squares
    private void SpawnSquares()
    {
        if(currentData != null)
        {
            var squareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));
            foreach(var squares in currentData.selBoardData.Board)
            {
                foreach(var letter in squares.Row)
                {
                    var normLetter = alphabetData._normList.Find(data => data.letter == letter);
                    var selLetter = alphabetData._wrongList.Find(data => data.letter == letter);
                    var correctLetter = alphabetData._highList.Find(data => data.letter == letter);
                    if(normLetter.image == null || selLetter.image == null)
                    {
                        Debug.LogError("Fill the letter: " + letter);
                    #if UNITY_EDITOR
                        if(UnityEditor.EditorApplication.isPlaying)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;
                        }
                    #endif
                    }
                    else
                    {
                        _squareList.Add(Instantiate(squarePrefab));
                        _squareList[_squareList.Count - 1].GetComponent<GridSquare>().SetSprite(
                            normLetter, selLetter, correctLetter);
                        _squareList[_squareList.Count - 1].transform.SetParent(this.transform);
                        _squareList[_squareList.Count - 1].GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
                        _squareList[_squareList.Count - 1].transform.localScale = squareScale;
                        _squareList[_squareList.Count - 1].GetComponent<GridSquare>().SetIndex(_squareList.Count - 1);
                    }
                        
                }
            }
        }
    }
    /// <summary>
    /// Return the calculated scale of the square prefab
    /// </summary>
    /// <param name="defaultScale"> initial scale customed by user by</param>
    /// <returns></returns>
    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;
        while(IsScaledDown(finalScale))
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
    /// <summary>
    /// Return true if the scale still need to be down
    /// </summary>
    /// <param name="targetScale"> </param>
    /// <returns></returns>
    /// Check the squarePrefab is outside of the screen boundaries
    private bool IsScaledDown(Vector3 targetScale)
    {
        var squareRect = squarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize = new Vector2(0f, 0f);
        var startPosition = new Vector2(0f, 0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareRect.height * targetScale.x) + squareOffset;

        //1 Unity units = 100 pixel;
        var midWidthPoint = ((currentData.selBoardData.cols * squareSize.x) / 2) * 0.01f;
        var midHeightPoint = ((currentData.selBoardData.rows * squareSize.x) / 2) * 0.01f;
        startPosition.x = (midWidthPoint > 0) ? midWidthPoint * (-1) : midWidthPoint;
        startPosition.y = midHeightPoint;

        return startPosition.x < HalfScreenWidth() * -1 || startPosition.y > topPosition;
    }

    private float HalfScreenWidth()
    {
        var worldHeight = Camera.main.orthographicSize * 2;
        var worldWidth = worldHeight * Screen.width / Screen.height;
        return worldWidth / 2;
    }
    
    private void SetSquarePosition()
    {
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTrans = _squareList[0].GetComponent<Transform>();

        var offset = new Vector2
        {
            x = (squareRect.width * squareTrans.localScale.x + squareOffset) * 0.01f,
            y = (squareRect.height * squareTrans.localScale.y + squareOffset) * 0.01f
        };

        var startPosition = GetFirstSquarePosition();
        int colNum = 0;
        int rowNum = 0;

        foreach(var square in _squareList )
        {
            if(rowNum + 1 > currentData.selBoardData.rows)
            {
                colNum++;
                rowNum = 0;
            }
            var PositionX = startPosition.x + offset.x * colNum;
            var PositionY = startPosition.y - offset.y * rowNum;

            square.GetComponent<Transform>().position = new Vector2(PositionX, PositionY);
            rowNum++;
        }
    }
    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2(0f, this.transform.position.y);
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTrans = _squareList[0].GetComponent<Transform>();
        var squareSize = new Vector2(0f, 0f);
        
        squareSize.x = squareRect.width * squareTrans.localScale.x;
        squareSize.y = squareRect.height* squareTrans.localScale.y;

        var midWidthPoint = (((currentData.selBoardData.cols - 1) * squareSize.x) / 2) * 0.01f;
        var midHeightPoint = (((currentData.selBoardData.rows - 1) * squareSize.y) / 2) * 0.01f;

        startPosition.x = midWidthPoint != 0 ? midWidthPoint * -1 : midWidthPoint;
        startPosition.y += midHeightPoint;

        return startPosition;
    }
}
