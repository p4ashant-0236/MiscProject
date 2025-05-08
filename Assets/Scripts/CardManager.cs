using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardDataSO cardDataSO;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardBoardParent;

    private List<CardData> cardBoard = new List<CardData>();
    private List<Card> cardBoardItems = new List<Card>();

    private void OnEnable()
    {
        EventManager.AddListener(EventID.Event_CardSelected, OnCardSelected);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventID.Event_CardSelected, OnCardSelected);
    }

    internal void InitializeCardBoard(int row, int column)
    {
        cardBoard.Clear();

        int totalCardsNeeded = row * column;

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

        cardBoardItems.Clear();

        for (int i = 0; i < cardBoard.Count; i++)
        {
            Card card = Instantiate(cardPrefab.gameObject, cardBoardParent).GetComponent<Card>();
            cardBoardItems.Add(card);
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

    private Card firstSelectedCard;
    private Card secondSelectedCard;

    private void OnCardSelected(object arg)
    {
        Card data = (Card)arg;
        if (firstSelectedCard == null)
        {
            firstSelectedCard = data;
            return;
        }

        secondSelectedCard = data;
        CheckForMatch();
    }

    private void CheckForMatch()
    {
        if (firstSelectedCard.cardData.Id == secondSelectedCard.cardData.Id)
        {
            //Match
            Debug.Log("Match");
            firstSelectedCard.MarkDeactive();
            secondSelectedCard.MarkDeactive();

            AudioManager.Instance?.PlaySound(AudioType.Match);
            EventManager.TriggerEvent(EventID.Event_OnMatch);
            ScoreController.AddMatchScore();

            CheckForGameWin();
        }
        else
        {
            firstSelectedCard.FlipCard(CardFlipType.Back, 0.3f);
            secondSelectedCard.FlipCard(CardFlipType.Back, 0.3f);

            AudioManager.Instance?.PlaySound(AudioType.Mismatch);
            EventManager.TriggerEvent(EventID.Event_OnMismatch);
        }

        ScoreController.AddTurn();
        firstSelectedCard = null;
        secondSelectedCard = null;
    }

    private void CheckForGameWin()
    {
        var item = cardBoardItems.Find(x => x.isActive == true);
        if (item == null)
        {
            Debug.Log("Game Completes");
            EventManager.TriggerEvent(EventID.Event_GameWin);
            AudioManager.Instance?.PlaySound(AudioType.GameComplete);
        }
    }

    internal void ResetSelection()
    {
        firstSelectedCard = null;
        secondSelectedCard = null;
    }

    internal List<Card> GetAllCards()
    {
        return cardBoardItems;
    }
}