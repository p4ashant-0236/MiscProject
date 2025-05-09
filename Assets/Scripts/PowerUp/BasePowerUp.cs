public abstract class BasePowerUp
{
    // Power-up type (e.g., ShowTwoCard, RevealAll)
    public PowerUpType powerUpType { get; protected set; }

    // Number of remaining uses for this power-up
    public int UsesRemaining { get; protected set; }

    // Constructor to initialize power-up with type and initial uses
    protected BasePowerUp(PowerUpType powerUpType, int initialUses)
    {
        this.powerUpType = powerUpType;
        UsesRemaining = initialUses;
    }

    // Checks if the power-up can be used (remaining uses > 0)
    public bool CanUse() => UsesRemaining > 0;

    // Activates the power-up and executes its effect if usable
    public void Activate(CardManager cardManager)
    {
        if (!CanUse()) return;

        UsesRemaining--;
        Execute(cardManager);
    }

    // Abstract method for executing the power-up effect
    protected abstract void Execute(CardManager cardManager);
}
