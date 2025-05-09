using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central manager to handle and provide access to all UI screens in the game.
/// Screens must be registered in the Inspector.
/// </summary>
public class ScreenFactory : MonoBehaviour
{
    public static ScreenFactory Instance { get; private set; }

    [Header("Registered Screens")]
    [Tooltip("Drag all ScreenBase-derived UI screens here in the Inspector.")]
    [SerializeField] private List<ScreenBase> registeredScreens;

    // Internal screen map for fast lookup by type
    private Dictionary<Type, ScreenBase> screenMap = new Dictionary<Type, ScreenBase>();

    // Constants
    private const string DUPLICATE_SCREEN_WARNING = "Duplicate screen type registered: ";
    private const string SCREEN_NOT_FOUND_ERROR = "Screen of type {0} not found. Make sure it's registered in the Inspector.";

    private void Awake()
    {
        // Enforce singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Register each screen from the list into the dictionary
        foreach (var screen in registeredScreens)
        {
            if (screen == null) continue;

            var type = screen.GetType();

            if (!screenMap.ContainsKey(type))
            {
                screenMap[type] = screen;
            }
            else
            {
                Debug.LogWarning(DUPLICATE_SCREEN_WARNING + type);
            }
        }
    }

    /// <summary>
    /// Retrieves a registered screen of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of screen to retrieve.</typeparam>
    /// <returns>The screen instance if found; otherwise, null.</returns>
    public T GetScreen<T>() where T : ScreenBase
    {
        var type = typeof(T);

        if (screenMap.TryGetValue(type, out var screen))
        {
            return (T)screen;
        }

        Debug.LogErrorFormat(SCREEN_NOT_FOUND_ERROR, type);
        return null;
    }
}
