# Using ModSync

ModSync is a dependency management system for Mage Arena that ensures players have compatible mod configurations when joining multiplayer games. It automatically checks mod compatibility between clients and hosts to prevent crashes and gameplay issues.

## Table of Contents
- [What is ModSync?](#what-is-modsync)
- [Basic Usage](#basic-usage)
- [Sync Modes](#sync-modes)
- [Implementation Examples](#implementation-examples)
- [Best Practices](#best-practices)
- [Troubleshooting](#troubleshooting)

## What is ModSync?

ModSync works by:
- **Checking mod compatibility** between players as they join a game. If a player joins with a mod tagged "all" that the host doesnt have, it forces them to leave. It also allows lobby owners to force a lobby-wide requirement on mods.

## Basic Usage

To make your mod compatible with ModSync, you need to:

1. **Add ModSync as a dependency** in your plugin attributes. Use a [Bepinex Hard Dependency](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/2_plugin_start.html) to do this. Your mod should NOT load without ModSync present if it can be abused in vanilla lobbies.
2. **Define a sync mode** using a static string variable
3. **Initialize your mod** knowing ModSync is available
### Required Plugin Attributes

```csharp
[BepInPlugin("your.mod.id", "Your Mod Name", "1.0.0")]
[BepInProcess("MageArena.exe")]
[BepInDependency("com.magearena.modsync", BepInDependency.DependencyFlags.HardDependency)]
```

## Sync Modes

ModSync supports four different sync modes:

| Mode | Value | Description | Use Case |
|------|--------|-------------|----------|
| **Universal** | `"all"` | Required on both client and host | Gameplay-affecting mods, new spells, cheats |
| **Client Only** | `"client"` | Only needs to be on the client | UI improvements, visual effects, audio changes |
| **Host Only** | `"host"` | Only needs to be on the host | Server management, admin tools, logging |
| **No Sync** | *(omitted)* | Excluded from compatibility checks | Personal preferences, debugging tools |

## Implementation Examples

### Universal Mod (Required by All Players)

```csharp
using BepInEx;
using UnityEngine;

namespace ExampleMod
{
    [BepInPlugin("com.example.mymod", "My Example Mod", "1.0.0")]
    [BepInProcess("MageArena.exe")]
    [BepInDependency("com.magearena.modsync", BepInDependency.DependencyFlags.HardDependency)]
    public class ExampleMod : BaseUnityPlugin
    {
        // This mod requires both client and host to have it
        public static string modsync = "all";
        
        private void Awake()
        {
            // ModSync dependency is guaranteed by BepInEx hard dependency
            Logger.LogInfo("ModSync found! Initializing mod...");
            // Your mod initialization code here
        }
    }
}
```

### Client-Only Mod

```csharp
namespace AnotherExampleMod
{
    [BepInPlugin("com.example.clientonly", "Client Only Mod", "1.0.0")]
    [BepInProcess("MageArena.exe")]
    // The hard BepInEx dependency is NOT required in any mod that isnt tagged as "all"
    [BepInDependency("com.magearena.modsync", BepInDependency.DependencyFlags.HardDependency)]
    public class ClientOnlyMod : BaseUnityPlugin
    {
        // This mod only needs to be on the client
        public static string modsync = "client";
        
        private void Awake()
        {
            // Your mod initialization code here
        }
    }
}
```

### Host-Only Mod

```csharp
namespace HostOnlyExample
{
    [BepInPlugin("com.example.hostonly", "Host Only Mod", "1.0.0")]
    [BepInProcess("MageArena.exe")]
    // The hard BepInEx dependency is NOT required in any mod that isnt tagged as "all"
    [BepInDependency("com.magearena.modsync", BepInDependency.DependencyFlags.HardDependency)]
    public class HostOnlyMod : BaseUnityPlugin
    {
        // This mod only needs to be on the host
        public static string modsync = "host";
        
        private void Awake()
        {
            // Your mod initialization code here
        }
    }
}
```

### No Sync Mod (Excluded from Checking)

```csharp
namespace NoSyncExample
{
    [BepInPlugin("com.example.nosync", "No Sync Mod", "1.0.0")]
    [BepInProcess("MageArena.exe")]
    public class NoSyncMod : BaseUnityPlugin
    {
        // This mod has no modsync variable, so it's excluded from matching
        // It will work regardless of what other players have
        
        private void Awake()
        {
            // Your mod initialization code here
        }
    }
}
```

## Best Practices

### Choosing the Right Sync Mode

**Use `"all"` for:**
- New spells or abilities
- Gameplay mechanics changes
- Balance modifications
- New items or weapons
- Anything that affects game state

**Use `"client"` for:**
- UI improvements
- Visual effects
- Audio replacements
- Cosmetic changes
- Quality of life improvements

**Use `"host"` for:**
- Admin tools
- Server configuration
- Logging systems
- Anti-cheat measures

**Use no sync for:**
- Debug tools
- Personal preferences
- Development utilities
- Non-gameplay affecting tweaks

### Development Tips

1. **Always test compatibility** - Join games with and without your mod to ensure proper behavior
2. **Use descriptive mod IDs** - Make them unique and easily identifiable
3. **Version your mods properly** - ModSync uses version information for compatibility
4. **Document your sync requirements** - Let users know what sync mode your mod uses

### Error Handling

Since ModSync is a hard dependency, your mod won't load if ModSync isn't present:

```csharp
private void Awake()
{
    // No need to check if ModSync exists - BepInEx guarantees it
    Logger.LogInfo("ModSync dependency satisfied, initializing...");
    
    // Your initialization code here
}
```

## Troubleshooting

### Common Issues

**"Mod compatibility mismatch" error:**
- Check that all required mods are installed on both client and host
- Verify mod versions match between players
- Ensure sync modes are set correctly

**Mod not loading:**
- Verify ModSync is installed and working
- Check BepInEx logs for dependency errors
- Confirm your mod's sync mode is declared correctly

**Connection issues:**
- Make sure host-only mods aren't on clients expecting universal sync
- Verify client-only mods aren't required by the host
- Check for conflicting mod versions

### Debug Information

ModSync logs compatibility information to the BepInEx console. Look for:
- Mod discovery messages
- Sync mode detection
- Compatibility check results
- Connection status updates

## Related Documentation

- [BlackMagicAPI Guide](BlackMagicAPI.md) - For creating custom spells and items
- [BepInEx Documentation](https://docs.bepinex.dev/) - Plugin development basics
- [Mage Arena Modding Wiki](/) - General modding information
