using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// UI screen displayed when the game is completed.
/// Shows final score and turn count.
/// </summary>
public class CompleteScreenUI : ScreenBase
{
    [Header("Completion Stats")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnText;

    /// <summary>
    /// Displays the game complete screen with final score and turn info.
    /// </summary>
    /// <param name="score">Final score achieved.</param>
    /// <param name="turn">Number of turns used.</param>
    internal void ShowGameCompleteScreen(int score, int turn)
    {
        scoreText.text = score.ToString();
        turnText.text = turn.ToString();
        Activate();
    }

    /// <summary>
    /// Called when the Home button is clicked.
    /// Transitions back to the home screen.
    /// </summary>
    public void OnClick_HomeButton()
    {
        Deactivate();
        ScreenFactory.Instance.GetScreen<HomeScreenUI>()?.Activate();
    }
}
