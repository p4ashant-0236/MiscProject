using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Types of power-ups available in the game.
/// </summary>
public enum PowerUpType
{
    ShowTwoCard,
    RevealAll
}

/// <summary>
/// Manages all power-ups, their initialization, usage, and remaining count.
/// </summary>
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [SerializeField] private CardManager cardManager;

    // Stores currently available power-ups and their instances.
    private Dictionary<PowerUpType, BasePowerUp> activePowerUps = new Dictionary<PowerUpType, BasePowerUp>();

    private void Awake()
    {
        // Assign this instance for global access
        Instance = this;
    }

    /// <summary>
    /// Initializes power-ups to their default starting state.
    /// </summary>
    internal void PrepareDefaultState()
    {
        activePowerUps.Clear();
        AddPowerUp(new SelectTwoPowerUp(1));      // Starts with 1 use for SelectTwo
        AddPowerUp(new RevealAllCardsPowerUp(2)); // Starts with 2 uses for RevealAll
    }

    /// <summary>
    /// Initializes power-ups based on saved state (e.g., from disk).
    /// </summary>
    internal void PrepareLoadState(int selectTwoPowerUp, int revealAllPowerUp)
    {
        activePowerUps.Clear();
        AddPowerUp(new SelectTwoPowerUp(selectTwoPowerUp));
        AddPowerUp(new RevealAllCardsPowerUp(revealAllPowerUp));
    }

    /// <summary>
    /// Adds a new power-up to the active list if not already present.
    /// </summary>
    public void AddPowerUp(BasePowerUp powerUp)
    {
        if (!activePowerUps.ContainsKey(powerUp.powerUpType))
        {
            activePowerUps.Add(powerUp.powerUpType, powerUp);
        }
    }

    /// <summary>
    /// Triggers the activation of the given power-up if available and usable.
    /// </summary>
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

    /// <summary>
    /// Returns how many times the given power-up can still be used.
    /// </summary>
    public int GetRemainingUses(PowerUpType powerUpType)
    {
        return activePowerUps.TryGetValue(powerUpType, out var powerUp) ? powerUp.UsesRemaining : 0;
    }
}
