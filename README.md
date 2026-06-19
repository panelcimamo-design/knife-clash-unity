# Knife Clash - Valorant Style 3D Game

Professional 3D first-person shooter game inspired by Valorant with advanced graphics, realistic weapon mechanics, and competitive gameplay.

## Features

**Weapons (8 Types)**
- Karambit, Butterfly Knife, Phantom, Vandal, Classic, Sheriff, Operator, Ares
- Realistic damage models and fire rates
- Ammunition and reload mechanics
- Weapon switching system

**Agents (4 Characters)**
- Sage, Phoenix, Jett, Reyna
- Unique abilities for each agent
- Health and armor system
- Agent selection screen

**Game Modes**
- Unrated (Casual)
- Competitive (Ranked)
- Deathmatch (Quick Play)
- Spike Mode (Objective-based)

**Graphics & Effects**
- Dynamic lighting system (4+ light sources)
- Advanced particle effects
- High-quality 3D models
- 4x MSAA anti-aliasing
- Real-time shadows

**Audio System**
- Background music
- Weapon sound effects
- Voice lines
- UI feedback sounds

**Gameplay Mechanics**
- First-person perspective
- Smooth movement and jumping
- Ability system (Q, E, X keys)
- Ultimate ability cooldowns
- Armor and health system
- Team-based gameplay

## Technical Specifications

| Specification | Value |
|---|---|
| Engine | Unity 2022.3.0f1 |
| Platform | Android (APK) |
| Target API | 34 (Android 14) |
| Minimum API | 21 (Android 5.0) |
| Architecture | ARM64, ARMv7 |
| Target FPS | 60 FPS |
| Graphics Quality | High (4x MSAA) |
| Audio Quality | 48kHz |
| Estimated Size | 200+ MB |

## Controls

| Action | Key |
|---|---|
| Move Forward | W |
| Move Backward | S |
| Move Left | A |
| Move Right | D |
| Jump | Space |
| Sprint | Left Shift |
| Shoot | Left Mouse Button |
| Reload | R |
| Ability 1 | Q |
| Ability 2 | E |
| Ultimate | X |
| Switch Weapon | 1-8 |

## Installation

1. Download the latest APK from Releases
2. Enable "Unknown Sources" in Android Settings
3. Install the APK
4. Launch the game

## System Requirements

**Minimum**
- Android 5.0 (API 21)
- 2GB RAM
- 250MB Storage

**Recommended**
- Android 10+ (API 29+)
- 4GB+ RAM
- 500MB+ Storage
- Snapdragon 800 or equivalent

## Build Instructions

### Prerequisites
- Unity 2022.3.0f1
- Android SDK (API 21-34)
- Android NDK
- Java Development Kit (JDK 11+)

### Building APK

```bash
# Open project in Unity
unity -projectPath .

# Build APK
File → Build Settings → Android → Build

# Or via command line
unity -projectPath . -executeMethod BuildScript.BuildAndroidAPK -nographics -batchmode -quit
```

## Project Structure

```
knife-clash-game/
├── GameManager.cs          # Main game logic
├── MenuManager.cs          # Menu system
├── PlayerController.cs     # Player movement and combat
├── .github/
│   └── workflows/
│       └── build.yml       # GitHub Actions CI/CD
└── README.md
```

## Development

### Scripts Overview

**GameManager.cs**
- Weapon and agent data
- Game mode management
- Team and player tracking
- Lighting setup
- Audio management

**MenuManager.cs**
- Main menu interface
- Agent selection
- Weapon selection
- Game mode selection
- Settings panel

**PlayerController.cs**
- First-person movement
- Weapon handling
- Ability system
- Combat mechanics
- Health and armor

## Performance Targets

- 60 FPS on mid-range devices
- < 512 MB RAM usage
- < 5 seconds load time
- Low battery impact

## Optimization Techniques

- Level of Detail (LOD) for models
- Texture atlasing
- Object pooling for effects
- Efficient UI rendering
- Shader optimization
- Memory management

## License

MIT License - Feel free to use and modify

## Support

For issues and questions:
- GitHub Issues: https://github.com/panelcimamo-design/knife-clash-unity/issues
- Discord: https://discord.gg/knifeclash

## Credits

- Inspired by Valorant (Riot Games)
- Built with Unity Engine
- Community contributions welcome

---

**Version**: 1.0.0  
**Last Updated**: June 19, 2026  
**Developer**: Knife Clash Team
