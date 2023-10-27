using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Contains methods and properties related to event buses and event types in the Unity application.
/// </summary>
public static class EventBusUtil {
    public static IReadOnlyList<Type> EventTypes { get; set; }
    public static IReadOnlyList<Type> EventBusTypes { get; set; }
    
#if UNITY_EDITOR
    public static PlayModeStateChange PlayModeState { get; set; }
    
    /// <summary>
    /// Initializes the Unity Editor related components of the EventBusUtil.
    /// The [InitializeOnLoadMethod] attribute causes this method to be called every time a script
    /// is loaded or when the game enters Play Mode in the Editor. This is useful to initialize
    /// fields or states of the class that are necessary during the editing state that also apply
    /// when the game enters Play Mode.
    /// The method sets up a subscriber to the playModeStateChanged event to allow
    /// actions to be performed when the Editor's play mode changes.
    /// </summary>    
    [InitializeOnLoadMethod]
    public static void InitializeEditor() {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    
    static void OnPlayModeStateChanged(PlayModeStateChange state) {
        PlayModeState = state;
        if (state == PlayModeStateChange.ExitingPlayMode) {
            ClearAllBuses();
        }
    }
#endif

    /// <summary>
    /// Initializes the EventBusUtil class at runtime before the loading of any scene.
    /// The [RuntimeInitializeOnLoadMethod] attribute instructs Unity to execute this method after
    /// the game has been loaded but before any scene has been loaded, in both Play Mode and after
    /// a Build is run. This guarantees that necessary initialization of bus-related types and events is
    /// done before any game objects, scripts or components have started.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize() {
        EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
        EventBusTypes = InitializeAllBuses();
    }

    static List<Type> InitializeAllBuses() {
        List<Type> eventBusTypes = new List<Type>();
        
        var typedef = typeof(EventBus<>);
        foreach (var eventType in EventTypes) {
            var busType = typedef.MakeGenericType(eventType);
            eventBusTypes.Add(busType);
            Debug.Log($"Initialized EventBus<{eventType.Name}>");
        }
        
        return eventBusTypes;
    }

    /// <summary>
    /// Clears (removes all listeners from) all event buses in the application.
    /// </summary>
    public static void ClearAllBuses() {
        Debug.Log("Clearing all buses...");
        for (int i = 0; i < EventBusTypes.Count; i++) {
            var busType = EventBusTypes[i];
            var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
            clearMethod?.Invoke(null, null);
        }
    }
}