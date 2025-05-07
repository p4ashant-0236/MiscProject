using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreenUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnText;

    [SerializeField] private Button showTwoCardButton;
    [SerializeField] private TMP_Text showTwoCardCount;
    [SerializeField] private Button revealedAllCardButton;
    [SerializeField] private TMP_Text revealedAllCardCount;

    private void OnEnable()
    {
        EventManager.AddListener(EventID.Event_UpdateScore, OnScoreUpdate);
        EventManager.AddListener(EventID.Event_UpdateTurn, OnTurnUpdate);
        EventManager.AddListener(EventID.Event_PowerUp_TwoCard, OnPowerUp_TwoCard);
        EventManager.AddListener(EventID.Event_PowerUp_RevealAll, OnPowerUp_RevealAll);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventID.Event_UpdateScore, OnScoreUpdate);
        EventManager.RemoveListener(EventID.Event_UpdateTurn, OnTurnUpdate);
        EventManager.RemoveListener(EventID.Event_PowerUp_TwoCard, OnPowerUp_TwoCard);
        EventManager.RemoveListener(EventID.Event_PowerUp_RevealAll, OnPowerUp_RevealAll);
    }

    private void OnPowerUp_TwoCard(object arg)
    {
        int count = (int)arg;
        if (count < 0)
        {
            showTwoCardButton.interactable = false;
        }
        showTwoCardCount.text = count.ToString();
    }

    private void OnPowerUp_RevealAll(object arg)
    {
        int count = (int)arg;
        if (count < 0)
        {
            revealedAllCardButton.interactable = false;
        }
        revealedAllCardCount.text = count.ToString();
    }

    private void OnScoreUpdate(object arg)
    {
        scoreText.text = ((int)arg).ToString();
    }

    private void OnTurnUpdate(object arg)
    {
        turnText.text = ((int)arg).ToString();
    }

    public void OnClick_TwoCardShow()
    {
        PowerUpManager.Instance.Activate(PowerUpType.ShowTwoCard);
    }

    public void OnClick_RevealAllCard()
    {
        PowerUpManager.Instance.Activate(PowerUpType.RevealAll);
    }

}