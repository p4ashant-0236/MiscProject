# Card Matching Game

A feature-rich Unity-based card matching puzzle game designed to test memory, strategy, and quick decision-making. This project was developed as part of a technical test and showcases clean architecture, modular code design, and scalable gameplay systems.

---

## 🎮 Gameplay Overview

- Flip and match pairs of cards on a dynamic grid.
- Grid size is customizable (e.g., 4x4, 5x6).
- The game tracks combos and mismatches.
- Power-ups are available to assist the player.
- Animated card flipping and sound effects for immersive feedback.

---

## 🛠 Features

### 🔊 Audio System
- Background music and sound effects.
- Sound types managed via `AudioManager` using `AudioType` enum and `AudioDataSO`.

### 💡 Power-Up System
- Two types of power-ups:
  - **Show Two Cards**: Temporarily reveals two unmatched cards.
  - **Reveal All Cards**: Briefly reveals the entire board.
- Uses interfaces and abstract base classes (`IPowerUp`, `BasePowerUp`) for clean extensibility.
- Tracks remaining uses per power-up.

### 💥 Combo System
- Tracks consecutive matches.
- Displays combo messages (e.g., “Combo X3!”).
- Resets on mismatch.

### 📐 Dynamic Grid Resizing
- Uses `GridLayoutGroup` and `RectTransform` to automatically adjust the card size and spacing depending on the screen or window size.
- Configurable spacing and padding for responsiveness.

---

## 🧩 Architecture Highlights

- **Event-Driven**: Uses `EventManager` to broadcast game events like match or mismatch.
- **Modular & Scalable**: Systems like audio, power-ups, and combos are loosely coupled and easy to expand.
- **Singleton Managers**: For global access to core systems like `AudioManager`, `ComboSystemManager`, and `PowerUpManager`.

---
