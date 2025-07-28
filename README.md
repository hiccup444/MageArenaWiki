# 🥣 Soup Effects Guide

A breakdown of all consumable soups in the game and their effects when used by the player.

---

## 🧪 Crystal Soup (ID: 0)

**Flavor Text:**  
*A surge of magical energy passes through you...*

**Effects:**
- Increases **stamina bonus** by `+1`
- Increases **movement speed** by `+2`
- Increases **player level** by `+1` (if `crystalCDReduction < 4`)
- Reduces **cooldown durations** (up to 4 times)
- Grants a temporary buff (via `SubtractBonus(5)`)
- Increments `soupsdrank` counter

---

## 🌳 Log Soup (ID: 1)

**Flavor Text:**  
*Your skin hardens like tree bark...*

**Effects:**
- Increases **health** by `+50`
- Increments `soupsdrank` counter

---

## 🍄 Mushroom Soup (ID: 2)

**Flavor Text:**  
*Your joints feel like rubber...*

**Effects:**
- Increases **jump bonus** by `+5`
- Grants a temporary buff (via `SubtractBonus(2)`)
- Increments `soupsdrank` counter

---

## 🐸 Frog Soup (ID: 3)

**Flavor Text:**  
*Your tongue gains a mind of its own...*

**Effects:**
- Increases **number of licks** by `+3`
- Triggers special behavior (`FrogStewRoutine`)
- Increments `soupsdrank` counter

---

## 🪨 Rock Soup (ID: 4)

**Flavor Text:**  
*Your voice bellows from the mountain tops...*

**Effects:**
- Activates **loud voice ability** via `ServerToggleLoud()`
- Increments `soupsdrank` counter

---

## 🚬 Pipe (Bonus, ID: 5)

> Not technically a soup, but handled by the same function.

**Effects:**
- Increases **player level** and **cooldown reduction** (if `crystalCDReduction < 4`)
- Grants a temporary buff (via `SubtractBonus(5)`)
- Increments `pipessmoked` counter
