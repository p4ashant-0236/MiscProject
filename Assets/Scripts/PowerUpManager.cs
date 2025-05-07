using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    ShowTwoCard,
    RevealAll
}

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [SerializeField] private CardManager cardManager;

    private Dictionary<PowerUpType, IPowerUp> activePowerUps = new Dictionary<PowerUpType, IPowerUp>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AddPowerUp(new ShowTwoPairsPowerUp());
        AddPowerUp(new RevealAllCardsPowerUp());
    }

    public void AddPowerUp(IPowerUp powerUp)
    {
        if (!activePowerUps.ContainsKey(powerUp.powerUpType))
        {
            activePowerUps.Add(powerUp.powerUpType, powerUp);
        }
    }

    public void Activate(PowerUpType powerUpType)
    {
        if (activePowerUps.TryGetValue(powerUpType, out var powerUp))
        {
            if (powerUp.CanUse())
            {
                powerUp.Activate(cardManager);
                Debug.Log($"Power-up {powerUpType} used. Remaining: {powerUp.UsesRemaining}");
            }
            else
            {
                Debug.Log($"Power-up {powerUpType} is out of uses.");
            }
        }
    }

    public int GetRemainingUses(PowerUpType powerUpType)
    {
        return activePowerUps.TryGetValue(powerUpType, out var powerUp) ? powerUp.UsesRemaining : 0;
    }
}
