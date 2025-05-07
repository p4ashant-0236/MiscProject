using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShowTwoPairsPowerUp : BasePowerUp
{
    public ShowTwoPairsPowerUp() : base(PowerUpType.ShowTwoCard, 1) { }

    protected override void Execute(CardManager cardManager)
    {
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
                    cards[i].FlipCard(CardFlipType.Front);
                    cards[j].FlipCard(CardFlipType.Front);

                    cardManager.ResetSelection();
                    return;
                }
            }
        }

        Debug.LogWarning("No matching pair of active cards found.");
    }
}
