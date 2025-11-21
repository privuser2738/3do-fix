@echo off
echo =======================================
echo  Rebuilding ALL 3DO Projects
echo =======================================
echo.

echo [1/5] Force cleaning ALL old builds...
if exist "3DOLauncher-Release" rmdir /s /q "3DOLauncher-Release" 2>nul
if exist "ThreeDOEmulator-Release" rmdir /s /q "ThreeDOEmulator-Release" 2>nul
if exist "CDImageConverter-Release" rmdir /s /q "CDImageConverter-Release" 2>nul
if exist "3DOLauncher\bin" rmdir /s /q "3DOLauncher\bin" 2>nul
if exist "3DOLauncher\obj" rmdir /s /q "3DOLauncher\obj" 2>nul
if exist "ThreeDOEmulator\bin" rmdir /s /q "ThreeDOEmulator\bin" 2>nul
if exist "ThreeDOEmulator\obj" rmdir /s /q "ThreeDOEmulator\obj" 2>nul
if exist "CDImageConverter\bin" rmdir /s /q "CDImageConverter\bin" 2>nul
if exist "CDImageConverter\obj" rmdir /s /q "CDImageConverter\obj" 2>nul
echo Done cleaning.
echo.

echo [2/5] Building 3DO Launcher...
cd 3DOLauncher
dotnet build -c Release --no-incremental --force >nul 2>&1
if errorlevel 1 (
    echo ERROR: Failed to build 3DO Launcher
    cd ..
    pause
    exit /b 1
)
dotnet publish -c Release -r win-x64 --self-contained false --force -o "..\3DOLauncher-Release" >nul 2>&1
cd ..
echo Done.
echo.

echo [3/5] Building 3DO Emulator...
cd ThreeDOEmulator
dotnet build -c Release --no-incremental --force >nul 2>&1
if errorlevel 1 (
    echo ERROR: Failed to build 3DO Emulator
    cd ..
    pause
    exit /b 1
)
dotnet publish -c Release -r win-x64 --self-contained false --force -o "..\ThreeDOEmulator-Release" >nul 2>&1
cd ..
echo Done.
echo.

echo [4/5] Building CD Image Converter (GUI)...
cd CDImageConverter
dotnet build -c Release --no-incremental --force >nul 2>&1
if errorlevel 1 (
    echo ERROR: Failed to build CD Image Converter
    cd ..
    pause
    exit /b 1
)
dotnet publish -c Release -r win-x64 --self-contained false --force -o "..\CDImageConverter-Release" >nul 2>&1
cd ..
echo Done.
echo.

echo [5/5] Verifying builds...
echo.
if exist "3DOLauncher-Release\3DOLauncher.exe" (
    echo [OK] 3DOLauncher.exe
) else (
    echo [ERROR] 3DOLauncher.exe not found!
)

if exist "ThreeDOEmulator-Release\ThreeDOEmulator.exe" (
    echo [OK] ThreeDOEmulator.exe
) else (
    echo [ERROR] ThreeDOEmulator.exe not found!
)

if exist "CDImageConverter-Release\CDImageConverter.exe" (
    echo [OK] CDImageConverter.exe
) else (
    echo [ERROR] CDImageConverter.exe not found!
)
echo.

echo =======================================
echo  Build Complete!
echo =======================================
echo.
echo Executables:
echo   3DOLauncher-Release\3DOLauncher.exe
echo   ThreeDOEmulator-Release\ThreeDOEmulator.exe
echo   CDImageConverter-Release\CDImageConverter.exe  [NEW!]
echo.
echo To convert CHD/ISO files, run:
echo   CDImageConverter-Release\CDImageConverter.exe
echo.
pause
