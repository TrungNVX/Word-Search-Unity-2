using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static string ReverseWord(string word)
    {
        char[] letter = word.ToCharArray();
        Array.Reverse(letter);
        return new string(letter);
    }
}
