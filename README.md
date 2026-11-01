# Underpin Slots

A simple 2D slot machine game made in Unity for the **Underpin Services** assignment.

**Play online:** _add your Unity Play link here_

---

## Game Overview

Pull the lever, spin three reels and try to land the **same symbol on all 3 reels** on the middle line.

- **Home screen:** balance, Start Game, Settings, Exit
- **Game screen:** Settings, Pause / Resume, Balance, Bet (− / +) and Win amount
- **Symbols:** Seven, Cherry, Bell, Bar
- **Win:** 3 matching symbols → prize = bet × symbol multiplier. The win is shown first, then added to your balance.

| Symbol | Prize |
|--------|-------|
| Bar    | bet × 5 |
| Cherry | bet × 10 |
| Bell   | bet × 25 |
| Seven  | bet × 100 |

Your balance, bet and sound setting are **saved locally** and restored next time you play.

## How to Run the WebGL Build

**Online:** open the Unity Play link above.

**On your computer:** WebGL does not run by double-clicking `index.html`, so start a small server:

```bash
cd Build/WebGL
python -m http.server 8000
```

Then open http://localhost:8000

**From Unity:** open the project, open the `Home` scene and press Play.

## Bonus Features

- Saved balance and settings
- Bet selector (10 / 50 / 100 / 500)
- Pause and Resume
- Sound effects and background music, with a Sound ON/OFF setting
- Smooth balance count-up animation

## My Approach

1. **Random result first, animation second.** A weighted random number generator picks the result *before* the reels move. The reels then spin and stop on that result, so the game stays fair and easy to test.
2. **Smooth reels.** The reel moves with `Time.deltaTime`, so it looks the same on any frame rate. It speeds up, spins, then slows down with a small bounce. No Animator is needed.
3. **Clean code (OOP + SOLID).** Each class does one job (wallet, payout, reel, UI, saving). Classes talk through interfaces, and symbols, odds and prizes are stored in a `SlotConfig` asset, so they can be changed without touching code.

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/       app setup and scene loading
│   ├── Economy/    wallet, bet, payout
│   ├── Settings/   sound setting
│   ├── Config/     symbol and payout data
│   ├── Data/       local save
│   ├── Audio/      sound player
│   ├── Slots/      reels, lever, game flow
│   └── UI/         screens and popups
├── Scenes/         Home, Game
├── Resources/      SlotConfig, SoundPlayer prefab
├── UI/  Sounds/  Prefabs/  Animations/
Build/WebGL/        WebGL build
```

## Built With

Unity · C# · Unity UI · TextMeshPro · WebGL

https://github.com/user-attachments/assets/ef0838c8-491e-46a7-ac1d-611d90679999

