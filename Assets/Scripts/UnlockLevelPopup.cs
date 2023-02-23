using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevelPopup : MonoBehaviour
{
    [System.Serializable]
    public struct CategoryName
    {
        public string name;
        public Sprite sprite;
    }
    public GameData gameData;
    public List<CategoryName> listCateogry;
    public GameObject winPopup;
    public Image catNameImage;
    private void Start()
    {
        this.winPopup.SetActive(false);
        GameEvents.OnUnlockNextCat += OnUnlockNextCat;
    }

    private void OnDisable()
    {
        GameEvents.OnUnlockNextCat -= OnUnlockNextCat;
    }

    private void OnUnlockNextCat()
    {
        bool captureNext = false;
        foreach(var element in listCateogry)
        {
            if(captureNext)
            {
                catNameImage.sprite = element.sprite;
                captureNext = false;
                break; //if false break
            }
            
            if(element.name == gameData.selCategoryName)
            {
                captureNext = true;
            }
        }
        winPopup.SetActive(true);
    }

}
