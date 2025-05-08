using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompleteScreenUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnText;

    private UIManager uIManager;
    internal void PrepareDefaultState(UIManager uIManager)
    {
        this.uIManager = uIManager;
    }

    internal void ShowGameCompleteScreen(int score, int turn)
    {
        scoreText.text = score.ToString();
        turnText.text = turn.ToString();
        this.gameObject.SetActive(true);
    }

    public void OnClick_HomeButton()
    {
        uIManager.ShowHomeScreen();
    }
}
