using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu]
public class AlphabetData : ScriptableObject
{
    [System.Serializable]
    public class LetterData
    {
        public string letter;
        public Sprite image;
    }
    public List<LetterData> _normList = new List<LetterData>();
    public List<LetterData> _plainList = new List<LetterData>();
    public List<LetterData> _highList = new List<LetterData>();
    public List<LetterData> _wrongList = new List<LetterData>();
}
