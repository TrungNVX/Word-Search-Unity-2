using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSelector : MonoBehaviour
{
    public GameData gameData;
    public GameLevelData levelData;
    private void Awake()
    {
        SelSeqBoardData();
    }

    private void SelSeqBoardData()
    {
        foreach(var data in this.levelData.data)
        {
            if(data.categoryName == this.gameData.selCategoryName)
            {
                var boardIndex = DataSaver.ReadCatIndex(gameData.selCategoryName);

                if(boardIndex < data.boardData.Count)
                {
                    this.gameData.selBoardData = data.boardData[boardIndex];
                }
                else
                {
                    var randomIndex = Random.Range(0, data.boardData.Count);
                    this.gameData.selBoardData = data.boardData[randomIndex];
                }
            }
        }
    }
}
