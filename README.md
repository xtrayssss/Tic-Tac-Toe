# 🎲 Tic-Tac-Toe
A modern implementation of the classic Tic Tac Toe game built with Unity, featuring AI and random opponents.

## ✨ Features
- Two game modes:
 - 🤖 Play against Yandex GPT AI for strategic matches
 - 🎲 Play against Random Player for casual games
- 🚀 Smooth animations using PrimeTween
- ⚡ Async/await pattern with UniTask for efficient operations
- 🛠️ Enhanced inspector and serialization with Odin
- 📱 Clean and responsive UI
- 🎯 Win state detection with visual feedback

## 🛠️ Technical Stack
- 🎮 Unity 2022.x
- 💻 C# with modern async patterns
- 📚 Key Libraries:
 - [UniTask](https://github.com/Cysharp/UniTask) - Zero allocation async/await integration
 - [Odin Inspector](https://odininspector.com) - Enhanced Unity inspector and serialization
 - [PrimeTween](https://github.com/KyryloKuzyk/PrimeTween) - High-performance animation system
 - Yandex GPT API integration

## 🎮 Game Modes
### 🤖 VS Yandex GPT
> Strategic AI opponent powered by language processing
![AI Demo](Documentation/Images/AI.gif)

### 🎲 VS Random Player
> Casual mode with randomized moves
![Random Demo](Documentation/Images/Random.gif)

## 📋 Requirements
- Unity 2022.x or higher
- Odin Inspector Asset
- PrimeTween Asset
- UniTask Package
- Internet connection (for AI mode)

## 🚀 Installation
1. Clone the repository
2. Open in Unity 2022.x
3. Install required packages:
  - Import Odin Inspector from Asset Store
4. Configure Yandex GPT API credentials
5. Build and play!

## 🎮 Core Gameplay Demo
<p align="center">
 <img src="Documentation/Images/Gameplay.gif" alt="Gameplay Demo"/>
</p>
<p align="center"><i>Experience smooth animations and responsive gameplay</i></p>

## 📝 License
MIT License

Copyright (c) 2024 [xtrayssss]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
