using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController
{
    private static int baseScore = 10;
    private static int currentScore = 0;
    private static int currentTurn = 0;

    public static int CurrentScore => currentScore;

    public static void AddTurn()
    {
        currentTurn += 1;
        EventManager.TriggerEvent(EventID.Event_UpdateTurn, currentTurn);
    }

    public static void AddMatchScore()
    {
        int multiplier = ComboSystemManager.Instance.GetCurrentCombo();
        Debug.Log(multiplier);
        int scoreToAdd = baseScore * multiplier;
        currentScore += scoreToAdd;

        EventManager.TriggerEvent(EventID.Event_UpdateScore, currentScore);
    }

    public static void ResetScore()
    {
        currentScore = 0;
        currentTurn = 0;
        EventManager.TriggerEvent(EventID.Event_UpdateScore, currentScore);
        EventManager.TriggerEvent(EventID.Event_UpdateTurn, currentTurn);
    }
}