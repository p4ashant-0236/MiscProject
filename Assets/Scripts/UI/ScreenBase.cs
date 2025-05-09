using UnityEngine;

/// <summary>
/// Abstract base class for all UI screens in the game.
/// Provides default Activate/Deactivate behavior.
/// </summary>
public abstract class ScreenBase : MonoBehaviour
{
    /// <summary>
    /// Activates the screen (sets it active in the hierarchy).
    /// Override to add custom logic on activation.
    /// </summary>
    public virtual void Activate()
    {
        gameObject.SetActive(true);
        Debug.Log($"{GetType().Name} Activated");
    }

    /// <summary>
    /// Deactivates the screen (sets it inactive in the hierarchy).
    /// Override to add custom logic on deactivation.
    /// </summary>
    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        Debug.Log($"{GetType().Name} Deactivated");
    }
}
