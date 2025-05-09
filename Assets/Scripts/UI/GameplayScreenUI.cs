using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the gameplay UI, including score, turn count, and power-up buttons.
/// </summary>
public class GameplayScreenUI : ScreenBase
{
    [Header("Score and Turn")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnText;

    [Header("Power-Up: Show Two Cards")]
    [SerializeField] private Button showTwoCardButton;
    [SerializeField] private TMP_Text showTwoCardCount;

    [Header("Power-Up: Reveal All Cards")]
    [SerializeField] private Button revealedAllCardButton;
    [SerializeField] private TMP_Text revealedAllCardCount;

    // Thresholds/constants
    private const int MINIMUM_POWERUP_COUNT = 0;

    private void OnEnable()
    {
        // Subscribe to game events
        EventManager.AddListener(EventID.Event_UpdateScore, OnScoreUpdate);
        EventManager.AddListener(EventID.Event_UpdateTurn, OnTurnUpdate);
        EventManager.AddListener(EventID.Event_PowerUp_TwoCard, OnPowerUp_TwoCard);
        EventManager.AddListener(EventID.Event_PowerUp_RevealAll, OnPowerUp_RevealAll);
    }

    private void OnDisable()
    {
        // Unsubscribe from game events
        EventManager.RemoveListener(EventID.Event_UpdateScore, OnScoreUpdate);
        EventManager.RemoveListener(EventID.Event_UpdateTurn, OnTurnUpdate);
        EventManager.RemoveListener(EventID.Event_PowerUp_TwoCard, OnPowerUp_TwoCard);
        EventManager.RemoveListener(EventID.Event_PowerUp_RevealAll, OnPowerUp_RevealAll);
    }

    /// <summary>
    /// Updates UI for the "Show Two Cards" power-up count and button interactability.
    /// </summary>
    private void OnPowerUp_TwoCard(object arg)
    {
        int count = (int)arg;
        showTwoCardButton.interactable = count > MINIMUM_POWERUP_COUNT;
        showTwoCardCount.text = count.ToString();
    }

    /// <summary>
    /// Updates UI for the "Reveal All Cards" power-up count and button interactability.
    /// </summary>
    private void OnPowerUp_RevealAll(object arg)
    {
        int count = (int)arg;
        revealedAllCardButton.interactable = count > MINIMUM_POWERUP_COUNT;
        revealedAllCardCount.text = count.ToString();
    }

    /// <summary>
    /// Updates the score text.
    /// </summary>
    private void OnScoreUpdate(object arg)
    {
        scoreText.text = ((int)arg).ToString();
    }

    /// <summary>
    /// Updates the turn text.
    /// </summary>
    private void OnTurnUpdate(object arg)
    {
        turnText.text = ((int)arg).ToString();
    }

    /// <summary>
    /// Called when the Show Two Cards power-up is clicked.
    /// </summary>
    public void OnClick_TwoCardShow()
    {
        PowerUpManager.Instance.Activate(PowerUpType.ShowTwoCard);
    }

    /// <summary>
    /// Called when the Reveal All Cards power-up is clicked.
    /// </summary>
    public void OnClick_RevealAllCard()
    {
        PowerUpManager.Instance.Activate(PowerUpType.RevealAll);
    }
}
