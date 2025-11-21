using System;

namespace ThreeDOEmulator.Core
{
    /// <summary>
    /// Main 3DO system emulator - coordinates all components
    /// </summary>
    public class ThreeDOSystem
    {
        public CPU.ARM60 CPU { get; private set; }
        public Memory.MemoryBus Memory { get; private set; }
        public IO.CDROMController CDROM { get; private set; }
        public Graphics.CELEngine Graphics { get; private set; }
        public Audio.DSP Audio { get; private set; }

        private bool _running;
        private long _cycleCount;

        public ThreeDOSystem()
        {
            Console.WriteLine("Initializing 3DO System...");

            // Initialize memory (2MB RAM + ROM space)
            Memory = new Memory.MemoryBus();

            // Initialize ARM60 CPU @ 12.5MHz
            CPU = new CPU.ARM60(Memory);

            // Initialize CD-ROM controller
            CDROM = new IO.CDROMController(Memory);

            // Initialize graphics (CEL engine)
            Graphics = new Graphics.CELEngine(Memory);

            // Initialize audio (DSP)
            Audio = new Audio.DSP(Memory);

            Console.WriteLine("3DO System initialized successfully!");
        }

        public bool LoadBIOS(string biosPath)
        {
            Console.WriteLine($"Loading BIOS from: {biosPath}");

            try
            {
                byte[] biosData = System.IO.File.ReadAllBytes(biosPath);
                Console.WriteLine($"BIOS size: {biosData.Length} bytes");

                // Load BIOS into ROM space (starts at 0x03000000)
                Memory.LoadROM(biosData, 0x03000000);

                // Set CPU to start at BIOS entry point
                CPU.Reset(0x03000000);

                Console.WriteLine("BIOS loaded successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load BIOS: {ex.Message}");
                return false;
            }
        }

        public bool LoadGame(string gamePath)
        {
            Console.WriteLine($"Loading game: {gamePath}");
            return CDROM.LoadDisc(gamePath);
        }

        public void Run()
        {
            Console.WriteLine("Starting emulation...");
            _running = true;
            _cycleCount = 0;

            while (_running)
            {
                // Execute one CPU instruction
                int cycles = CPU.Step();
                _cycleCount += cycles;

                // Update other components based on cycles
                CDROM.Update(cycles);
                Graphics.Update(cycles);
                Audio.Update(cycles);

                // Handle timing (aim for 12.5MHz)
                if (_cycleCount >= 12500000)
                {
                    // One second has passed
                    _cycleCount = 0;
                    Console.WriteLine($"Emulation running... Frame rendered");
                }

                // Temporary: stop after 1000 cycles for testing
                if (_cycleCount > 1000)
                {
                    Console.WriteLine("Test run complete (1000 cycles)");
                    break;
                }
            }
        }

        public void Stop()
        {
            _running = false;
            Console.WriteLine("Emulation stopped.");
        }
    }
}
