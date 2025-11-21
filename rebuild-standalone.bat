@echo off
echo =======================================
echo  Rebuilding 3DO Projects (Standalone)
echo  Includes .NET runtime
echo =======================================
echo.

echo [1/3] Cleaning old builds...
if exist "3DOLauncher-Standalone" rmdir /s /q "3DOLauncher-Standalone"
if exist "ThreeDOEmulator-Standalone" rmdir /s /q "ThreeDOEmulator-Standalone"
echo Done.
echo.

echo [2/3] Building 3DO Launcher (Standalone)...
echo This may take a few minutes...
cd 3DOLauncher
dotnet clean -c Release
dotnet publish -c Release -r win-x64 --self-contained true -o "..\3DOLauncher-Standalone"
if errorlevel 1 (
    echo ERROR: Failed to build 3DO Launcher
    cd ..
    pause
    exit /b 1
)
cd ..
echo Done.
echo.

echo [3/3] Building 3DO Emulator (Standalone)...
echo This may take a few minutes...
cd ThreeDOEmulator
dotnet clean -c Release
dotnet publish -c Release -r win-x64 --self-contained true -o "..\ThreeDOEmulator-Standalone"
if errorlevel 1 (
    echo ERROR: Failed to build 3DO Emulator
    cd ..
    pause
    exit /b 1
)
cd ..
echo Done.
echo.

echo =======================================
echo  Build Complete!
echo =======================================
echo.
echo Standalone executables (no .NET required):
echo   Launcher:  3DOLauncher-Standalone\3DOLauncher.exe
echo   Emulator:  ThreeDOEmulator-Standalone\ThreeDOEmulator.exe
echo.
echo These can run on any Windows PC without .NET installed.
echo.
pause
