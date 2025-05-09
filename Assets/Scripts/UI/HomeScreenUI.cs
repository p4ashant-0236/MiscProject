using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the Home screen UI behavior and transitions.
/// </summary>
public class HomeScreenUI : ScreenBase
{
    [Header("Main Buttons")]
    [SerializeField] private Button startNewGameButton;
    [SerializeField] private Button loadGameButton;

    [Header("Game Size Selection Buttons")]
    [SerializeField] private Button button_1;
    [SerializeField] private Button button_2;
    [SerializeField] private Button button_3;

    // Constants for grid sizes using Vector2Int
    [SerializeField] private Vector2Int GRID_1 = new Vector2Int(3, 4);
    [SerializeField] private Vector2Int GRID_2 = new Vector2Int(4, 5);
    [SerializeField] private Vector2Int GRID_3 = new Vector2Int(5, 6);

    private void OnEnable()
    {
        // Reset UI state to initial screen
        startNewGameButton.gameObject.SetActive(true);
        loadGameButton.gameObject.SetActive(true);

        button_1.gameObject.SetActive(false);
        button_2.gameObject.SetActive(false);
        button_3.gameObject.SetActive(false);

        // Enable or disable load button based on save availability
        var savedata = SaveSystem.LoadSavedData();
        Debug.Log(savedata.canResume);
        loadGameButton.interactable = savedata.canResume;
    }

    /// <summary>
    /// Called when the Start New Game button is clicked.
    /// Shows board size options.
    /// </summary>
    public void OnClick_StartNewGame()
    {
        startNewGameButton.gameObject.SetActive(false);
        loadGameButton.gameObject.SetActive(false);

        button_1.gameObject.SetActive(true);
        button_2.gameObject.SetActive(true);
        button_3.gameObject.SetActive(true);
    }

    /// <summary>
    /// Loads the saved game and transitions to gameplay.
    /// </summary>
    public void OnClick_LoadGame()
    {
        ScreenFactory.Instance.GetScreen<GameplayScreenUI>()?.Activate();
        GameManager.Instance.LoadOldGame();
        Deactivate();
    }

    public void OnClick_Grid1() => StartGameWithGrid(GRID_1);
    public void OnClick_Grid2() => StartGameWithGrid(GRID_2);
    public void OnClick_Grid3() => StartGameWithGrid(GRID_3);

    /// <summary>
    /// Helper method to start a new game and switch screens.
    /// </summary>
    private void StartGameWithGrid(Vector2Int gridSize)
    {
        ScreenFactory.Instance.GetScreen<GameplayScreenUI>()?.Activate();
        GameManager.Instance.StartNewGame(gridSize.x, gridSize.y);
        Deactivate();
    }
}
