# 🦍 GorillaStopWatch Plugins

A plugin framework extension for [arielthemonke's GorillaStats Stopwatch Mod](https://github.com/arielthemonke/GorillaStopWatch)

> 🎯 This version allows you to add custom plugins to the stopwatch mod!

![Stopwatch Showcase](Assets/Stopwatchshowcase.png)

---

## ⚠️ Important Notice

This mod **will disable the original GorillaStats mod** to avoid conflicts.

---

## 📦 What Is This?

The original mod turns the GorillaStats watch into a functioning stopwatch.

This fork adds a **plugin system** so developers can extend stopwatch behavior by dropping custom DLLs into a plugin folder — no need to modify the core mod!

---

## 🧩 Plugin System Overview

Plugins are external `.dll` files that implement a simple interface and can hook into stopwatch events like start, stop, and awake.

### Interface: `IStopWatchPlugin`

```csharp
public interface IStopWatchPlugin
{
    void OnStart();
    void OnStop();
    void OnAwake();
}
```
Plugins must implement this interface and will be automatically loaded at runtime.

## 🛠️ How to Create a Plugin

### 1. Reference the Plugin API

First, download or reference the `StopWatchPluginAPI.dll`, which contains the `IStopWatchPlugin` interface.

### 2. Create a New Class Library Project

In Visual Studio or your preferred IDE:
- Create a new **Class Library** (.NET Framework 9.0 is recommended).
- Reference `StopWatchPluginAPI.dll`.

### 3. Implement the Interface

```csharp
using StopWatchPluginAPI;

public class ExamplePlugin : IStopWatchPlugin
{
    public void OnStart()
    {
        Console.WriteLine("[ExamplePlugin] Stopwatch started!");
    }

    public void OnStop()
    {
        Console.WriteLine("[ExamplePlugin] Stopwatch stopped.");
    }

    public void OnAwake()
    {
        Console.WriteLine("[ExamplePlugin] I have been awoken.");
    }
}
```

### 4. Build the Plugin

Build the project to generate `YourPlugin.dll`. You’ll usually find it in:
```
YourProjectFolder/bin/Debug/YourPlugin.dll
```

### 5. Install the Plugin

Place your plugin `.dll` file into the following folder:
```
BepInEx/plugins/StopWatchPlugins/
```

When you launch the game, the stopwatch mod will automatically detect and load all valid plugins from this folder.
Let me know if you'd like a ready-to-go template project!


