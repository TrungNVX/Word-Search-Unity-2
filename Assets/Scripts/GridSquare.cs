using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    public int SquareIndex { get; set; }

    private AlphabetData.LetterData normLetter;
    private AlphabetData.LetterData selLetter;
    private AlphabetData.LetterData correctLetter;

    private SpriteRenderer displayedImage;
    private bool selected;
    private bool clicked;
    private bool corrected;
    private int index = -1;

    private AudioSource source;


    public void SetIndex(int index)
    {
        this.index = index;
    }

    public int GetIndex() 
    { 
        return index; 
    }

    private void Start()
    {
        this.selected = false;
        this.clicked = false;
        this.corrected = false;
        this.displayedImage = GetComponent<SpriteRenderer>();
        this.source = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        GameEvents.OnEnableSquareSelection += OnEnableSquareSelection;
        GameEvents.OnSelectSquare += OnSelectSquare;
        GameEvents.OnDisableSquareSelection += OnDisableSquareSelection;
        GameEvents.OnCorrectWord += OnCorrectWord;
    }

    private void OnDisable()
    {
        GameEvents.OnEnableSquareSelection -= OnEnableSquareSelection;
        GameEvents.OnSelectSquare -= OnSelectSquare;
        GameEvents.OnDisableSquareSelection -= OnDisableSquareSelection;
        GameEvents.OnCorrectWord -= OnCorrectWord;
    }
    public void SetSprite(AlphabetData.LetterData normLetter, AlphabetData.LetterData selLetter
        , AlphabetData.LetterData correctLetter)
    {
        this.normLetter = normLetter;
        this.selLetter = selLetter;
        this.correctLetter = correctLetter;

        GetComponent<SpriteRenderer>().sprite = normLetter.image;
    }
    public void OnEnableSquareSelection()
    {
        this.clicked = true;
        this.selected = false;
    }
    public void OnSelectSquare(Vector3 position)
    {
        if(this.gameObject.transform.position == position)
        {
            this.displayedImage.sprite = this.selLetter.image;
        }
    }
    public void OnDisableSquareSelection()
    {
        this.clicked = false;
        this.selected = false;
        if(this.corrected)
            this.displayedImage.sprite = this.correctLetter.image;
        else
            this.displayedImage.sprite = this.normLetter.image;
    }
    private void OnMouseDown()
    {
        //OnEnableSquareSelection();
        GameEvents.EnableSquareSelectionMethod();
        CheckSquare();
        this.displayedImage.sprite = this.selLetter.image;
    }
    private void OnMouseEnter()
    {
        CheckSquare();
    }
    private void OnMouseUp()
    {
        GameEvents.ClearSelectionMethod();
        GameEvents.DisableSquareSelectionMethod();
    }
    private void CheckSquare()
    {
        if(!this.selected && this.clicked)
        {
            if(!SoundManager.Instance.IsSoundFxMuted())
                this.source.Play();
            this.selected = true;
            GameEvents.CheckSquareMethod(this.normLetter.letter,gameObject.transform.position, this.index);
        }
    }
    //Check if the current index is included in this list
    private void OnCorrectWord(string word, List<int> prefabIndices)
    {
        if(this.selected && prefabIndices.Contains(this.index))
        {
            this.corrected = true;
            this.displayedImage.sprite = this.correctLetter.image;
        }
        this.selected = false;
        this.clicked = false;
    }
}
