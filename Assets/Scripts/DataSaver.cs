using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Save player progress data.
/// </summary>
public class DataSaver : MonoBehaviour
{
    /// <summary>
    /// Read category name to return how many puzzle have been solved
    /// </summary>
    /// <param name="name"></param> name of the category
    /// <returns></returns> value is number of solved puzzles
    public static int ReadCatIndex(string name) //Cat = Category
    {
        var value = -1;
        //Check how many puzzle have solved from caterogy to display on the screen
        if(PlayerPrefs.HasKey(name))
            value = PlayerPrefs.GetInt(name);
        return value;
    }
    /// <summary>
    /// Save the category data
    /// </summary>
    /// <param name="catName"></param> category name
    /// <param name="currentIndex"></param> number of current solved puzzles
    public static void SaveCatData(string catName, int currentIndex)
    {
        PlayerPrefs.SetInt(catName, currentIndex);
        PlayerPrefs.Save();
    }
    public static void ClearGameData(GameLevelData levelData)
    {
        foreach(var data in levelData.data)
        {
            PlayerPrefs.SetInt(data.categoryName, -1);
        }
        //Unlock first level
        PlayerPrefs.SetInt(levelData.data[0].categoryName, 0); //0 means no puzzle solved
        PlayerPrefs.Save();
    }
}
