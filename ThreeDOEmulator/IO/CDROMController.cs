using System;
using System.IO;
using ThreeDOEmulator.Memory;

namespace ThreeDOEmulator.IO
{
    /// <summary>
    /// 3DO CD-ROM Controller
    /// Handles loading and reading CHD/CUE/BIN disc images
    /// </summary>
    public class CDROMController
    {
        private MemoryBus _memory;
        private byte[] _discData;
        private bool _discLoaded;
        private int _currentSector;

        public CDROMController(MemoryBus memory)
        {
            _memory = memory;
            _discLoaded = false;
            Console.WriteLine("CD-ROM controller initialized");
        }

        public bool LoadDisc(string path)
        {
            try
            {
                string ext = Path.GetExtension(path).ToLower();

                if (ext == ".chd")
                {
                    Console.WriteLine("CHD format detected - would need to extract or use chdman");
                    Console.WriteLine("For now, please convert CHD to CUE/BIN using chdman");
                    return false;
                }
                else if (ext == ".cue")
                {
                    return LoadCUEFile(path);
                }
                else if (ext == ".bin")
                {
                    return LoadBINFile(path);
                }

                Console.WriteLine($"Unsupported disc format: {ext}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load disc: {ex.Message}");
                return false;
            }
        }

        private bool LoadCUEFile(string cuePath)
        {
            Console.WriteLine($"Loading CUE file: {cuePath}");

            // Read CUE file to find BIN file
            string[] lines = File.ReadAllLines(cuePath);
            string binFile = null;

            foreach (string line in lines)
            {
                if (line.Trim().StartsWith("FILE"))
                {
                    // Extract filename from: FILE "filename.bin" BINARY
                    int start = line.IndexOf('"') + 1;
                    int end = line.LastIndexOf('"');
                    if (start > 0 && end > start)
                    {
                        binFile = line.Substring(start, end - start);
                        break;
                    }
                }
            }

            if (binFile == null)
            {
                Console.WriteLine("No BIN file found in CUE");
                return false;
            }

            // Make path absolute if relative
            string binPath = Path.Combine(Path.GetDirectoryName(cuePath), binFile);
            return LoadBINFile(binPath);
        }

        private bool LoadBINFile(string binPath)
        {
            Console.WriteLine($"Loading BIN file: {binPath}");

            if (!File.Exists(binPath))
            {
                Console.WriteLine($"BIN file not found: {binPath}");
                return false;
            }

            _discData = File.ReadAllBytes(binPath);
            _discLoaded = true;
            _currentSector = 0;

            Console.WriteLine($"Disc loaded successfully: {_discData.Length} bytes ({_discData.Length / 1024 / 1024}MB)");
            return true;
        }

        public byte[] ReadSector(int sectorNumber)
        {
            if (!_discLoaded)
            {
                return new byte[2352]; // Empty sector
            }

            // Each sector is 2352 bytes (MODE1/2352 format)
            int offset = sectorNumber * 2352;

            if (offset + 2352 > _discData.Length)
            {
                return new byte[2352]; // Beyond disc end
            }

            byte[] sector = new byte[2352];
            Array.Copy(_discData, offset, sector, 0, 2352);

            return sector;
        }

        public void Update(int cycles)
        {
            // Handle CD-ROM state updates
            // (Motor spin, seek operations, etc.)
        }

        public bool IsDiscLoaded()
        {
            return _discLoaded;
        }
    }
}
