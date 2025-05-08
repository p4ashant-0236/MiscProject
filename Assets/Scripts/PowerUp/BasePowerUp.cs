public abstract class BasePowerUp : IPowerUp
{
    public PowerUpType powerUpType { get; protected set; }
    public int UsesRemaining { get; protected set; }

    protected BasePowerUp(PowerUpType powerUpType, int initialUses)
    {
        this.powerUpType = powerUpType;
        UsesRemaining = initialUses;
    }

    public bool CanUse() => UsesRemaining > 0;

    public void Activate(CardManager cardManager)
    {
        if (!CanUse()) return;

        UsesRemaining--;
        Execute(cardManager);
    }

    protected abstract void Execute(CardManager cardManager);
}
