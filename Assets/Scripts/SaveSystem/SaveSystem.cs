using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SavedCardData
{
    public CardData cardData;
    public bool isActive;
}

[Serializable]
public class GameSaveResult
{
    public int row;
    public int column;
    public int score;
    public int turns;
    public int selectTwoPowerUp;
    public int revealAllPowerUp;
    public List<SavedCardData> cardBoard = new List<SavedCardData>();
}

[Serializable]
public class GameSaveData
{
    public bool canResume;
    public GameSaveResult data;
}

public class SaveSystem : MonoBehaviour
{
    private const string SaveKey = "GameSaveData";
    private static SaveSystem Get;
    private static GameSaveData cachedData;
    private GameSaveData fgcachedData;

    // --------------- Public Save Methods ----------------

    private void Awake()
    {
        Get = this;
    }

    public static void SaveScoreAndTurn(int score, int turns)
    {
        EnsureCacheLoaded();
        cachedData.data.score = score;
        cachedData.data.turns = turns;
        Get.fgcachedData = cachedData;
        Flush();
    }

    public static void SaveSelectTwoPowerUps(int count)
    {
        EnsureCacheLoaded();
        cachedData.data.selectTwoPowerUp = count;
        Get.fgcachedData = cachedData;
        Flush();
    }

    public static void SaveRevealAllPowerUps(int count)
    {
        EnsureCacheLoaded();
        cachedData.data.revealAllPowerUp = count;
        Get.fgcachedData = cachedData;
        Flush();
    }

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
        Flush();
    }

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
        Flush();
    }

    public static void MarkValidData()
    {
        EnsureCacheLoaded();
        cachedData.canResume = true;
        Get.fgcachedData = cachedData;
        //  Debug.Log(GameManager.Instance);
        //  Debug.Log(GameManager.Instance.gameSaveData1);
        //  GameManager.Instance.gameSaveData1 = cachedData;

        Flush();
    }

    public static GameSaveData LoadSavedData()
    {
        EnsureCacheLoaded();
        Get.fgcachedData = cachedData;
        return cachedData;
    }

    public static bool HasSaveData()
    {
        EnsureCacheLoaded();
        Get.fgcachedData = cachedData;
        return cachedData.canResume;
    }

    public static void ClearSave()
    {
        cachedData = new GameSaveData { canResume = false, data = new GameSaveResult() };
        PlayerPrefs.DeleteKey(SaveKey);
        Debug.Log("Save data cleared.");
    }

    public static void Flush()
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

    // ----------------- Private Helpers ------------------

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
}
