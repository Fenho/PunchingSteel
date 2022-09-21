using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticVars : MonoBehaviour
{

    public static int score;
    public static bool win;

    public static void addPoints(int points)
    {
        score += points;
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
