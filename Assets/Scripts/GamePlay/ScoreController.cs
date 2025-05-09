using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController
{
    // Base score for each successful match (removed magic number 10)
    private const int DefaultBaseScore = 10;

    private static int baseScore = DefaultBaseScore;
    private static int currentScore = 0;
    private static int currentTurn = 0;

    // Expose current score as read-only
    public static int CurrentScore => currentScore;

    // Expose current turn count as read-only
    public static int CurrentTurn => currentTurn;

    /// <summary>
    /// Increments the turn counter and notifies listeners via event.
    /// </summary>
    public static void AddTurn()
    {
        currentTurn += 1;
        EventManager.TriggerEvent(EventID.Event_UpdateTurn, currentTurn);
    }

    /// <summary>
    /// Adds score based on current combo multiplier and base score,
    /// then triggers a score update event.
    /// </summary>
    public static void AddMatchScore()
    {
        int multiplier = ComboSystemManager.Instance.GetCurrentCombo(); // Get current combo count
        Debug.Log(multiplier); // Debug combo value

        int scoreToAdd = baseScore * multiplier;
        currentScore += scoreToAdd;

        EventManager.TriggerEvent(EventID.Event_UpdateScore, currentScore);
    }

    /// <summary>
    /// Resets score and turn count to zero and sends update events.
    /// </summary>
    public static void ResetScore()
    {
        currentScore = 0;
        currentTurn = 0;

        EventManager.TriggerEvent(EventID.Event_UpdateScore, currentScore);
        EventManager.TriggerEvent(EventID.Event_UpdateTurn, currentTurn);
    }

    /// <summary>
    /// Loads previously saved score and turn values and updates UI.
    /// </summary>
    /// <param name="data">Saved game result containing score and turn info.</param>
    public static void LoadOldData(GameSaveResult data)
    {
        currentScore = data.score;
        currentTurn = data.turns;

        EventManager.TriggerEvent(EventID.Event_UpdateScore, currentScore);
        EventManager.TriggerEvent(EventID.Event_UpdateTurn, currentTurn);
    }
}
