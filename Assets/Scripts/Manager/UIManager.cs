using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private HomeScreenUI homeScreenUI;
    [SerializeField] private CompleteScreenUI completeScreenUI;
    [SerializeField] private GameplayScreenUI gameplayScreenUI;

    private GameManager gameManager;

    internal void OnGameComplete(int score, int turn)
    {
        gameplayScreenUI.gameObject.SetActive(false);
        homeScreenUI.gameObject.SetActive(false);
        completeScreenUI.ShowGameCompleteScreen(score, turn);
        SaveSystem.ClearSave();
    }

    internal void PrepareDefaultState(GameManager gameManager)
    {
        this.gameManager = gameManager;

        homeScreenUI.PrepareDefaultState(this);
        completeScreenUI.PrepareDefaultState(this);
        gameplayScreenUI.PrepareDefaultState(this);

    }

    internal void StartNewGame(int row, int column)
    {
        gameplayScreenUI.gameObject.SetActive(true);
        gameManager.StartNewGame(row, column);
    }

    internal void LoadOldGame() { 
        gameplayScreenUI.gameObject.SetActive(true);
        gameManager.LoadOldGame();
    }

    internal void ShowHomeScreen()
    {
        gameplayScreenUI.gameObject.SetActive(false);
        homeScreenUI.gameObject.SetActive(true);
        completeScreenUI.gameObject.SetActive(false);
    }
}
