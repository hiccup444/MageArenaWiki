# Soup Effects Documentation

This document outlines the effects of different soup types in the game.

## Soup Types and Effects

### Crystal Soup
**Display Name:** "Crystal Soup"

**Effects:**
- Increases stamina bonus by +1.0
- Increases speed boost by +2.0
- Increases level by +1 (if crystal CD reduction < 4)
- Increases crystal CD reduction by +1 (max 4)
- Effect duration: 120 seconds (2 minutes)

**Message:** "A surge of magical energy passes through you..."
---
### Log Soup
**Display Name:** "Log Soup"

**Effects:**
- Instantly heals +50 health points

**Message:** "Your skin hardens like tree bark..."

### Mushroom Soup
**Display Name:** "Mushroom Soup"

**Effects:**
- Increases jump bonus by +5.0
- Effect duration: 120 seconds (2 minutes)

**Message:** "Your joints feel like rubber..."

### Frog Soup
**Display Name:** "Frog Soup"

**Effects:**
- Grants +3 frog tongue licks
- Reduces speed boost by -1.5 (applied after 120 seconds)
- Frog tongue effect:
- - The frog tongue automatically fires when your camera crosshair is pointing at another player within 30 units.
- - Each successful hit applies frog force and makes target drop items

**Message:** "Your tongue gains a mind of its own..."

### Rock Soup
**Display Name:** "Rock Soup"

**Effects:**
- Activates loud/echo voice mode
- Effect duration: 4 seconds

**Message:** "Your voice bellows from the mountain tops..."

### Pipe Smoking

**Effects:**
- Increases level by +1 (if crystal CD reduction < 4)
- Increases crystal CD reduction by +1 (max 4)
- Effect duration: 15 seconds

## Important Notes

### Crystal CD Reduction
- Maximum crystal CD reduction is capped at 4
- Both Crystal Soup and Pipe Smoking can increase this value, which affects spell cooldowns in-game.

### Effect Stacking
- Multiple soup effects can be active simultaneously
- Speed boosts and other numerical bonuses appear to stack additively
- Some effects have different durations and may expire independently

### Bonus Removal System
The game uses a coroutine system (`SubtractBonus`) to remove temporary effects:
- **stewid 0 (Crystal):** Removes after 120 seconds
- **stewid 2 (Mushroom):** Removes after 120 seconds  
- **stewid 3 (Frog):** Speed penalty applied after 120 seconds
- **stewid 4 (Rock):** Removes after 4 seconds
- **stewid 5 (Pipe):** Removes after 15 seconds
