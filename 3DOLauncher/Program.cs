using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

class ThreeDOLauncher
{
    static string GamePath = @"C:\Users\rob\Games\3DO";
    static string EmulatorPath = @"C:\Users\rob\Games\3DO\4DO\4DO.exe";
    static string BiosPath = @"C:\Users\rob\Games\3DO\4DO\bios";

    static void Main(string[] args)
    {
        Console.WriteLine("=== 3DO Game Launcher ===\n");

        // Find all CHD files recursively
        var games = Directory.GetFiles(GamePath, "*.chd", SearchOption.AllDirectories)
            .OrderBy(f => Path.GetFileName(f))
            .ToList();

        if (games.Count == 0)
        {
            Console.WriteLine("No .chd game files found in " + GamePath);
            return;
        }

        // Handle command-line argument for direct launch
        if (args.Length > 0 && int.TryParse(args[0], out int cmdLineSelection))
        {
            if (cmdLineSelection >= 1 && cmdLineSelection <= games.Count)
            {
                Console.WriteLine($"Launching game #{cmdLineSelection}: {Path.GetFileName(games[cmdLineSelection - 1])}");
                LaunchGame(games[cmdLineSelection - 1]);
                return;
            }
        }

        // Display games
        Console.WriteLine("Available Games:");
        for (int i = 0; i < games.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(games[i])}");
            Console.WriteLine($"    Location: {Path.GetDirectoryName(games[i])}");
        }

        Console.Write("\nSelect game (1-" + games.Count + ") or 'q' to quit: ");
        var input = Console.ReadLine();

        if (input?.ToLower() == "q") return;

        if (int.TryParse(input, out int selection) && selection >= 1 && selection <= games.Count)
        {
            string selectedGame = games[selection - 1];
            Console.WriteLine($"\nLaunching: {Path.GetFileName(selectedGame)}");
            Console.WriteLine($"Using emulator: {EmulatorPath}");
            Console.WriteLine($"BIOS path: {BiosPath}");

            LaunchGame(selectedGame);
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }

    static void LaunchGame(string gamePath)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = EmulatorPath,
                Arguments = $"-StartLoadFile \"{gamePath}\"",
                UseShellExecute = true
            };

            Console.WriteLine("\nStarting emulator...");
            Console.WriteLine($"Command: {psi.FileName} {psi.Arguments}\n");

            var process = Process.Start(psi);

            if (process != null)
            {
                Console.WriteLine("Emulator launched successfully!");
                System.Threading.Thread.Sleep(2000); // Give it 2 seconds to start
            }
            else
            {
                Console.WriteLine("Failed to start emulator.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error launching game: {ex.Message}");
        }
    }
}
