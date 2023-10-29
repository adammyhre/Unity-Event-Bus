# Unity C# Event Bus
 
 ![EventBus](https://github.com/adammyhre/Unity-Event-Bus/assets/38876398/1b053da8-4a22-4bef-a052-6bf7f3e24b7d)

## Description
This EventBus system provides a way to create decoupled architectures in Unity projects. It allows communication between different parts of an application without requiring direct references.

## Code Structure
This EventBus system contains several C# classes residing in the Scripts\EventBus directory:

1. `EventBus.cs` - Main EventBus class that provides static functions for registering, deregistering, and triggering custom events.

2. `EventBinding.cs` - IEventBinding interface and class definition for EventBinding, which is used to bind functions to events.

3. `Events.cs` - IEvent interface and sample code, which shows how to define custom events.

4. `PredefinedAssemblyUtil.cs` - Utility class for locating assemblies and finding types within them. See [Unity Documentation](https://docs.unity3d.com/Manual/ScriptCompileOrderFolders.html).

5. `EventBusUtil.cs` - Static initialization methods and additional utilities used for EventBus.


## Example Usage

The usage generally works like:

```csharp 
public struct PlayerEvent : IEvent {
    public int health;
    public int mana;
}

EventBinding<PlayerEvent> playerEventBinding;

void OnEnable() {    
    playerEventBinding = new EventBinding<PlayerEvent>(HandlePlayerEvent);
    EventBus<PlayerEvent>.Register(playerEventBinding);

    // Can Add or Remove Actions to/from the EventBinding
}

void OnDisable() {
    EventBus<PlayerEvent>.Deregister(playerEventBinding);
}

void Start() {
    EventBus<PlayerEvent>.Raise(new PlayerEvent {
        health = healthComponent.GetHealth(),
        mana = manaComponent.GetMana()
    });    
}

void HandlePlayerEvent(PlayerEvent playerEvent) {
    Debug.Log($"Player event received! Health: {playerEvent.health}, Mana: {playerEvent.mana}");
}
```

## YouTube

[**Watch the tutorial video here**](https://youtu.be/4_DTAnigmaQ)

You can also check out my [YouTube channel](https://www.youtube.com/@git-amend?sub_confirmation=1) for more Unity content.

## Installation and Setup
Since this is a Unity-centric project, you will need to have Unity installed on your system. The EventBus codebase is entirely C# and conforms to the .NETFramework v4.7.1 standards.

To use these scripts in your project, please place these scripts in your Scripts or a related directory in the Unity editor.

## Contributions
Contributions are always welcome. You can contribute by improving the EventBus codebase, enhancing its features, or providing suggestions.

## Inspired by

This project takes inspiration from the following open source projects:
- [BasicEventBus](https://github.com/pointcache/BasicEventBus?)
- [UnityEventAggregator](https://github.com/EricFreeman/UnityEventAggregator)
- [EventBus](https://github.com/SaldayOpen/EventBus)
- [GenericEventBus](https://github.com/PeturDarri/GenericEventBus/tree/main)
