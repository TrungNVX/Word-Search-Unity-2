using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public GameLevelData levelData;

    public void ResetGameProgress()
    {
        DataSaver.ClearGameData(this.levelData);
    }
}
