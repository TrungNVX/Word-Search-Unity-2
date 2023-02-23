using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public GameData gameData;
    public Text timerText;

    private float timeLeft;
    private float minutes;
    private float seconds;
    private float oneSecondDown;
    private bool timeOut;
    private bool stopTimer;
    private void Start()
    {
        this.stopTimer = false;
        this.timeOut = false;
        this.timeLeft = gameData.selBoardData.timeInSeconds;
        this.oneSecondDown = this.timeLeft - 1f;
        GameEvents.OnBoardCompleted += StopTimer;
        GameEvents.OnUnlockNextCat += StopTimer;
    }
    private void Update()
    {
        if (!this.stopTimer)
        {
            timeLeft -= Time.deltaTime;
        }
        if(this.timeLeft <= this.oneSecondDown)
        {
            this.oneSecondDown = this.timeLeft - 1f;
        }
    }
    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= StopTimer;
        GameEvents.OnUnlockNextCat -= StopTimer;
    }
    private void StopTimer()
    {
        this.stopTimer = true;
    }
    //Call everytime Unity redraw the UI system
    private void OnGUI()
    {
        if (!this.timeOut)
        {
            if(this.timeLeft > 0)
            {
                this.minutes = Mathf.Floor(this.timeLeft / 60);
                this.seconds = Mathf.RoundToInt(this.timeLeft % 60);
                this.timerText.text = this.minutes.ToString("00") + ":" + this.seconds.ToString("00");
            }
            else
            {
                this.stopTimer = true;
                ActivateGameOverGUI();
            }
        }
    }
    private void ActivateGameOverGUI()
    {
        GameEvents.GameOverMethod();
        this.timeOut = true;
    }
}
