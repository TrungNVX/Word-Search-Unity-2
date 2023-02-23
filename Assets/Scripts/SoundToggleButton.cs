using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    public enum ButtonType
    {
        BackgroundMusic,
        SoundFx
    };
    public ButtonType buttonType;

    public Sprite onSprite;
    public Sprite offSprite;

    public GameObject button;
    public Vector3 offBtnPos;
    private Vector3 onBtnPos;
    private Image image;
    void Start()
    {
        this.image = GetComponent<Image>();
        this.image.sprite = onSprite;
        this.onBtnPos = this.button.GetComponent<RectTransform>().anchoredPosition;
        this.ToggleBtn();
    }
    public void ToggleBtn()
    {
        var muted = false;
        if(buttonType == ButtonType.BackgroundMusic)       
            muted = SoundManager.Instance.IsBgMusicMuted();
        else
            muted = SoundManager.Instance.IsSoundFxMuted();
        if(muted)
        {
            this.image.sprite = this.offSprite;
            button.GetComponent<RectTransform>().anchoredPosition = this.offBtnPos;
        }
        else
        {
            this.image.sprite = this.onSprite;
            button.GetComponent<RectTransform>().anchoredPosition = this.onBtnPos;
        }
    }
}
