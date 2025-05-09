using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SavedCardData
{
    public CardData cardData; // The data related to the card (e.g., ID, type, etc.)
    public bool isActive;     // Whether the card is currently active in the game
}

[Serializable]
public class GameSaveResult
{
    public int row;                          // Number of rows in the game board
    public int column;                       // Number of columns in the game board
    public int score;                        // Player's current score
    public int turns;                        // Number of turns taken so far
    public int selectTwoPowerUp;             // Count of "Select Two" power-ups
    public int revealAllPowerUp;             // Count of "Reveal All" power-ups
    public List<SavedCardData> cardBoard = new List<SavedCardData>(); // Current card board state
}

[Serializable]
public class GameSaveData
{
    public bool canResume;                   // Whether the game can be resumed
    public GameSaveResult data;              // Actual saved game result data
}

public class SaveSystem : MonoBehaviour
{
    private const string SaveKey = "GameSaveData";   // Key for saving data in PlayerPrefs
    private static SaveSystem Get;                  // Singleton-like reference for this component
    private static GameSaveData cachedData;         // Static cache for quick access
    private GameSaveData fgcachedData;              // Instance-level mirror of cache for debug/inspection

    // Called when the script instance is being loaded
    private void Awake()
    {
        Get = this;
    }

    // Saves only score and turns to cache and PlayerPrefs
    public static void SaveScoreAndTurn(int score, int turns)
    {
        EnsureCacheLoaded();
        cachedData.data.score = score;
        cachedData.data.turns = turns;
        Get.fgcachedData = cachedData;
        Save();
    }

    // Saves count of "Select Two" power-ups
    public static void SaveSelectTwoPowerUps(int count)
    {
        EnsureCacheLoaded();
        cachedData.data.selectTwoPowerUp = count;
        Get.fgcachedData = cachedData;
        Save();
    }

    // Saves count of "Reveal All" power-ups
    public static void SaveRevealAllPowerUps(int count)
    {
        EnsureCacheLoaded();
        cachedData.data.revealAllPowerUp = count;
        Get.fgcachedData = cachedData;
        Save();
    }

    // Saves the current card board layout and state
    public static void SaveCardBoard(int row, int column, List<Card> cardBoardItems)
    {
        EnsureCacheLoaded();
        cachedData.data.row = row;
        cachedData.data.column = column;
        cachedData.data.cardBoard.Clear();

        foreach (var card in cardBoardItems)
        {
            if (card?.cardData == null) continue;

            cachedData.data.cardBoard.Add(new SavedCardData
            {
                cardData = card.cardData,
                isActive = card.isActive
            });
        }
        Get.fgcachedData = cachedData;
        Save();
    }

    // Marks a specific card (by ID) as inactive in the save data
    public static void MarkCardDeactive(int cardId)
    {
        EnsureCacheLoaded();

        bool foundAny = false;
        foreach (var card in cachedData.data.cardBoard)
        {
            if (card.cardData != null && card.cardData.Id == cardId)
            {
                card.isActive = false;
                foundAny = true;
            }
        }

        if (!foundAny)
        {
            Debug.LogWarning($"Card ID {cardId} not found in saved data.");
        }

        Get.fgcachedData = cachedData;
        Save();
    }

    // Marks the current save as valid for resuming
    public static void MarkValidData()
    {
        EnsureCacheLoaded();
        cachedData.canResume = true;
        Get.fgcachedData = cachedData;
        Save();
    }

    // Loads saved data from PlayerPrefs (if available)
    public static GameSaveData LoadSavedData()
    {
        EnsureCacheLoaded();
        Get.fgcachedData = cachedData;
        return cachedData;
    }

    // Returns whether valid save data is available to resume
    public static bool HasSaveData()
    {
        EnsureCacheLoaded();
        Get.fgcachedData = cachedData;
        return cachedData.canResume;
    }

    // Clears the save data from PlayerPrefs and cache
    public static void ClearSave()
    {
        cachedData = new GameSaveData { canResume = false, data = new GameSaveResult() };
        PlayerPrefs.DeleteKey(SaveKey);
        Debug.Log("Save data cleared.");
    }

    // Serializes and stores the current cachedData to PlayerPrefs
    public static void Save()
    {
        if (cachedData == null)
        {
            Debug.LogWarning("Flush skipped: no data cached.");
            return;
        }

        string json = JsonUtility.ToJson(cachedData);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Game data flushed to PlayerPrefs.");
    }

    // Ensures the cache is loaded from PlayerPrefs or initializes a new one
    private static void EnsureCacheLoaded()
    {
        if (cachedData != null) return;

        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            cachedData = JsonUtility.FromJson<GameSaveData>(json);
            if (cachedData.data == null)
                cachedData.data = new GameSaveResult();
        }
        else
        {
            cachedData = new GameSaveData
            {
                canResume = false,
                data = new GameSaveResult()
            };
        }

        Get.fgcachedData = cachedData;
        Debug.Log("Game data cache loaded.");
    }

    // Automatically saves when the app is paused (e.g., home button or incoming call)
    private void OnApplicationPause(bool pause)
    {
        Save();
    }
}
