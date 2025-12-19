# 05_AI_Game 🤖

This folder contains advanced **Enemy AI projects** developed in **Unreal Engine** and **Unity**, focusing on perception, state-driven behaviour, and player detection systems.

---

## 🕹️ Unreal Engine: Enemy AI Behaviour System

### 🧠 Overview

An Unreal Engine project demonstrating a state-driven **Enemy AI system** with multiple behaviours:

- **Idle Patrol**: Enemy follows predefined patrol routes  
- **Chase**: Enemy actively pursues the player when detected  
- **Search**: Enemy searches the last known player location after losing sight  

This project focuses on **AI state management**, perception, and responsive enemy behaviour.

### 🚀 Features

- Finite State Machine–style AI behaviour  
- Patrol system with waypoints  
- Player detection and chase logic  
- Search behaviour when player is lost  
- Player movement using the **Enhanced Input System**

### 🎮 Controls

| Action      | Key |
|-------------|-----|
| Move        | WASD |
| Jump        | Space |
| Sprint      | Left Shift |
| Crouch      | Ctrl |

### 📦 Build

The Windows build is provided as a zipped folder.

**Download:**  
[Unreal Enemy AI Build](./Builds/Unreal_AI_Game_Enhanced.zip)

### 📸 Screenshots / GIFs

- `images/unreal-ai-patrol.gif` → Enemy patrolling behaviour  
- `images/unreal-ai-chase.gif` → Enemy chasing the player  
- `images/unreal-ai-search.gif` → Enemy searching last known position  

---

## 🕹️ Unity: Enemy AI with Light & Sound Detection

### 🧠 Overview

A Unity project demonstrating an **Enemy AI system** with more complex player detection mechanics:

- **Patrol, Chase, and Search states**
- **Light detection**: Player visibility affected by lighting conditions
- **Sound detection**: Detection based on player movement speed
- Stealth-focused gameplay mechanics

This project emphasizes **stealth AI design** and sensory-based detection.

### 🚀 Features

- Enemy patrol, chase, and search behaviours  
- Light-based player visibility detection  
- Sound detection influenced by player movement speed  
- Dynamic chase distance based on player behaviour  
- Stealth-oriented AI logic

### 🔗 Source Code

This Unity project is hosted in a separate repository:

👉 **Enemy AI with Light & Sound Detection (Unity)**  
https://github.com/JAndrews004/COMP2007CW2

### 📸 Screenshots / GIFs

- `images/unity-ai-light-detection.gif` → Light-based detection  
- `images/unity-ai-sound-detection.gif` → Sound detection via movement  
- `images/unity-ai-chase.gif` → Enemy chase behaviour  

---

## 🛠️ Technologies & Concepts Used

### 🟪 Unreal Engine
- Enhanced Input System  
- AI state-driven behaviour  
- Patrol, chase, and search logic  
- Blueprint-based AI systems  

### 🟦 Unity
- C# scripting  
- AI state management  
- Light-based detection logic  
- Movement-speed-based sound detection  
- Stealth gameplay concepts  

---

## 📌 Notes

- This folder represents the most advanced AI work in the portfolio.  
- Projects focus on **player perception**, **enemy decision-making**, and **reactive AI behaviour**.  
- Demonstrates progression from basic systems to more complex AI logic.
