using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealAllCardsPowerUp : BasePowerUp
{
    // Duration for which all cards remain revealed (in seconds)
    private const float RevealDuration = 1f;

    /// <summary>
    /// Constructor: Initializes the Reveal All power-up, triggers event and saves remaining uses.
    /// </summary>
    /// <param name="uses">Initial number of times this power-up can be used.</param>
    public RevealAllCardsPowerUp(int uses) : base(PowerUpType.RevealAll, uses)
    {
        EventManager.TriggerEvent(EventID.Event_PowerUp_RevealAll, UsesRemaining);
        SaveSystem.SaveRevealAllPowerUps(UsesRemaining);
    }

    /// <summary>
    /// Executes the power-up effect: briefly reveals all cards then hides them again.
    /// </summary>
    /// <param name="cardManager">Reference to the game's card manager.</param>
    protected override void Execute(CardManager cardManager)
    {
        EventManager.TriggerEvent(EventID.Event_PowerUp_RevealAll, UsesRemaining);
        SaveSystem.SaveRevealAllPowerUps(UsesRemaining);
        cardManager.StartCoroutine(RevealAllTemporary(cardManager));
    }

    /// <summary>
    /// Coroutine that flips all cards face up temporarily, then flips them back after a delay.
    /// </summary>
    /// <param name="cardManager">Reference to the card manager.</param>
    /// <returns>IEnumerator for coroutine handling.</returns>
    private IEnumerator RevealAllTemporary(CardManager cardManager)
    {
        var cards = cardManager.GetAllCards();

        // Flip all cards to front (reveal)
        foreach (var card in cards)
            card.FlipCard(CardFlipType.Front);

        // Wait for the reveal duration
        yield return new WaitForSeconds(RevealDuration);

        // Flip all cards back to hide them
        foreach (var card in cards)
            card.FlipCard(CardFlipType.Back);

        // Clear any current selection state
        cardManager.ResetSelection();
    }
}
