public interface IPowerUp
{
    // Power-up type (e.g., ShowTwoCard, RevealAll)
    PowerUpType powerUpType { get; }

    // Number of remaining uses for the power-up
    int UsesRemaining { get; }

    // Activates the power-up and executes its effect
    void Activate(CardManager cardManager);

    // Checks if the power-up can be used (remaining uses > 0)
    bool CanUse();
}
