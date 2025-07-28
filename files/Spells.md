# Spell Effects Documentation

## Fireball

* **Muzzle Velocity:** 10 units/second
* **High Trajectory:** Calculates arcing path for maximum range
* **Low Trajectory:** Reduced velocity (÷1.5) and gravity (÷1.75) for shorter arcs
* Creates explosion at impact location

### Explosion Effects:
* **Blast Radius:** Scales up to level 8
* **Damage:** 60 - (distance × 10) + level HP (clamped 5-100)
* **Self-Damage:** Reduced by 1.5× if hitting yourself
* **Knockback:** 3 × ((10 - distance + level) ÷ 2) horizontal force
* **Upward Force:** 10 + level + height difference
* **Fire Timer:** 8 - distance + (level ÷ 2) seconds (if not self-damage)
* **Camera Shake:** Applied to all players within 100 units
---
## Frost Bolt

* **Muzzle Velocity:** 80 units/second
* Same ballistic trajectory system as Fireball
* **Against Players:** freezes Target In Ice
* **Against Wormholes:** Destroys wormhole
* **Against Hitable Objects:** Triggers interaction (break pot, etc)
* **Cooldown:** Players can only be hit by an icicle once per second.

### Ice Box Effects by Level:

**Level 1:**
* Movement speed reduced by 50% for 4 seconds  
* Animation speed reduced to 50%
* 10 HP damage
* -3 speed boost penalty

**Level 2:**
* Complete freeze for 1 second (cannot move, jump, or look)
* 25 HP damage
* -3 speed boost penalty

**Level 3:**
* Complete freeze for 1.5 seconds
* 30 HP damage  
* -6 speed boost penalty (double)

* **Breakout:** Ice can be broken by taking fireball explosion damage
---
## Magic Missile

* **Muzzle Velocity:** 20 units/second
* **Homing System:** Tracks targets within 30-unit sphere
* **Search Cone:** 90-degree forward cone from launch direction
* **Timeout:** 4-second maximum flight time
* **Target Priority:** Distance + angle weighting
* Ignores same-team players

### Damage:
* **AI Shot:** 10 HP
* **Player Shot:** 12.5 + (level ÷ 2) HP
* **Against NPCs:** 14 + (level × 2) HP
---
## Dark Blast

* **Shape:** Box-shaped beam
* **Duration:** 0.8 seconds of damage ticks
* **Tick Rate:** Every 0.1 seconds
* **Range:** 70 units forward from caster
* **Damage per Hit:** 12 HP + 2 seconds fire timer
* **Against NPCs:** 15 HP per hit
* **Multi-Hit:** Can hit same target multiple times during duration
* **Destroys:** Wormholes, lights braziers, triggers hitable objects
---
## Blink

* **Range:** Up to 15 units forward
* **Targeting Priority:**
  1. 15 units forward + 10 units up, raycast down
  2. 5 units forward + 5 units up, raycast down  
  3. 20 units forward at eye level
* **Teleportation:** Instant movement with position confirmation over 5 frames

## Holy Light (Divine)

* **Range:** 40-unit radius sphere
* **Effect:** Heals all players within range for 50 HP
* **Delay:** 0.25 seconds before healing applies
* **Team Targeting:** Affects all players regardless of team
* **Visual:** Large light beam from 40 units above caster
