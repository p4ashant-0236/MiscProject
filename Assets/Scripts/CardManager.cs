using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardDataSO cardDataSO;
    [SerializeField] private Button cardPrefab;
    [SerializeField] private Transform cardBoardParent;

    private List<CardData> cardBoard;

    internal void InitializeCardBoard()
    {
        cardBoard = new List<CardData>();

        int totalCardsNeeded = 12;

        int cardCounter = 0;
        // Add pairs of cards ensuring different suits
        while (cardBoard.Count < totalCardsNeeded - 1)
        {
            cardBoard.Add(cardDataSO.cardDatas[cardCounter]);
            cardBoard.Add(cardDataSO.cardDatas[cardCounter]);

            cardCounter += 1;
        }

        ShuffleCardBoard();
        SpawnCardsOnBoard();
    }

    internal void ShuffleCardBoard()
    {
        System.Random rng = new System.Random();
        for (int i = cardBoard.Count - 1; i > 0; i--)
        {
            int randomIndex = rng.Next(cardBoard.Count);
            CardData temp = cardBoard[i];
            cardBoard[i] = cardBoard[randomIndex];
            cardBoard[randomIndex] = temp;
        }
    }

    private void SpawnCardsOnBoard()
    {
        foreach (Transform child in cardBoardParent)
        {
            Destroy(child.gameObject); // Clean up old cards if any
        }

        for (int i = 0; i < cardBoard.Count; i++)
        {
            Button cardButton = Instantiate(cardPrefab, cardBoardParent);
            Card card = cardButton.GetComponent<Card>();

            if (card != null)
            {
                card.Initialize(cardBoard[i], cardDataSO.cardBackSideSprite);
            }
            else
            {
                Debug.LogError("Card component missing on prefab!");
            }
        }
    }

}