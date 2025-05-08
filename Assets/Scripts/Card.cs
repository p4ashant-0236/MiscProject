using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum CardFlipType
{
    Front,
    Back
}

public class Card : MonoBehaviour
{
    internal CardData cardData;
    private Sprite _cardBackSideSprite;
    [SerializeField] private Image cardImage;
    private bool _isFront;

    internal bool isActive;

    private void OnEnable()
    {
        EventManager.AddListener(EventID.Reset_AllCard, OnResetCard);
    }

    private void OnResetCard(object arg)
    {
        if(isActive)
            FlipCard(CardFlipType.Back);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventID.Reset_AllCard, OnResetCard);
    }

    internal void Initialize(CardData cardData, Sprite cardBackSideSprite)
    {
        this.cardData = cardData;
        _cardBackSideSprite = cardBackSideSprite;
        cardImage.sprite = cardBackSideSprite;
        _isFront = false;
        isActive = true;
    }

    internal void MarkDeactive()
    {
        FlipCard(CardFlipType.Front, ()=> {
            BounceCard();
        });
        isActive = false;
    }

    public void OnClick_Card()
    {
        if(isActive && _isFront == false)
            FlipCard(CardFlipType.Front, ()=> {
                EventManager.TriggerEvent(EventID.Event_CardSelected, this);
            });
    }

    internal void FlipCard(CardFlipType cardFlipType, float delay, Action onCompleteCallback = default)
    {
        if (isActive)
            StartCoroutine(FlipCardRoutine(cardFlipType, onCompleteCallback, delay));
        else
            onCompleteCallback?.Invoke();

        AudioManager.Instance?.PlaySound(AudioType.Flip);
    }

    internal void FlipCard(CardFlipType cardFlipType, Action onCompleteCallback = default)
    {
        if (isActive)
            StartCoroutine(FlipCardRoutine(cardFlipType, onCompleteCallback));
        else
            onCompleteCallback?.Invoke();

        AudioManager.Instance?.PlaySound(AudioType.Flip);
    }

    internal void BounceCard()
    {
        LeanTween.scale(cardImage.gameObject, Vector3.one * 1.2f, 0.3f)
                .setEase(LeanTweenType.easeOutBack).setOnComplete(()=> {
                    LeanTween.scale(cardImage.gameObject, Vector3.one, 0.3f)
                    .setEase(LeanTweenType.easeOutBack);
                });
    }

    private IEnumerator FlipCardRoutine(CardFlipType cardFlipType, Action onCompleteCallback, float delay = 0f, bool waitForAnimation = true)
    {
        yield return new WaitForSeconds(delay);

        if (cardFlipType == CardFlipType.Front && _isFront == false)
        {
            yield return FlipCardCore(() =>
            {
                cardImage.sprite = cardData.cardSprite;
                _isFront = true;
            }, onCompleteCallback, waitForAnimation);
        }
        else if (cardFlipType == CardFlipType.Back && _isFront == true)
        {
            yield return FlipCardCore(() =>
            {
                cardImage.sprite = _cardBackSideSprite;
                _isFront = false;
            }, onCompleteCallback, waitForAnimation);
        }
        else
        {
            onCompleteCallback?.Invoke();
        }
        yield break;
    }

    private IEnumerator FlipCardCore(Action onSwapCallback, Action onCompleteCallback = default, bool waitForAnimation = true)
    {
        float flipDuration = 0.2f;
        Vector3 originalScale = Vector3.one;
        Quaternion originalRotation = Quaternion.identity;

        LeanTween.cancel(cardImage.gameObject);

        // Step 1: Scale Up Slightly & Move Up
        LeanTween.scale(cardImage.gameObject, originalScale * 1.2f, flipDuration / 3)
            .setEase(LeanTweenType.easeOutQuad);

        // Step 2: Rotate Around Local Y-axis (Maintain Other Axes)
        LeanTween.value(cardImage.gameObject, 0, 90, flipDuration / 2)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnUpdate((float val) =>
            {
                cardImage.transform.localRotation = originalRotation * Quaternion.Euler(0, val, 0);
            })
            .setOnComplete(() =>
            {
                onSwapCallback?.Invoke();

                // Step 3: Rotate Back & Restore Scale
                LeanTween.value(cardImage.gameObject, 90, 0, flipDuration / 2)
                                .setEase(LeanTweenType.easeOutQuad)
                                .setOnUpdate((float val) =>
                                {
                                    cardImage.transform.localRotation = originalRotation * Quaternion.Euler(0, val, 0);
                                });

                LeanTween.scale(cardImage.gameObject, originalScale, flipDuration / 3)
                .setEase(LeanTweenType.easeOutBack);
            });

        if (waitForAnimation)
            yield return new WaitForSeconds(flipDuration);
        else
            yield return new WaitForSeconds(0.1f);

        onCompleteCallback?.Invoke();
        yield break;
    }
}