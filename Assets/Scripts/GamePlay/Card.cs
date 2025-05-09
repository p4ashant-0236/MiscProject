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

    // Constants for animation timings and scaling
    private const float FlipDuration = 0.2f;
    private const float BounceScale = 1.2f;
    private const float BounceDuration = 0.3f;
    private static readonly Vector3 OriginalScale = Vector3.one;
    private static readonly Quaternion OriginalRotation = Quaternion.identity;

    private void OnEnable()
    {
        EventManager.AddListener(EventID.Reset_AllCard, OnResetCard);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventID.Reset_AllCard, OnResetCard);
    }

    /// <summary>
    /// Resets the card to its back side if active.
    /// </summary>
    private void OnResetCard(object arg)
    {
        if (isActive)
            FlipCard(CardFlipType.Back);
    }

    /// <summary>
    /// Initializes the card with data and back side sprite.
    /// </summary>
    internal void Initialize(CardData cardData, Sprite cardBackSideSprite)
    {
        this.cardData = cardData;
        _cardBackSideSprite = cardBackSideSprite;
        cardImage.sprite = cardBackSideSprite;
        _isFront = false;
        isActive = true;
    }

    /// <summary>
    /// Deactivates the card and plays bounce animation after flipping to front.
    /// </summary>
    internal void MarkDeactive()
    {
        FlipCard(CardFlipType.Front, () => {
            BounceCard();
        });
        isActive = false;
    }

    /// <summary>
    /// Handles card click event. Flips if active and not already front.
    /// </summary>
    public void OnClick_Card()
    {
        if (isActive && !_isFront)
            FlipCard(CardFlipType.Front, () => {
                EventManager.TriggerEvent(EventID.Event_CardSelected, this);
            });
    }

    /// <summary>
    /// Flips the card with delay and optional callback.
    /// </summary>
    internal void FlipCard(CardFlipType cardFlipType, float delay, Action onCompleteCallback = default)
    {
        if (isActive)
            StartCoroutine(FlipCardRoutine(cardFlipType, onCompleteCallback, delay));
        else
            onCompleteCallback?.Invoke();

        AudioManager.Instance?.PlaySound(AudioType.Flip);
    }

    /// <summary>
    /// Flips the card immediately with optional callback.
    /// </summary>
    internal void FlipCard(CardFlipType cardFlipType, Action onCompleteCallback = default)
    {
        if (isActive)
            StartCoroutine(FlipCardRoutine(cardFlipType, onCompleteCallback));
        else
            onCompleteCallback?.Invoke();

        AudioManager.Instance?.PlaySound(AudioType.Flip);
    }

    /// <summary>
    /// Plays bounce animation on card.
    /// </summary>
    internal void BounceCard()
    {
        LeanTween.scale(cardImage.gameObject, OriginalScale * BounceScale, BounceDuration)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() => {
                    LeanTween.scale(cardImage.gameObject, OriginalScale, BounceDuration)
                             .setEase(LeanTweenType.easeOutBack);
                });
    }

    /// <summary>
    /// Coroutine that handles full flip animation and sprite swap.
    /// </summary>
    private IEnumerator FlipCardRoutine(CardFlipType cardFlipType, Action onCompleteCallback, float delay = 0f, bool waitForAnimation = true)
    {
        yield return new WaitForSeconds(delay);

        if (cardFlipType == CardFlipType.Front && !_isFront)
        {
            yield return FlipCardCore(() => {
                cardImage.sprite = cardData.cardSprite;
                _isFront = true;
            }, onCompleteCallback, waitForAnimation);
        }
        else if (cardFlipType == CardFlipType.Back && _isFront)
        {
            yield return FlipCardCore(() => {
                cardImage.sprite = _cardBackSideSprite;
                _isFront = false;
            }, onCompleteCallback, waitForAnimation);
        }
        else
        {
            onCompleteCallback?.Invoke();
        }
    }

    /// <summary>
    /// Core animation logic to flip the card visually using LeanTween.
    /// </summary>
    private IEnumerator FlipCardCore(Action onSwapCallback, Action onCompleteCallback = default, bool waitForAnimation = true)
    {
        LeanTween.cancel(cardImage.gameObject);

        // Step 1: Scale Up Slightly
        LeanTween.scale(cardImage.gameObject, OriginalScale * BounceScale, FlipDuration / 3)
                 .setEase(LeanTweenType.easeOutQuad);

        // Step 2: Rotate card to 90 degrees
        LeanTween.value(cardImage.gameObject, 0, 90, FlipDuration / 2)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnUpdate((float val) => {
                     cardImage.transform.localRotation = OriginalRotation * Quaternion.Euler(0, val, 0);
                 })
                 .setOnComplete(() => {
                     // Step 3: Swap sprite and rotate back to 0
                     onSwapCallback?.Invoke();

                     LeanTween.value(cardImage.gameObject, 90, 0, FlipDuration / 2)
                              .setEase(LeanTweenType.easeOutQuad)
                              .setOnUpdate((float val) => {
                                  cardImage.transform.localRotation = OriginalRotation * Quaternion.Euler(0, val, 0);
                              });

                     LeanTween.scale(cardImage.gameObject, OriginalScale, FlipDuration / 3)
                              .setEase(LeanTweenType.easeOutBack);
                 });

        yield return new WaitForSeconds(waitForAnimation ? FlipDuration : 0.1f);
        onCompleteCallback?.Invoke();
    }
}
