using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StaticVars : MonoBehaviour
{

    public static int score;
    public static bool win;
    public static int pointsLeftJabTeam = 100;
    public static int pointsRightJabTeam = 100;
    public static int pointsLeftJabMissTeam = -30;
    public static int pointsRightJabMissTeam = -30;
    public static int pointsLeftJabBlockTeam = -30;
    public static int pointsRightJabBlockTeam = -30;
    public static int pointsLeftJabEnemy = -60;
    public static int pointsRightJabEnemy = -60;
    public static int pointsLeftDodgeTeam = 30;
    public static int pointsRightDodgeTeam = 30;
    public static int pointsUncoordinateTeam = -50;
    public static int missEnemy = 10;
    public static int blockEnemy = 5;
    public static int pointsBlockTeam;

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
