using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    public GameObject gameOverPopup;
    public GameObject continueAfterAdsBtn;
    void Start()
    {
        continueAfterAdsBtn.GetComponent<Button>().interactable = false;
        gameOverPopup.SetActive(false);

        GameEvents.OnGameOver += OnGameOver;
    }
    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
    }
    private void OnGameOver()
    {
        gameOverPopup.SetActive(true);
        continueAfterAdsBtn.GetComponent<Button>().interactable = false;
    }
}
