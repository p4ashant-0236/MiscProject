using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the gameplay UI, including score, turn count, and power-up buttons.
/// </summary>
public class GameplayScreenUI : ScreenBase
{
    [Header("Score and Turn")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnText;

    [Header("Power-Up: Show Two Cards")]
    [SerializeField] private Button showTwoCardButton;
    [SerializeField] private TMP_Text showTwoCardCount;

    [Header("Power-Up: Reveal All Cards")]
    [SerializeField] private Button revealedAllCardButton;
    [SerializeField] private TMP_Text revealedAllCardCount;

    // Thresholds/constants
    private const int MINIMUM_POWERUP_COUNT = 0;

    [SerializeField] TMP_Text comboAnimationText;
    [SerializeField] TMP_Text scoreAnimationText;

    private UnityPool comboAnimationPool;
    private UnityPool scoreAnimationPool;

    private void Start()
    {
        comboAnimationPool = new UnityPool(comboAnimationText.gameObject, 2, comboAnimationText.transform.parent, true);
        scoreAnimationPool = new UnityPool(scoreAnimationText.gameObject, 2, scoreAnimationText.transform.parent, true);
    }

    private void OnEnable()
    {
        // Subscribe to game events
        EventManager.AddListener(EventID.Event_UpdateScore, OnScoreUpdate);
        EventManager.AddListener(EventID.Event_UpdateTurn, OnTurnUpdate);
        EventManager.AddListener(EventID.Event_Combo, OnCombo);
        EventManager.AddListener(EventID.Event_PowerUp_TwoCard, OnPowerUp_TwoCard);
        EventManager.AddListener(EventID.Event_PowerUp_RevealAll, OnPowerUp_RevealAll);
    }

    private void OnDisable()
    {
        // Unsubscribe from game events
        EventManager.RemoveListener(EventID.Event_UpdateScore, OnScoreUpdate);
        EventManager.RemoveListener(EventID.Event_UpdateTurn, OnTurnUpdate);
        EventManager.RemoveListener(EventID.Event_Combo, OnCombo);
        EventManager.RemoveListener(EventID.Event_PowerUp_TwoCard, OnPowerUp_TwoCard);
        EventManager.RemoveListener(EventID.Event_PowerUp_RevealAll, OnPowerUp_RevealAll);
    }

    /// <summary>
    /// Updates UI for the "Show Two Cards" power-up count and button interactability.
    /// </summary>
    private void OnPowerUp_TwoCard(object arg)
    {
        int count = (int)arg;
        showTwoCardButton.interactable = count > MINIMUM_POWERUP_COUNT;
        showTwoCardCount.text = count.ToString();
    }

    /// <summary>
    /// Updates UI for the "Reveal All Cards" power-up count and button interactability.
    /// </summary>
    private void OnPowerUp_RevealAll(object arg)
    {
        int count = (int)arg;
        revealedAllCardButton.interactable = count > MINIMUM_POWERUP_COUNT;
        revealedAllCardCount.text = count.ToString();
    }

    /// <summary>
    /// Updates the score text.
    /// </summary>
    private void OnScoreUpdate(object arg)
    {
        ScoreData data = (ScoreData)arg;
        scoreText.text = ((int)data.score).ToString();

        if (data.scoreGained > 0)
        {
            var text = scoreAnimationPool.Get<TMP_Text>(scoreAnimationText.transform.parent);
            text.text = "+" + ((int)data.scoreGained).ToString();
            PlayTextEffect(text,scoreAnimationText.rectTransform, scoreAnimationPool);
        }
    }

    /// <summary>
    /// Updates the turn text.
    /// </summary>
    private void OnTurnUpdate(object arg)
    {
        turnText.text = ((int)arg).ToString();
    }

    /// <summary>
    /// Called On Combo
    /// </summary>
    private void OnCombo(object arg)
    {
        var text = comboAnimationPool.Get<TMP_Text>(scoreAnimationText.transform.parent);
        text.text = "Combo X " + ((int)arg).ToString();
        PlayTextEffect(text, comboAnimationText.rectTransform, comboAnimationPool);
    }

    /// <summary>
    /// Called when the Show Two Cards power-up is clicked.
    /// </summary>
    public void OnClick_TwoCardShow()
    {
        PowerUpManager.Instance.Activate(PowerUpType.ShowTwoCard);
    }

    /// <summary>
    /// Called when the Reveal All Cards power-up is clicked.
    /// </summary>
    public void OnClick_RevealAllCard()
    {
        PowerUpManager.Instance.Activate(PowerUpType.RevealAll);
    }

    private void PlayTextEffect(TMP_Text text, RectTransform referenceText, UnityPool pool)
    {
        text.transform.localScale = Vector3.zero;
        text.transform.position = referenceText.position;

        LeanTween.scale(text.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack).setOnComplete(()=> {

            LeanTween.moveY(text.rectTransform, text.rectTransform.anchoredPosition.y + 100, 1f).setEaseOutCubic();

            Color color = text.color;
            // Fade Out
            LeanTween.value(gameObject, 1f, 0f, 1f)
                .setOnUpdate((float val) =>
                {
                    color = text.color;
                    color.a = val;
                    text.color = color;
                }).setOnComplete(()=> {
                    scoreAnimationPool.Add(text.gameObject);
                });
        });
    }
}
