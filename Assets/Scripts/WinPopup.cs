using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopup : MonoBehaviour
{
    public GameObject winPopup;
    // Start is called before the first frame update
    private void Start()
    {
        this.winPopup.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += OnBoardCompleted;
    }
    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= OnBoardCompleted;
    }

    private void OnBoardCompleted()
    {
        this.winPopup.SetActive(true);
    }
    public void LoadNextLevel()
    {
        GameEvents.LoadNextLevelMethod();
    }
}
