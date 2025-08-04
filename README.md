# Getting Started with Mage Arena Modding

This guide will walk you through creating your first mod for Mage Arena using BepInEx.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Setting Up Your Development Environment](#setting-up-your-development-environment)
- [Creating Your First Plugin](#creating-your-first-plugin)
- [Understanding the Plugin Structure](#understanding-the-plugin-structure)
- [Building and Testing](#building-and-testing)
- [Next Steps](#next-steps)

## Prerequisites

Before you begin, make sure you have:

- **Visual Studio 2019/2022** or **Visual Studio Code** with C# extensions
- **Mage Arena** installed
- **BepInEx** installed in your Mage Arena or Mod Manager directory
- Basic knowledge of C# programming

## Setting Up Your Development Environment

### 1. Create a New Project

1. Open Visual Studio
2. Create a new **Class Library (.NET Framework)** project
3. **Important**: Set the target framework to **.NET Framework 4.8**
   - Right-click your project → Properties
   - Change "Target Framework" to **.NET Framework 4.8**

### 2. Add Required References

You need to add references to several DLL files. Navigate to your Mage Arena installation directory and add the following references:

#### BepInEx References
Located in `MageArena/BepInEx/core/`:
- `BepInEx.dll`
- `0Harmony.dll`

#### Unity and Game References  
Located in `MageArena/MageArena_Data/Managed/`:
- `Assembly-CSharp.dll`
- `UnityEngine.dll`
- `UnityEngine.CoreModule.dll`

**To add references:**
1. Right-click **References** in Solution Explorer
2. Select **Add Reference**
3. Click **Browse** and navigate to the directories above
4. Select the required DLL files

### 3. Project Structure

Your project should look like this:
```
MyFirstPlugin/
├── Properties/
│   └── AssemblyInfo.cs
├── References/
│   ├── BepInEx.dll
│   ├── 0Harmony.dll
│   ├── Assembly-CSharp.dll
│   └── (other Unity DLLs)
├── Plugin.cs
└── PluginInfo.cs
```

## Creating Your First Plugin

### 1. Create PluginInfo.cs

First, create a file to store your plugin metadata:

```csharp
namespace MyFirstPlugin
{
    internal static class PluginInfo
    {
        public const string PLUGIN_GUID = "com.yourname.myfirstplugin";
        public const string PLUGIN_NAME = "My First Plugin";
        public const string PLUGIN_VERSION = "1.0.0";
    }
}
```

### 2. Create Plugin.cs

Now create your main plugin class:

```csharp
using BepInEx;

namespace MyFirstPlugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
```

## Understanding the Plugin Structure

A BepInEx plugin contains three main parts:

### 1. Class that Inherits BaseUnityPlugin
```csharp
public class Plugin : BaseUnityPlugin
```
This base class provides access to Unity's lifecycle methods and BepInEx functionality.

### 2. BepInPlugin Attribute and Metadata
```csharp
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
```
This attribute tells BepInEx about your plugin:
- **GUID**: Unique identifier (use reverse domain notation)
- **Name**: Human-readable name for your plugin
- **Version**: Version number (use semantic versioning)

### 3. Plugin Startup Code
```csharp
private void Awake()
{
    // Your initialization code here
}
```
The `Awake()` method is called when your plugin loads. This is where you put your initialization logic.

### Common Unity Lifecycle Methods

You can override these methods in your plugin:

```csharp
private void Awake()
{
    // Called when the plugin is first loaded
}

private void Start()
{
    // Called after all plugins have awakened
}

private void Update()
{
    // Called every frame (use sparingly!)
}

private void OnDestroy()
{
    // Called when the plugin is being unloaded
}
```

## Building and Testing

### 1. Build Your Plugin

1. Build your project (Build > Build Solution)
2. Find the compiled DLL in `bin/Debug/` or `bin/Release/`
3. Copy `YourPluginName.dll` to `MageArena/BepInEx/plugins/` or relevant Mod Manager location.

### 2. Test Your Plugin

1. Launch Mage Arena
2. Check the BepInEx console window for your log message
3. Look for: `"Plugin com.yourname.myfirstplugin is loaded!"`

### 3. Common Build Issues

**"Reference not found" errors:**
- Verify all DLL references are correctly added. You often need to add more references to .dlls within the game directory.
- Ensure you're using .NET Framework 4.8

## Next Steps

Now that you have a basic plugin working, you can:

### Learn Advanced Modding
- **[Using ModSync](ModSync.md)** - Make your mods multiplayer-compatible
- **[BlackMagicAPI Guide](BlackMagicAPI.md)** - Create custom spells and items
- **[Harmony Patching](https://docs.bepinex.dev/articles/dev_guide/runtime_patching.html)** - Modify existing game code

### Expand Your Plugin

```csharp
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace MyFirstPlugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        // Configuration
        private ConfigEntry<bool> enableFeature;
        
        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            
            // Create config options
            enableFeature = Config.Bind("General", "EnableFeature", true, "Enable the main feature");
            
            // Set up key bindings
            if (enableFeature.Value)
            {
                Logger.LogInfo("Feature enabled!");
            }
        }
        
        private void Update()
        {
            // Example: Check for key press
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Logger.LogInfo("F1 pressed!");
            }
        }
    }
}
```

### Best Practices

1. **Use meaningful GUIDs** - Include your name/modname. A good mod is named: `com.author.name`
2. **Log important events** - Use `Logger.LogInfo()`, `Logger.LogWarning()`, etc.
3. **Handle errors gracefully** - Use try-catch blocks for risky operations
4. **Follow naming conventions** - Use PascalCase for public members
5. **Comment your code** - Make it maintainable for future you

### Resources

- **[BepInEx Documentation](https://docs.bepinex.dev/)** - Official BepInEx guides
- **[Unity Scripting Reference](https://docs.unity3d.com/ScriptReference/)** - Unity API documentation
- **[Harmony Documentation](https://harmony.pardeike.net/)** - Code patching library
- **[Spell Creation Guide](BlackMagicAPI.md#creating-spells)** - Using BlackMagicAPI to create custom spells
- **[Item Creation Guide](BlackMagicAPI.md#creating-items)** - Using BlackMagicAPI to create custom items
- **[Utilizing ModSync](ModSync.md)** - Using ModSync to ensure multiplayer compatability
- **[Mage Arena Modding Discord](https://discord.gg/GHdT7kHEBT)** - Community support

Happy modding! 🔮✨
