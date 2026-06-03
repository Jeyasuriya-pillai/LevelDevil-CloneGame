# 🎮 Level Devil Clone - 2D Trap Platformer

<p align="center">
  <img src="https://img.shields.io/badge/Made%20With-Unity-black?style=for-the-badge&logo=unity&logoColor=white" alt="Unity Badge" />
</p>

Welcome to the **Level Devil Clone**, a punishing, high-precision 2D puzzle-platformer heavily inspired by the psychological tricks of the original hit game! Built entirely within **Unity**, this project showcases invisible activation zones, proximity-triggered blade matrices, seamless audio loops, state-driven animations, and asynchronous scene transition states.

---

## 🌟 Game Overview & Key Features

This project was built from scratch as a personal learning milestone to master technical engineering workflows within Unity Hub, C# architecture, and native 2D systems.

* **🏃 Responsive Grid Movement:** Fluid, physics-backed horizontal mechanics enabling swift forward and backward adjustments alongside fine-tuned jump thresholds.
* **⚙️ Proximity-Triggered Traps:** Advanced logic profiles where hazards dynamically track the player's coordinate vectors, breaking patrol pathways to spring surprise attacks instantly.
* **🔊 Persistent Sound Architecture:** A standalone `AudioManager` utilizing singleton blueprints to handle stateful running loops and instant one-shot audio clips cleanly without memory fragmentation.
* **🎭 State-Driven Animation Engine:** Automated sprite processing matrix running fluid transitions between Idle, Run, Jump, Fall, Death explosions (`PlayerDeathAni`), and Respawn sequences (`PlayerRegAni`).
* **🎬 Asynchronous Level Loading:** Fully managed scene logic utilizing background threads and screen-fading animator callbacks to transition maps without freezing frame rates.

---

## 📸 Game Screenshots & Media

### 🗺️ Level Design Maps
<p align="center">
  <img src="GamePlayPreview's/Level 1.png" width="32%" alt="Level 1 Layout" />
  <img src="GamePlayPreview's/Level 2.png" width="32%" alt="Level 2 Layout" />
  <img src="GamePlayPreview's/Level 3.png" width="32%" alt="Level 3 Layout" />
</p>

### 🎞️ Gameplay Preview Video
Watch the full execution loop, animation switching, and level designs in motion here:

👉 [Download The GamePlay Video From Here](https://github.com/Jeyasuriya-pillai/LevelDevil-CloneGame/blob/main/GamePlayPreview's/GamePlay.mp4)

---

## 🎮 Game Controls

| Action | Control Key |
| :--- | :--- |
| **Move Left / Backward** | ⬅️ Arrow Left / **A** |
| **Move Right / Forward** | ➡️ Arrow Right / **D** |
| **Jump** | **Spacebar** |
| **Survive Traps** | **Your Instincts** *[Watch out for sudden changes!]* |

---

## 🛠️ Project Architecture & Technical Stack

* **Game Engine:** Unity Engine (Managed cleanly via Unity Hub)
* **Scripting Language:** C# (Object-Oriented Architecture)
* **Physics Framework:** Unity Physics2D (`Rigidbody2D`, `OverlapCircle` ground masking, dynamic target interpolation)
* **Patterns Used:** Singleton Pattern (For persistent Audio and Scene Controllers)

---

## 📝 Developer Profile

This game clone was completely conceptualized, coded, and built individually for educational skill scaling.

* **Developer:** Jeyasuriya
* **Objective:** Master Unity workflow, manage cross-scene reference persistency, manipulate custom 2D transform systems, and streamline UI transition handlers.
