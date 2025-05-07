using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private CardData _cardData;
    private Sprite _cardBackSideSprite;
    [SerializeField] private Image cardImage;

    internal void Initialize(CardData cardData, Sprite cardBackSideSprite)
    {
        _cardData = cardData;
        _cardBackSideSprite = cardBackSideSprite;
        cardImage.sprite = _cardData.cardSprite;
    }

}