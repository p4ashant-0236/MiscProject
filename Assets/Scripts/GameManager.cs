using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private CardManager cardManager;

    private void Start()
    {
        cardManager.InitializeCardBoard();
    }
}