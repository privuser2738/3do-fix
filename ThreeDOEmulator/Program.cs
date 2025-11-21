using System;
using ThreeDOEmulator.Core;

namespace ThreeDOEmulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("  3DO Emulator - Built from Scratch");
            Console.WriteLine("=======================================\n");

            // Create 3DO system
            ThreeDOSystem threeDO = new ThreeDOSystem();

            // Load BIOS
            string biosPath = @"C:\Users\rob\Games\3DO\4DO\bios\panafz10.bin";
            Console.WriteLine($"\nAttempting to load BIOS: {biosPath}");

            if (!threeDO.LoadBIOS(biosPath))
            {
                Console.WriteLine("Failed to load BIOS. Exiting.");
                return;
            }

            // Load game (if specified)
            if (args.Length > 0)
            {
                string gamePath = args[0];
                Console.WriteLine($"\nLoading game: {gamePath}");

                if (!threeDO.LoadGame(gamePath))
                {
                    Console.WriteLine("Failed to load game, but will continue with BIOS only.");
                }
            }
            else
            {
                Console.WriteLine("\nNo game specified. Running BIOS only.");
                Console.WriteLine("Usage: ThreeDOEmulator.exe <path_to_cue_file>");
            }

            // Start emulation
            Console.WriteLine("\n=======================================");
            Console.WriteLine("Starting emulation...");
            Console.WriteLine("Press Ctrl+C to stop");
            Console.WriteLine("=======================================\n");

            try
            {
                threeDO.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nEmulation error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("\n=======================================");
            Console.WriteLine("Emulation completed.");
            Console.WriteLine("=======================================");
        }
    }
}
