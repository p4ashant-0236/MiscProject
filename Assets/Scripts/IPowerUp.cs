public interface IPowerUp
{
    PowerUpType powerUpType { get; }
    int UsesRemaining { get; }
    void Activate(CardManager cardManager);
    bool CanUse();
}
