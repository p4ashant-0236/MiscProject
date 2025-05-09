using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardDataSO cardDataSO;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardBoardParent;

    private List<CardData> cardBoard = new List<CardData>();
    private List<Card> cardBoardItems = new List<Card>();
    private int activeCardCount; // Tracks active cards to improve game win check

    private static readonly System.Random rng = new System.Random(); // Cached Random instance

    private const float CardFlipBackDelay = 0.3f;
    private const float MismatchSoundDelay = 0.2f;

    private UnityPool cardPool;

    private void OnEnable()
    {
        // Subscribe to card selection event
        EventManager.AddListener(EventID.Event_CardSelected, OnCardSelected);
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        EventManager.RemoveListener(EventID.Event_CardSelected, OnCardSelected);
    }

    /// <summary>
    /// Load the card board using previously saved game data.
    /// </summary>
    internal void LoadCardBoard(GameSaveResult gameSaveResult)
    {
        cardBoard.Clear();
        cardBoardItems.Clear();

        if (cardPool == null)
        {
            cardPool = new UnityPool(cardPrefab.gameObject, 10, cardBoardParent, true);
        }

        // Remove any existing card objects in the hierarchy
        foreach (Transform child in cardBoardParent)
        {
            cardPool.Add(child.gameObject);
        }

        // Instantiate cards from saved data
        foreach (var savedCard in gameSaveResult.cardBoard)
        {
            cardBoard.Add(savedCard.cardData);

            Card card = cardPool.Get<Card>(cardBoardParent);
            cardBoardItems.Add(card);

            if (card != null)
            {
                card.Initialize(savedCard.cardData, cardDataSO.cardBackSideSprite);

                if (!savedCard.isActive)
                {
                    card.MarkDeactive();
                     // Update active card count
                }else
                    activeCardCount++;
            }
            else
            {
                Debug.LogError("Card component missing on prefab!");
            }
        }

        Debug.Log("Board loaded from saved data.");
    }

    /// <summary>
    /// Create a new shuffled board based on row/column dimensions.
    /// </summary>
    internal void InitializeCardBoard(int row, int column)
    {
        cardBoard.Clear();

        int totalCardsNeeded = row * column;
        int cardCounter = 0;

        // Create card pairs
        while (cardBoard.Count < totalCardsNeeded - 1)
        {
            cardBoard.Add(cardDataSO.cardDatas[cardCounter]);
            cardBoard.Add(cardDataSO.cardDatas[cardCounter]);
            cardCounter++;
        }

        ShuffleCardBoard();       // Shuffle before spawning
        SpawnCardsOnBoard();      // Instantiate card objects

        SaveSystem.ClearSave();   // Clear old save
        SaveSystem.MarkValidData();
        SaveSystem.SaveCardBoard(row, column, cardBoardItems);
    }

    /// <summary>
    /// Shuffle the internal card board list.
    /// </summary>
    internal void ShuffleCardBoard()
    {
        // Use the cached Random instance to shuffle
        for (int i = cardBoard.Count - 1; i > 0; i--)
        {
            int randomIndex = rng.Next(cardBoard.Count);
            CardData temp = cardBoard[i];
            cardBoard[i] = cardBoard[randomIndex];
            cardBoard[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Spawn shuffled card data into instantiated card objects on the board.
    /// </summary>
    private void SpawnCardsOnBoard()
    {
        if (cardPool == null)
        {
            cardPool = new UnityPool(cardPrefab.gameObject, 10, cardBoardParent, true);
        }

        // Remove any existing card objects in the hierarchy
        foreach (Transform child in cardBoardParent)
        {
            cardPool.Add(child.gameObject);
        }

        cardBoardItems.Clear();
        activeCardCount = 0; // Reset active card count

        for (int i = 0; i < cardBoard.Count; i++)
        {
            Card card = cardPool.Get<Card>(cardBoardParent);
            cardBoardItems.Add(card);

            if (card != null)
            {
                card.Initialize(cardBoard[i], cardDataSO.cardBackSideSprite);
                activeCardCount++;
            }
            else
            {
                Debug.LogError("Card component missing on prefab!");
            }
        }
    }

    private Card firstSelectedCard;
    private Card secondSelectedCard;

    /// <summary>
    /// Handle card selection logic and trigger match check.
    /// </summary>
    private void OnCardSelected(object arg)
    {
        Card selectedCard = (Card)arg;

        if (firstSelectedCard == null)
        {
            firstSelectedCard = selectedCard;
            return;
        }

        secondSelectedCard = selectedCard;
        CheckForMatch();
    }

    /// <summary>
    /// Check if selected cards match and apply logic accordingly.
    /// </summary>
    private void CheckForMatch()
    {
        if (firstSelectedCard.cardData.Id == secondSelectedCard.cardData.Id)
        {
            // Match Found
            Debug.Log("Match");
            firstSelectedCard.MarkDeactive();
            secondSelectedCard.MarkDeactive();

            activeCardCount -= 2;

            AudioManager.Instance?.PlaySound(AudioType.Match);
            EventManager.TriggerEvent(EventID.Event_OnMatch);
            ScoreController.AddMatchScore();

            CheckForGameWin();

            SaveSystem.MarkCardDeactive(firstSelectedCard.cardData.Id);
            SaveSystem.MarkCardDeactive(secondSelectedCard.cardData.Id);

        }
        else
        {
            // Mismatch: Flip cards back after short delay
            firstSelectedCard.FlipCard(CardFlipType.Back, CardFlipBackDelay);
            secondSelectedCard.FlipCard(CardFlipType.Back, CardFlipBackDelay);

            LeanTween.delayedCall(MismatchSoundDelay, () =>
            {
                AudioManager.Instance?.PlaySound(AudioType.Mismatch);
            });

            EventManager.TriggerEvent(EventID.Event_OnMismatch);
        }

        // Save current turn/score and reset selections
        ScoreController.AddTurn();
        SaveSystem.SaveScoreAndTurn(ScoreController.CurrentScore, ScoreController.CurrentTurn);

        firstSelectedCard = null;
        secondSelectedCard = null;
    }

    /// <summary>
    /// Check if all cards are matched (i.e., game win).
    /// </summary>
    private void CheckForGameWin()
    {
        if (activeCardCount == 0)
        {
            Debug.Log("Game Completes");
            EventManager.TriggerEvent(EventID.Event_GameWin);
            AudioManager.Instance?.PlaySound(AudioType.GameComplete);
        }
    }

    /// <summary>
    /// Manually reset selected card references.
    /// </summary>
    internal void ResetSelection()
    {
        firstSelectedCard = null;
        secondSelectedCard = null;
    }

    /// <summary>
    /// Get all card GameObjects currently on the board.
    /// </summary>
    internal List<Card> GetAllCards()
    {
        return cardBoardItems;
    }
}
