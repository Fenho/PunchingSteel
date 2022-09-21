using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StaticVars : MonoBehaviour
{

    public static int score;
    public static bool win;

    public static void addPoints(int points)
    {
        score = Math.Max(0, score + points);
    }

    public static void winGame()
    {
        win = true;
    }

    public static void loseGame()
    {
        win = false;
    }
}
