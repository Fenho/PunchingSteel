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

    public static void addPointsByType(string type)
    {
        int assignedScore = 0;
        switch (type){
            case "LeftJabTeam":
                assignedScore = pointsLeftJabTeam;
                break;
            case "RightJabTeam":
                assignedScore = pointsRightJabTeam;
                break;
            case "LeftJabMissTeam":
                assignedScore = pointsLeftJabMissTeam;
                break;
            case "RightJabMissTeam":
                assignedScore = pointsRightJabMissTeam;
                break;
            case "LeftJabBlockTeam":
                assignedScore = pointsLeftJabBlockTeam;
                break;
            case "RightJabBlockTeam":
                assignedScore = pointsRightJabBlockTeam;
                break;
            case "LeftJabEnemy":
                assignedScore = pointsLeftJabEnemy;
                break;
            case "RightJabEnemy":
                assignedScore = pointsRightJabEnemy;
                break;
            case "LeftDodgeTeam":
                assignedScore = pointsLeftDodgeTeam;
                break;
            case "RightDodgeTeam":
                assignedScore = pointsRightDodgeTeam;
                break;
            case "UncoordinatedTeam":
                assignedScore = pointsLeftJabTeam;
                break;
            case "missEnemy":
                assignedScore = missEnemy;
                break;
            case "blockEnemy":
                assignedScore = blockEnemy;
                break;
            default:
                assignedScore = 0;
                break;
        }
        score = Math.Max(0, score + assignedScore);
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
