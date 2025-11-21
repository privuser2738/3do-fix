@echo off
echo =======================================
echo  Rebuilding 3DO Projects
echo =======================================
echo.

echo [1/3] Cleaning old builds...
if exist "3DOLauncher-Release" rmdir /s /q "3DOLauncher-Release"
if exist "ThreeDOEmulator\bin\Release" rmdir /s /q "ThreeDOEmulator\bin\Release"
echo Done.
echo.

echo [2/3] Building 3DO Launcher...
cd 3DOLauncher
dotnet clean -c Release
dotnet publish -c Release -r win-x64 --self-contained false -o "..\3DOLauncher-Release"
if errorlevel 1 (
    echo ERROR: Failed to build 3DO Launcher
    cd ..
    pause
    exit /b 1
)
cd ..
echo Done.
echo.

echo [3/3] Building 3DO Emulator...
cd ThreeDOEmulator
dotnet clean -c Release
dotnet build -c Release
dotnet publish -c Release -r win-x64 --self-contained false -o "..\ThreeDOEmulator-Release"
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
echo Launcher:  3DOLauncher-Release\3DOLauncher.exe
echo Emulator:  ThreeDOEmulator-Release\ThreeDOEmulator.exe
echo.
echo You can also find debug builds in:
echo   3DOLauncher\bin\Debug\net8.0\
echo   ThreeDOEmulator\bin\Debug\net8.0\
echo.
pause
