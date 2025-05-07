using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Card Data")]
public class CardDataSO : ScriptableObject
{
    public Sprite cardBackSideSprite;
    public List<CardData> cardDatas;
}

[System.Serializable]
public class CardData
{
    public int Id;
    public Sprite cardSprite;
}