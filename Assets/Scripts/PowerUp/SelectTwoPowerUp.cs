using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTwoPowerUp : BasePowerUp
{
    // Constant to represent the starting offset for nested loop (i + 1)
    private const int StartIndexOffset = 1;

    /// <summary>
    /// Constructor: Initializes the power-up and immediately saves its uses and notifies listeners.
    /// </summary>
    /// <param name="uses">Number of uses available for the power-up.</param>
    public SelectTwoPowerUp(int uses) : base(PowerUpType.ShowTwoCard, uses)
    {
        // Notify systems and save state
        EventManager.TriggerEvent(EventID.Event_PowerUp_TwoCard, UsesRemaining);
        SaveSystem.SaveSelectTwoPowerUps(UsesRemaining);
    }

    /// <summary>
    /// Executes the power-up: finds the first active matching card pair and marks them deactive.
    /// </summary>
    /// <param name="cardManager">Reference to the current card manager.</param>
    protected override void Execute(CardManager cardManager)
    {
        // Notify systems and persist state
        EventManager.TriggerEvent(EventID.Event_PowerUp_TwoCard, UsesRemaining);
        SaveSystem.SaveSelectTwoPowerUps(UsesRemaining);

        var cards = cardManager.GetAllCards();

        // Loop through all active cards and find the first matching pair
        for (int i = 0; i < cards.Count; i++)
        {
            if (!cards[i].isActive)
                continue;

            for (int j = i + StartIndexOffset; j < cards.Count; j++)
            {
                if (!cards[j].isActive)
                    continue;

                // Found matching pair
                if (cards[i].cardData.Id == cards[j].cardData.Id)
                {
                    cards[i].MarkDeactive();
                    cards[j].MarkDeactive();

                    // Reset any current card selection
                    cardManager.ResetSelection();

                    // Play match audio
                    AudioManager.Instance?.PlaySound(AudioType.Match);
                    return;
                }
            }
        }

        Debug.LogWarning("No matching pair of active cards found.");
    }
}
