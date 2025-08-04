# BlackMagicAPI

[The BlackMagicAPI framework](https://thunderstore.io/c/mage-arena/p/D1GQ/BlackMagicAPI/wiki/3840-blackmagicapi-guide) for creating custom spells and items in Mage Arena.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Basic Setup](#basic-setup)
- [Creating Spells](#creating-spells)
- [Creating Items](#creating-items)
- [Asset Loading](#asset-loading)
- [Troubleshooting](#troubleshooting)

## Prerequisites

### Required Using Statements
```csharp
using BlackMagicAPI.Enums;
using BlackMagicAPI.Modules.Spells;
using BlackMagicAPI.Helpers;
using BlackMagicAPI.Managers;
```

### Project Configuration
- **.NET Framework**: 4.8 (required)
- If you were previously using 4.7.2, you'll need to upgrade:
  1. Right-click project → Properties
  2. Change "Target Framework" to .NET Framework 4.8
  3. Rebuild your project

## Basic Setup

1. Create a BepInEx plugin project
2. Reference BlackMagicAPI.dll
3. Create a main plugin class inheriting from BaseUnityPlugin:

```csharp
[BepInPlugin("your.mod.id", "Your Mod Name", "1.0.0")]
public class MySpellMod : BaseUnityPlugin
{
    private void Awake()
    {
        // Register your spells here
        SpellManager.RegisterSpell(this, typeof(YourSpellData), typeof(YourSpellLogic));
    }
}
```

## Creating Spells

### 1. Spell Data (Configuration)

Create a class inheriting from SpellData:

```csharp
internal class YourSpellData : SpellData
{
    public override SpellType SpellType => SpellType.Page; // Or other types
    public override string Name => "YourSpellName";
    public override float Cooldown => 5f; // Cooldown in seconds
    public override Color GlowColor => Color.blue; // Visual glow color
    
    // Optional: Override textures
    public override Texture2D? GetMainTexture() { ... }
    public override Texture2D? GetEmissionTexture() { ... }
	
    // Optional: Manually load a SpellLogic prefab on spell creation
    public override Task<SpellLogic?> GetLogicPrefab() { ... }
}
```

### 2. Spell Logic (Behavior)

Create a class inheriting from SpellLogic:

```csharp
internal class YourSpellLogic : SpellLogic
{
    public override void CastSpell(GameObject playerObj, Vector3 direction, int level)
    {
        // Your spell logic here
        // Example: Damage the player
        playerObj.GetComponent<PlayerMovement>().DamagePlayer(100f, playerObj, "your_spell");
    }
}
```

### Texture Setup (Optional)

#### Standard Texture Location
```
MageArena/
└── BepInEx/
    └── plugins/
        └── YourModName/
            └── Sprites/          ← Automatic loading
                ├── 🔳 YourSpellName_Main.png
                └── ✨ YourSpellName_Emission.png
```

#### Advanced Loading Methods

The API provides multiple ways to load textures:

**1. From Disk (Automatic)**
```csharp
// Default behavior - loads from /Sprites/ folder
public override Texture2D? GetMainTexture() 
{
    // Optional manual path specification
    string customPath = Path.Combine(Path.GetDirectoryName(Plugin.Info.Location), 
                                   "CustomFolder/custom_texture.png");
    return Utils.LoadTextureFromDisk(customPath);
}
```

**2. From Embedded Resources**
```csharp
// For textures bundled inside your DLL
public override Texture2D? GetMainTexture()
{
    return Assembly.GetExecutingAssembly()
           .LoadTextureFromResources("YourNamespace.Resources.texture.png");
}
```

**3. Sprite Creation**
```csharp
// Create sprites with custom parameters
var sprite = Utils.LoadSpriteFromDisk(path, pixelsPerUnit: 100f);
// or
var resourceSprite = thisAssembly.LoadSpriteFromResources(path, pixelsPerUnit: 64f);
```

### Registering Spells

In your plugin's Awake():

```csharp
SpellManager.RegisterSpell(this, typeof(YourSpellData), typeof(YourSpellLogic));
```

## Creating Items

### Item Creation Basics

#### Required Project Setup
- Same as spells (BepInEx plugin, .NET 4.8)
- Additional required using statements:
```csharp
using BlackMagicAPI.Modules.Items;
using FishNet.Object;
```

#### Registration Method
```csharp
[BepInPlugin("your.item.mod", "Item Mod", "1.0.0")]
public class MyItemMod : BaseUnityPlugin
{
    private void Awake()
    {
        // Register items (behavior type is optional if using prefabs)
        ItemManager.RegisterItem(this, typeof(YourItemData), typeof(YourItemBehavior));
    }
}
```

### Item Data Configuration

```csharp
internal class HealthPotionData : ItemData
{
    public override string Name => "Health Potion";
    public override bool CanSpawnInTeamChest => true;
}
```

**Advanced Item Configuration:**
```csharp
internal class DemonHeartData : ItemData
{
    public override string Name => "Demon Heart";
    
    // Spawning rules
    public override bool CanSpawnInTeamChest => true;
    public override bool CanGetFromTrade => true;
    
    // Custom sounds (optional)
    public override AudioClip? GetPickupAudio() 
    {
        return Assembly.GetExecutingAssembly()
               .LoadWavFromResources("YourMod.Resources.heart_pickup.wav");
    }
    
    // Custom prefab (optional)
    public override async Task<ItemBehavior?> GetItemPrefab()
    {
        GameObject prefab = await AssetBundleLoader.LoadFromBundle("heart_bundle");
        return prefab.GetComponent<ItemBehavior>();
    }
}

### Item Behavior Logic

```csharp
internal class DemonHeartBehavior : ItemBehavior
{
    protected override void OnItemUse(GameObject owner)
    {
        SendItemSync(1, owner.GetComponent<NetworkObject>().ObjectId); 
    }

    protected override void HandleItemSync(uint syncId, object[] args)
    {
        if (syncId == 1)
        {
            int playerId = (int)args[0];
            // Handle networked effects...
        }
    }
}
```

### Network Synchronization

Use these methods in ItemBehavior:

```csharp
// Send data to all clients
SendItemSync(syncID, param1, param2);

// Receive sync data
protected override void HandleItemSync(uint syncId, object[] args)
{
    switch(syncId)
    {
        case 1: // Your custom event
            int playerId = (int)args[0];
            // Handle logic...
            break;
    }
}
```

## Asset Loading

### File Structure
```
BepInEx/
└── plugins/
    └── YourMod/
        ├── Sprites/
        │   └── 🔳 DemonHeart_Ui.png    ← Auto-loaded UI icon
        └── Sounds/
            ├── 🔊 DemonHeart_Pickup.wav
            └── 🔊 DemonHeart_Equip.wav
```

### Loading Methods

#### Automatic Loading
- **UI sprites**: `{ItemName}_Ui.png` in Sprites folder
- **Sounds**: `{ItemName}_{Pickup/Equip}.wav` in Sounds folder

#### Manual Loading

```csharp
// From absolute path
Texture2D tex = Utils.LoadTextureFromDisk("C:/path/to/texture.png");

// From embedded resources
AudioClip clip = Assembly.GetExecutingAssembly()
                  .LoadWavFromResources("Namespace.Resources.sound.wav");
```

#### Loading Prefabs
```csharp
public override async Task<ItemBehavior?> GetItemPrefab()
{
    // From AssetBundle
    GameObject prefab = await AssetBundleLoader.LoadFromBundle("items/prefabs");
    
    // From scene object (editor)
    // GameObject prefab = Instantiate(yourPrefabReference);
    
    return prefab.GetComponent<ItemBehavior>();
}
```

## Key Features

- **Auto-caching**: Textures/sprites are cached automatically
- **Error Handling**: Fails gracefully with error logging

## Troubleshooting

- ❌ **"File does not exist"**: Check case sensitivity and path
- ❌ **"Failed to load image"**: Verify PNG is uncompressed/not corrupt
- ℹ️ **All errors log to BepInEx console**

## Other Links

- [ModSync Guide](ModSync.md) - For syncing mods between all players
- [BepInEx Documentation](https://docs.bepinex.dev/) - Plugin development basics
- [Mage Arena Modding Wiki](/) - General modding information
