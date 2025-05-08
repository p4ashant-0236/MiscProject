using UnityEngine;

public class ComboSystemManager : MonoBehaviour
{
    public static ComboSystemManager Instance;
    private int _comboCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.AddListener(EventID.Event_OnMatch, OnMatch);
        EventManager.AddListener(EventID.Event_OnMismatch, OnMismatch);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventID.Event_OnMatch, OnMatch);
        EventManager.RemoveListener(EventID.Event_OnMismatch, OnMismatch);
    }

    private void OnMatch(object arg)
    {
        _comboCount++;

        if (_comboCount >= 2)
        {
            Debug.Log($"Combo X{_comboCount}!");
        }
    }

    private void OnMismatch(object arg)
    {
        _comboCount = 0;
    }

    internal int GetCurrentCombo()
    {
        return _comboCount;
    }

}
