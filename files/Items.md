# Items Documentation

## Frog Balloon (Item ID: 23)

* **Effect:** Provides fall damage immunity when held
* **Movement Bonus:** +2 running speed, +1.5 walking speed (when not in bog)
* **Animation:** Floating balloon animation when picked up or dropped
* **Sound:** Frog croak sound with random pitch
---
## Frog Blade (Item ID: 10)

* **Damage:** 30 HP per hit
* **Special Effect:** Hold-to-attack system that drains target while contact is maintained
* **Movement Debuff:** Gradually reduces target's movement speed and mouse sensitivity to near-zero
* **Recovery:** Target movement slowly returns to normal after contact ends
* **Blood Effects:** Creates blood splatter on successful hits
* **Spark Effects:** Creates sparks when hitting rock/wall surfaces
---
## Frog Spear (Item ID: 22)

* **Special Ability:** Tongue Pull - forces hit players to drop their items
* **Animation:** Extending spear animation during attack
* **Single Hit:** Unlike Frog Blade, triggers effect once per attack
* **Range:** Extended reach with spear extension animation
---
## Spore Frog (Item ID: 21)

* **Ability:** Spawns smoke cloud at frog location
* **Cooldown:** 15-second cooldown between uses
* **Activation:** Right-click interaction when equipped
* **Animation:** Squeeze animation when activating
* **Area Effect:** Creates smoke cloud that affects area around frog
---
## Excalibur Components

### Hilt of Excalibur (Item ID: 8)
* **Type:** Collectible component
* **No Combat Function:** Cannot be used as weapon
* **Crafting Material:** Used to create Excalibur Reforged

### Blade of Excalibur (Item ID: 9)
* **Type:** Collectible component  
* **No Combat Function:** Cannot be used as weapon
* **Crafting Material:** Used to create Excalibur Reforged

## Pull Excalibur (World Interaction)

* **Interaction Time:** 10-second channeled interaction
* **Requirements:** Must have free inventory slot
* **Effect:** Spawns Excalibur Reforged and automatically picks it up

## Excalibur Reforged (Item ID: 26)

* **Damage:** 105 HP per hit (massive damage)
* **Hold Attack:** Same hold-to-attack system as Frog Blade
* **Movement Debuff:** Drains target movement speed and sensitivity during contact
* **Blood/Spark Effects:** Creates appropriate visual effects based on surface hit
---
## Mushroom Sword (Item ID: 18)

* **Special Ability:** Knockback attack - launches players away from impact point
* **Wall Bounce:** Hitting walls causes wielder to bounce backwards
* **Single Hit:** Unlike hold-attack weapons, triggers effect once per swing
* **Sound Effect:** Plays boing sound when hitting walls or players
---
## Ritual Dagger (Item ID: 17)

* **Damage:** 30 HP per hit
* **Hold Attack:** Uses standard hold-to-drain system
* **Base Weapon:** Default sword with no special abilities
* **Effects:** Blood splatter on hits, sparks on hard surfaces
---
## Silverseed Bramble (Item ID: 20)

* **Ability:** Summons spike field at targeted location
* **Range:** 15 units maximum distance
* **Charges:** 3 maximum charges (visual indicator scales with charge count)
* **Cooldown:** 30 seconds per charge regeneration
* **Pattern:** Creates 5×5 grid of spikes centered on target point
* **Duration:** Spikes remain for 60 seconds before retracting
---
## Spike Field Mechanics

### Spike Growth Pattern
* **Grid Size:** 5×5 spikes (25 total spikes)
* **Spawn Timing:** Instant spike creation
* **Growth Animation:** Spikes rise from ground over time
* **Damage:** Continuous damage while standing on spikes
* **Retraction:** Automatic removal after 60 seconds
---
### Levitator (Item ID: 15)
* **Function:** Levitates target players using magical force
* **Energy System:** 30 seconds maximum energy, drains 2 energy/second during use
* **Regeneration:** Slowly refills energy when not in use
* **Target Control:** Forces target to move toward levitation point
* **Audio:** Continuous sound effect while levitating
---
### Ray of Shrink (Item ID: 14)
* **Function:** Shrinks target players to half size
* **Cooldown:** 30-second cooldown between uses
* **Duration:** 40 seconds shrink effect
* **Audio Effect:** Changes target's voice to high pitch
* **Animation:** Weapon has "pew" animation when firing
* **Range:** Configurable maximum distance for targeting
