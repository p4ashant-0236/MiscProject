using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SelectTwoPowerUp : BasePowerUp
{
    public SelectTwoPowerUp() : base(PowerUpType.ShowTwoCard, 1) {
        EventManager.TriggerEvent(EventID.Event_PowerUp_TwoCard, UsesRemaining);
    }

    protected override void Execute(CardManager cardManager)
    {
        EventManager.TriggerEvent(EventID.Event_PowerUp_TwoCard, UsesRemaining);
        var cards = cardManager.GetAllCards();

        for (int i = 0; i < cards.Count; i++)
        {
            if (!cards[i].isActive)
                continue;

            for (int j = i + 1; j < cards.Count; j++)
            {
                if (!cards[j].isActive)
                    continue;

                if (cards[i].cardData.Id == cards[j].cardData.Id)
                {
                    cards[i].MarkDeactive();
                    cards[j].MarkDeactive();

                    cardManager.ResetSelection();
                    AudioManager.Instance?.PlaySound(AudioType.Match);
                    return;
                }
            }
        }

        Debug.LogWarning("No matching pair of active cards found.");
    }
}
