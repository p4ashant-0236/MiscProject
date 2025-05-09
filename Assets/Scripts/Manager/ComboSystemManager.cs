using UnityEngine;

public class ComboSystemManager : MonoBehaviour
{
    public static ComboSystemManager Instance;

    private int _comboCount = 0;                    // Tracks consecutive match streaks
    private const int MIN_COMBO_TO_DISPLAY = 2;     // Minimum combo count required to show combo message

    private void Awake()
    {
        // Assign this instance to the static reference for global access
        Instance = this;
    }

    private void OnEnable()
    {
        // Subscribe to match and mismatch events
        EventManager.AddListener(EventID.Event_OnMatch, OnMatch);
        EventManager.AddListener(EventID.Event_OnMismatch, OnMismatch);
    }

    private void OnDisable()
    {
        // Unsubscribe from events to avoid memory leaks
        EventManager.RemoveListener(EventID.Event_OnMatch, OnMatch);
        EventManager.RemoveListener(EventID.Event_OnMismatch, OnMismatch);
    }

    /// <summary>
    /// Called when a card match occurs.
    /// Increments combo and logs it if threshold is met.
    /// </summary>
    private void OnMatch(object arg)
    {
        _comboCount++;

        if (_comboCount >= MIN_COMBO_TO_DISPLAY)
        {
            EventManager.TriggerEvent(EventID.Event_Combo, _comboCount);
            Debug.Log($"Combo X{_comboCount}!");
        }
    }

    /// <summary>
    /// Called when a mismatch occurs. Resets the combo count.
    /// </summary>
    private void OnMismatch(object arg)
    {
        _comboCount = 0;
    }

    /// <summary>
    /// Returns the current combo streak count.
    /// </summary>
    internal int GetCurrentCombo()
    {
        return _comboCount;
    }
}
