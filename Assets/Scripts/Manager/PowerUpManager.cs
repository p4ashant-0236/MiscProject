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

    internal void PrepareDefaultState()
    {
        activePowerUps.Clear();
        AddPowerUp(new SelectTwoPowerUp(1));
        AddPowerUp(new RevealAllCardsPowerUp(2));
    }

    internal void PrepareLoadState(int selectTwoPowerUp, int revealAllPowerUp)
    {
        activePowerUps.Clear();
        AddPowerUp(new SelectTwoPowerUp(selectTwoPowerUp));
        AddPowerUp(new RevealAllCardsPowerUp(revealAllPowerUp));
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
