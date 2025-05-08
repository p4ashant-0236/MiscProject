using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private CardManager cardManager;
    [SerializeField] private DynamicGridResizer dynamicGridResizer;
    [SerializeField] private Vector2Int boardSize;

    private void Start()
    {
        dynamicGridResizer.Initialize(Mathf.Min(boardSize.x, boardSize.y), Mathf.Min(boardSize.x, boardSize.y), Mathf.Max(boardSize.x, boardSize.y));
        dynamicGridResizer.UpdateBoard();
        cardManager.InitializeCardBoard(boardSize);
    }
}