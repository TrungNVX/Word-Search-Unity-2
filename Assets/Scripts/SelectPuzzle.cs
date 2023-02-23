using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SelectPuzzle : MonoBehaviour
{
    public GameData gameData;
    public GameLevelData levelData;
    public Text categoryText;
    public Image progressBarFilling;

    private string sceneName = "GameScene";
    private bool levelLocked;
    private void Start()
    {
        this.levelLocked = false;
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        button.interactable = true;
        UpdateBtnInfor();
        button.interactable = this.levelLocked ? false : true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateBtnInfor()
    {
        var currentIndex = -1;
        var totalBoards = 0;

        foreach(var data in levelData.data)
        {
            if(data.categoryName == gameObject.name)
            {
                currentIndex = DataSaver.ReadCatIndex(gameObject.name);
                totalBoards = data.boardData.Count;

                if(levelData.data[0].categoryName == gameObject.name && currentIndex < 0)
                {
                    DataSaver.SaveCatData(levelData.data[0].categoryName, 0); //Unlock first level
                    currentIndex = DataSaver.ReadCatIndex(gameObject.name);
                    totalBoards = data.boardData.Count;
                }
            }
        }
        if(currentIndex == -1)
            this.levelLocked = true;
        this.categoryText.text = this.levelLocked ? string.Empty : (currentIndex.ToString() + "/" + totalBoards.ToString());
        progressBarFilling.fillAmount =
            (currentIndex > 0 && totalBoards > 0) ? ((float)currentIndex / (float)totalBoards) : 0f;
    }
    private void OnButtonClick()
    {
        gameData.selCategoryName = gameObject.name;
        SceneManager.LoadScene(sceneName);
    }

}
