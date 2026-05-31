# Zig Zag Prototype

A mobile game prototype built in Unity 2022.3.62f2, inspired by Ketchapp's Zig Zag.

## Gameplay
Tap the screen to change the ball's direction and keep it on the platforms as long as possible. Collect diamonds along the way and try to beat your high score.

## Features
- Infinite platform generation with random direction changes
- Object Pooling for platforms and diamonds
- Progressive difficulty (speed increases over time)
- Dynamic color system with smooth transitions
- Score tracking and persistent high score
- Diamond collection with floating feedback text
- Squash & stretch animation on direction change
- Game Over screen with final stats

## Technical Highlights
- Object Pooling to avoid GC spikes on mobile
- Event-driven color system using C# events
- Singleton pattern for core managers
- Coroutine-based animations and timers

## Controls
- **Mouse click / Tap** → Change direction

## Built With
- Unity 2022.3.62f2
- C#
