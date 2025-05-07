using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RevealAllCardsPowerUp : BasePowerUp
{
    public RevealAllCardsPowerUp() : base(PowerUpType.RevealAll, 2) { }

    protected override void Execute(CardManager cardManager)
    {
        cardManager.StartCoroutine(RevealAllTemporary(cardManager));
    }

    private IEnumerator RevealAllTemporary(CardManager cardManager)
    {
        var cards = cardManager.GetAllCards();

        foreach (var card in cards)
            card.FlipCard(CardFlipType.Front);

        yield return new WaitForSeconds(1f);

        foreach (var card in cards)
            card.FlipCard(CardFlipType.Back);

        cardManager.ResetSelection();
    }
}
