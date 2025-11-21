# 3DO Projects - Quick Start Guide

## 🔨 Rebuilding Projects

### Option 1: Rebuild (Requires .NET 8.0 installed)
```bash
cd C:\users\rob\source\002\3do-fix
rebuild.bat
```

**Output:**
- `3DOLauncher-Release\3DOLauncher.exe` - Game launcher
- `ThreeDOEmulator-Release\ThreeDOEmulator.exe` - Custom emulator

### Option 2: Rebuild Standalone (No .NET required to run)
```bash
cd C:\users\rob\source\002\3do-fix
rebuild-standalone.bat
```

**Output:**
- `3DOLauncher-Standalone\3DOLauncher.exe` - Includes .NET runtime
- `ThreeDOEmulator-Standalone\ThreeDOEmulator.exe` - Includes .NET runtime

**Note:** Standalone builds are larger (~60MB) but can run on any Windows PC.

---

## 🎮 Using the Game Launcher

### Interactive Mode
```bash
cd C:\users\rob\source\002\3do-fix\3DOLauncher-Release
3DOLauncher.exe
```

Then select a game by number.

### Command-Line Mode
```bash
3DOLauncher.exe 1    # Launch game #1
3DOLauncher.exe 4    # Launch game #4 (Road Rash)
```

**Note:** 3DOLauncher uses 4DO emulator, which has CHD loading issues (INSERT CD screen).

---

## 🛠️ Using the Custom Emulator

### Run BIOS only
```bash
cd C:\users\rob\source\002\3do-fix\ThreeDOEmulator-Release
ThreeDOEmulator.exe
```

### Run with a game (CUE file)
```bash
ThreeDOEmulator.exe "C:\Users\rob\Games\3DO\ssf2t.cue"
```

**Note:** Custom emulator only supports CUE/BIN format (not CHD).

---

## 💿 Converting CHD to CUE/BIN

The custom emulator needs CUE/BIN format. Convert your CHD files:

```bash
cd C:\Users\rob\Games\3DO\MAME

# Convert a game
chdman.exe extractcd -i "..\Road Rash (USA).chd" -o "..\roadrash.cue" -ob "..\roadrash.bin"

# Convert Super Street Fighter (already done)
chdman.exe extractcd -i "..\Super Street Fighter II Turbo (USA).chd" -o "..\ssf2t.cue" -ob "..\ssf2t.bin"
```

Then run:
```bash
ThreeDOEmulator.exe "C:\Users\rob\Games\3DO\roadrash.cue"
```

---

## 📁 Project Structure

```
C:\users\rob\source\002\3do-fix\
├── rebuild.bat                    # Rebuild script (requires .NET)
├── rebuild-standalone.bat         # Rebuild with .NET included
│
├── 3DOLauncher-Release/           # After rebuild.bat
│   └── 3DOLauncher.exe           # Quick launcher (uses 4DO)
│
├── ThreeDOEmulator-Release/       # After rebuild.bat
│   └── ThreeDOEmulator.exe       # Custom emulator
│
├── 3DOLauncher-Standalone/        # After rebuild-standalone.bat
│   └── 3DOLauncher.exe           # Standalone launcher
│
├── ThreeDOEmulator-Standalone/    # After rebuild-standalone.bat
│   └── ThreeDOEmulator.exe       # Standalone emulator
│
├── 3DOLauncher/                   # Source code for launcher
├── ThreeDOEmulator/               # Source code for emulator
├── SUMMARY.md                     # Complete project summary
└── README.md                      # Overview
```

---

## 🎯 Which One Should I Use?

### For Quick Testing (Right Now)
Use **3DOLauncher** - it works but shows INSERT CD screen:
```bash
3DOLauncher-Release\3DOLauncher.exe
```

### For Development/Future
Use **ThreeDOEmulator** - custom built from scratch:
```bash
# First convert CHD to CUE/BIN
cd C:\Users\rob\Games\3DO\MAME
chdman.exe extractcd -i "..\game.chd" -o "..\game.cue" -ob "..\game.bin"

# Then run
cd C:\users\rob\source\002\3do-fix\ThreeDOEmulator-Release
ThreeDOEmulator.exe "C:\Users\rob\Games\3DO\game.cue"
```

---

## 🔄 When to Rebuild

Run `rebuild.bat` when:
- You modify source code in `3DOLauncher/` or `ThreeDOEmulator/`
- You pull updates from git
- Executables seem outdated or broken

---

## 📝 Your Games

Found in `C:\Users\rob\Games\3DO\`:
1. Daedalus Encounter The (USA) (Disc 1).chd
2. Daedalus Encounter The (USA) (Disc 1).chd (MAME copy)
3. daedalus-demo-jp.chd
4. **Road Rash (USA).chd** 🆕
5. Super Street Fighter II Turbo (USA).chd

---

## 🚀 Next Steps

1. **Try the launcher:** `3DOLauncher-Release\3DOLauncher.exe`
2. **Convert a game:** Use chdman to convert CHD to CUE/BIN
3. **Test custom emulator:** Run with converted game
4. **Continue development:** See ThreeDOEmulator/README.md

---

## ⚠️ Known Issues

- **4DO (used by launcher):** Shows INSERT CD screen with CHD files
- **Custom Emulator:** Still in development, needs graphics/audio output
- **MAME:** Sound not implemented
- **FreeDo:** Doesn't launch properly

---

## 📚 Documentation

- **SUMMARY.md** - Complete project overview
- **ThreeDOEmulator/README.md** - Technical details
- **This file** - Quick reference

Good luck! 🎮
