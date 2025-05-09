using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CardManager cardManager;
    [SerializeField] private PowerUpManager powerUpManager;
    [SerializeField] private DynamicGridResizer dynamicGridResizer;

    private const AudioType BACKGROUND_MUSIC = AudioType.BackgroundMusic;

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance exists
        Instance = this;
    }

    private void Start()
    {
        // Play background music at game start
        AudioManager.Instance?.PlaySound(BACKGROUND_MUSIC);

        // Show home screen
        ScreenFactory.Instance.GetScreen<HomeScreenUI>()?.Activate();
    }

    private void OnEnable()
    {
        // Listen for game win event
        EventManager.AddListener(EventID.Event_GameWin, OnGameComplete);
    }

    private void OnDisable()
    {
        // Remove event listener
        EventManager.RemoveListener(EventID.Event_GameWin, OnGameComplete);
    }

    /// <summary>
    /// Initializes and starts a new game.
    /// </summary>
    /// <param name="row">Number of rows in the grid.</param>
    /// <param name="column">Number of columns in the grid.</param>
    internal void StartNewGame(int row, int column)
    {
        int min = Mathf.Min(row, column);
        int max = Mathf.Max(row, column);

        dynamicGridResizer.Initialize(min, max);
        dynamicGridResizer.UpdateBoard();

        cardManager.InitializeCardBoard(row, column);
        powerUpManager.PrepareDefaultState();
    }

    /// <summary>
    /// Loads a previously saved game state.
    /// </summary>
    internal void LoadOldGame()
    {
        GameSaveData gameSaveData = SaveSystem.LoadSavedData();
        var data = gameSaveData.data;

        dynamicGridResizer.Initialize(data.row, data.column);
        dynamicGridResizer.UpdateBoard();

        cardManager.LoadCardBoard(data);
        powerUpManager.PrepareLoadState(data.selectTwoPowerUp, data.revealAllPowerUp);
        ScoreController.LoadOldData(data);

        Debug.Log("Loading Done");
    }

    /// <summary>
    /// Handles game completion logic and screen transitions.
    /// </summary>
    internal void OnGameComplete(object args)
    {
        var completeScreen = ScreenFactory.Instance.GetScreen<CompleteScreenUI>();
        completeScreen.ShowGameCompleteScreen(ScoreController.CurrentScore, ScoreController.CurrentTurn);
        completeScreen.Activate();

        // Clear saved game state after win
        SaveSystem.ClearSave();

        // Deactivate gameplay and home screens
        ScreenFactory.Instance.GetScreen<HomeScreenUI>()?.Deactivate();
        ScreenFactory.Instance.GetScreen<GameplayScreenUI>()?.Deactivate();
    }
}
