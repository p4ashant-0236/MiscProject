using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private PowerUpManager powerUpManager;
    [SerializeField] private DynamicGridResizer dynamicGridResizer;

    private void Start()
    {
        uiManager.PrepareDefaultState(this);
        AudioManager.Instance?.PlaySound(AudioType.BackgroundMusic);
    }

    private void OnEnable()
    {
        EventManager.AddListener(EventID.Event_GameWin, OnGameComplete);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventID.Event_GameWin, OnGameComplete);
    }


    internal void StartNewGame(int row, int column)
    {
        dynamicGridResizer.Initialize(Mathf.Min(row, column), Mathf.Min(row, column), Mathf.Max(row, column));
        dynamicGridResizer.UpdateBoard();
        cardManager.InitializeCardBoard(row, column);
        powerUpManager.PrepareDefaultState();
    }

    internal void OnGameComplete(object args)
    {
        uiManager.OnGameComplete(ScoreController.CurrentScore, ScoreController.CurrentTurn);
    }
}