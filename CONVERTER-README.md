# CD Image Converter - GUI Tool

## What It Does

Simple Windows GUI application to convert CHD and ISO files to BIN/CUE format for use with the 3DO emulator.

## Features

✅ **Easy to Use** - Just click and select files
✅ **Supports CHD** - Converts MAME CHD format to BIN/CUE
✅ **Supports ISO** - Converts ISO format to BIN/CUE
✅ **Saves Settings** - Remembers output folder and chdman path
✅ **Progress Display** - Shows conversion progress
✅ **Error Handling** - Clear error messages if something goes wrong

## How to Use

### 1. Launch the Converter
```bash
cd C:\users\rob\source\002\3do-fix\CDImageConverter-Release
CDImageConverter.exe
```

### 2. Select Input File
- Click **Browse...** next to the input field
- Select a .CHD or .ISO file
- Example: `C:\Users\rob\Games\3DO\Road Rash (USA).chd`

### 3. Configure Output (Optional)
- Default output: `C:\Users\rob\Games\3DO`
- Click **Change...** to select a different folder
- Settings are saved automatically

### 4. Configure chdman Path (Optional)
- Default: `C:\Users\rob\Games\3DO\MAME\chdman.exe`
- Click **Configure...** if chdman.exe is in a different location
- Settings are saved automatically

### 5. Convert!
- Click the big green **Convert to BIN + CUE** button
- Wait for conversion (shows progress)
- Success message shows output file locations

## Output

For input file: `Road Rash (USA).chd`

**Creates:**
- `Road Rash (USA).cue` - Cue sheet file
- `Road Rash (USA).bin` - Binary disc image (340MB)

## Using Converted Files

### With Custom Emulator
```bash
cd ThreeDOEmulator-Release
ThreeDOEmulator.exe "C:\Users\rob\Games\3DO\Road Rash (USA).cue"
```

### With 4DO
```bash
cd "C:\Users\rob\Games\3DO\4DO"
4DO.exe -StartLoadFile "C:\Users\rob\Games\3DO\Road Rash (USA).cue"
```

## Configuration File

Settings are saved in: `converter.config`

```
C:\Users\rob\Games\3DO
C:\Users\rob\Games\3DO\MAME\chdman.exe
```

Line 1: Output folder
Line 2: chdman.exe path

## Requirements

- Windows OS
- .NET 8.0 Runtime
- chdman.exe (comes with MAME)

## Supported Formats

**Input:**
- `.chd` - MAME Compressed Hunks of Data
- `.iso` - ISO 9660 CD image

**Output:**
- `.cue` - Cue sheet
- `.bin` - Raw binary disc image

## Error Messages

### "chdman.exe not found"
- Click **Configure...** and browse to chdman.exe
- Usually at: `C:\Users\rob\Games\3DO\MAME\chdman.exe`

### "Input file does not exist"
- Check that the selected file path is correct
- File may have been moved or deleted

### "Only .chd and .iso files are supported"
- Select a CHD or ISO file
- Other formats (BIN, CUE, etc.) don't need conversion

### "Output files already exist"
- Click **Yes** to overwrite
- Click **No** to cancel
- Output files from previous conversion detected

## Tips

1. **Batch Conversion** - Convert all your CHD files at once, one by one
2. **Keep Originals** - Don't delete original CHD files after conversion
3. **Storage** - BIN files are large (300-700MB per game)
4. **Organization** - Keep all converted files in one folder

## Technical Details

### CHD Conversion
Uses `chdman.exe extractcd` command:
```
chdman extractcd -i input.chd -o output.cue -ob output.bin
```

### ISO Conversion
1. Copies ISO to BIN (they're the same format)
2. Creates CUE file pointing to the BIN

### CUE File Format
```
FILE "gamename.bin" BINARY
  TRACK 01 MODE1/2352
    INDEX 01 00:00:00
```

## Location

**Executable:**
```
C:\users\rob\source\002\3do-fix\CDImageConverter-Release\CDImageConverter.exe
```

**Source Code:**
```
C:\users\rob\source\002\3do-fix\CDImageConverter\
  ├── Form1.cs              - Main logic
  ├── Form1.Designer.cs     - GUI layout
  └── Program.cs            - Entry point
```

## Rebuilding

To rebuild from source:
```bash
cd C:\users\rob\source\002\3do-fix
rebuild.bat
```

Or manually:
```bash
cd CDImageConverter
dotnet build -c Release
dotnet publish -c Release -r win-x64 --self-contained false -o "../CDImageConverter-Release"
```

## Screenshot (Text Description)

```
┌─────────────────────────────────────────────────────┐
│ CD Image Converter (CHD/ISO)                        │
├─────────────────────────────────────────────────────┤
│                                                     │
│ [Input File Path...................................] [Browse...]
│                                                     │
│ Output: C:\Users\rob\Games\3DO         [Change...]  │
│ chdman: C:\Users\rob\Games\3DO\MAME\chdman.exe [Configure...]
│                                                     │
│        ┌────────────────────────────┐              │
│        │  Convert to BIN + CUE      │              │
│        └────────────────────────────┘              │
│                                                     │
│ [====Progress Bar============================]      │
│                                                     │
│ ┌─────────────────────────────────────────────┐   │
│ │ Ready to convert CD images.                 │   │
│ │                                             │   │
│ │ [Conversion log appears here...]           │   │
│ │                                             │   │
│ └─────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
```

## Next Steps

After converting your games:
1. Delete old CHD files (optional, save space)
2. Use converted CUE/BIN with custom emulator
3. Test games to verify conversion worked
4. Enjoy your 3DO games!
