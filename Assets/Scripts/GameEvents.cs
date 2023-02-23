using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public delegate void EnableSquareSelection();
    public static event EnableSquareSelection OnEnableSquareSelection;
    public static void EnableSquareSelectionMethod()
    {
        if(OnEnableSquareSelection != null)
            OnEnableSquareSelection();
    }

    public delegate void DisableSquareSelection();
    public static event DisableSquareSelection OnDisableSquareSelection;
    public static void DisableSquareSelectionMethod()
    {
        if (OnDisableSquareSelection != null)
            OnDisableSquareSelection();
    }
    
    public delegate void SelectSquare(Vector3 position);
    public static event SelectSquare OnSelectSquare;
    public static void SelectSquareMethod(Vector3 position)
    {
        if (OnSelectSquare != null)
            OnSelectSquare(position);
    }

    /// <summary>
    /// Check if the selected square is a correct one or the wrong one
    /// </summary>
    /// <param name="letter"></param> the letter of the corresponding prefab
    /// <param name="position"></param> the postition of the prefab
    /// <param name="index"></param> the index of the prefab
    public delegate void CheckSquare(string letter, Vector3 position, int index);
    public static event CheckSquare OnCheckSquare;
    public static void CheckSquareMethod(string letter, Vector3 position, int index)
    {
        if (OnCheckSquare != null)
            OnCheckSquare(letter, position, index);
    }
    //Release mouse or liftup finger
    public delegate void ClearSelection();
    public static event ClearSelection OnClearSelection;
    public static void ClearSelectionMethod()
    {
        if (OnClearSelection != null)
            OnClearSelection();
    }
    public delegate void CorrectWord(string word, List<int> prefabIndices);
    public static event CorrectWord OnCorrectWord;
    public static void CorrectWordMethod(string word, List<int> prefabIndices)
    {
        if(OnCorrectWord != null)
        {
            OnCorrectWord(word, prefabIndices);
        }
    }

    public delegate void BoardCompleted();
    public static event BoardCompleted OnBoardCompleted;
    public static void BoardCompletedMethod()
    {
        if(OnBoardCompleted!=null) 
            OnBoardCompleted();
    }

    public delegate void UnlockNextCat();
    public static event UnlockNextCat OnUnlockNextCat;
    public static void UnlockNextCatMethod()
    {
        if(OnUnlockNextCat != null)
        {
            OnUnlockNextCat();
        }
            
    }

    public delegate void LoadNextLevel();
    public static event LoadNextLevel OnLoadNextLevel;
    public static void LoadNextLevelMethod()
    {
        if (OnLoadNextLevel != null)
            OnLoadNextLevel();
    }


    public delegate void GameOver();
    public static event GameOver OnGameOver;
    public static void GameOverMethod()
    {
        if (OnGameOver != null)
            OnGameOver();
    }

    public delegate void ToggleSoundFx();
    public static event ToggleSoundFx OnToggleSoundFx;
    public static void ToggleSoundFxMethod()
    {
        if (OnToggleSoundFx != null)
            OnToggleSoundFx();
    }
}
