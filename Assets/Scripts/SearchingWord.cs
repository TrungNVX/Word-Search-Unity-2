using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{
    public Text displayedText;
    public Image crossLine;

    private string word;
    private string reversWord;
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        GameEvents.OnCorrectWord += OnCorrectWord;
    }
    private void OnDisable()
    {
        GameEvents.OnCorrectWord -= OnCorrectWord;
    }
    public void SetWord(string word)
    {
        this.word = word;
        this.reversWord = Utilities.ReverseWord(this.word);
        this.displayedText.text = word;
    }
    private void OnCorrectWord(string word, List<int> prefabIndices)
    {
        if(word == this.word || word == this.reversWord)
        {
            crossLine.gameObject.SetActive(true);
        }
    }
}
