using System;

namespace ThreeDOEmulator.Memory
{
    /// <summary>
    /// 3DO Memory Bus
    /// 0x00000000-0x001FFFFF: 2MB System RAM
    /// 0x03000000-0x030FFFFF: 1MB BIOS ROM
    /// 0x03100000-0x031FFFFF: Expansion
    /// 0x03200000-0x032FFFFF: MADAM (Graphics)
    /// 0x03300000-0x033FFFFF: CLIO (Audio/IO)
    /// 0x03400000-0x034FFFFF: CD-ROM
    /// </summary>
    public class MemoryBus
    {
        private byte[] _ram;          // 2MB RAM
        private byte[] _rom;          // 1MB ROM (BIOS)
        private byte[] _vram;         // Video RAM

        public MemoryBus()
        {
            _ram = new byte[2 * 1024 * 1024];  // 2MB
            _rom = new byte[1 * 1024 * 1024];  // 1MB
            _vram = new byte[2 * 1024 * 1024]; // 2MB VRAM

            Console.WriteLine("Memory initialized: 2MB RAM, 1MB ROM, 2MB VRAM");
        }

        public void LoadROM(byte[] data, uint address)
        {
            if (address >= 0x03000000 && address < 0x03100000)
            {
                int offset = (int)(address - 0x03000000);
                int length = Math.Min(data.Length, _rom.Length - offset);
                Array.Copy(data, 0, _rom, offset, length);
                Console.WriteLine($"Loaded {length} bytes to ROM at 0x{address:X8}");
            }
        }

        public byte ReadByte(uint address)
        {
            // RAM: 0x00000000-0x001FFFFF
            if (address < 0x00200000)
            {
                return _ram[address];
            }
            // ROM: 0x03000000-0x030FFFFF
            else if (address >= 0x03000000 && address < 0x03100000)
            {
                return _rom[address - 0x03000000];
            }
            // VRAM: 0x02000000-0x021FFFFF (example mapping)
            else if (address >= 0x02000000 && address < 0x02200000)
            {
                return _vram[address - 0x02000000];
            }
            // Hardware registers
            else if (address >= 0x03200000)
            {
                return ReadHardwareRegister(address);
            }

            // Unmapped memory
            return 0;
        }

        public void WriteByte(uint address, byte value)
        {
            // RAM: 0x00000000-0x001FFFFF
            if (address < 0x00200000)
            {
                _ram[address] = value;
            }
            // VRAM: 0x02000000-0x021FFFFF
            else if (address >= 0x02000000 && address < 0x02200000)
            {
                _vram[address - 0x02000000] = value;
            }
            // Hardware registers
            else if (address >= 0x03200000)
            {
                WriteHardwareRegister(address, value);
            }
        }

        public uint ReadWord(uint address)
        {
            return (uint)(ReadByte(address) |
                         (ReadByte(address + 1) << 8) |
                         (ReadByte(address + 2) << 16) |
                         (ReadByte(address + 3) << 24));
        }

        public void WriteWord(uint address, uint value)
        {
            WriteByte(address, (byte)(value & 0xFF));
            WriteByte(address + 1, (byte)((value >> 8) & 0xFF));
            WriteByte(address + 2, (byte)((value >> 16) & 0xFF));
            WriteByte(address + 3, (byte)((value >> 24) & 0xFF));
        }

        private byte ReadHardwareRegister(uint address)
        {
            // MADAM (Graphics): 0x03200000-0x032FFFFF
            if (address >= 0x03200000 && address < 0x03300000)
            {
                return 0; // TODO: Graphics registers
            }
            // CLIO (Audio/IO): 0x03300000-0x033FFFFF
            else if (address >= 0x03300000 && address < 0x03400000)
            {
                return 0; // TODO: Audio/IO registers
            }
            // CD-ROM: 0x03400000-0x034FFFFF
            else if (address >= 0x03400000 && address < 0x03500000)
            {
                return 0; // TODO: CD-ROM registers
            }

            return 0;
        }

        private void WriteHardwareRegister(uint address, byte value)
        {
            // MADAM (Graphics): 0x03200000-0x032FFFFF
            if (address >= 0x03200000 && address < 0x03300000)
            {
                // TODO: Graphics registers
            }
            // CLIO (Audio/IO): 0x03300000-0x033FFFFF
            else if (address >= 0x03300000 && address < 0x03400000)
            {
                // TODO: Audio/IO registers
            }
            // CD-ROM: 0x03400000-0x034FFFFF
            else if (address >= 0x03400000 && address < 0x03500000)
            {
                // TODO: CD-ROM registers
            }
        }
    }
}
