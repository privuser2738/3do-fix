using System;
using ThreeDOEmulator.Memory;

namespace ThreeDOEmulator.CPU
{
    /// <summary>
    /// ARM60 RISC CPU Emulator (32-bit ARM architecture)
    /// Running at 12.5MHz in the 3DO
    /// </summary>
    public class ARM60
    {
        private MemoryBus _memory;

        // ARM registers (R0-R15)
        private uint[] _registers = new uint[16];

        // Special registers
        private uint PC { get => _registers[15]; set => _registers[15] = value; }  // Program Counter
        private uint SP { get => _registers[13]; set => _registers[13] = value; }  // Stack Pointer
        private uint LR { get => _registers[14]; set => _registers[14] = value; }  // Link Register

        // Status flags
        private bool _flagN; // Negative
        private bool _flagZ; // Zero
        private bool _flagC; // Carry
        private bool _flagV; // Overflow

        public ARM60(MemoryBus memory)
        {
            _memory = memory;
            Console.WriteLine("ARM60 CPU initialized @ 12.5MHz");
        }

        public void Reset(uint entryPoint)
        {
            // Clear all registers
            Array.Clear(_registers, 0, _registers.Length);

            // Set PC to entry point (BIOS start)
            PC = entryPoint;

            // Initialize stack pointer (top of RAM)
            SP = 0x001FFFFC;

            Console.WriteLine($"CPU reset, PC=0x{PC:X8}, SP=0x{SP:X8}");
        }

        public int Step()
        {
            // Fetch instruction from PC
            uint instruction = _memory.ReadWord(PC);

            // Decode and execute
            ExecuteInstruction(instruction);

            // Move to next instruction (4 bytes for ARM)
            PC += 4;

            // Return cycle count (simplified - most ARM instructions are 1 cycle)
            return 1;
        }

        private void ExecuteInstruction(uint instruction)
        {
            // Extract instruction type from bits 27-25
            uint type = (instruction >> 25) & 0x7;

            // Check condition (bits 31-28)
            uint condition = (instruction >> 28) & 0xF;
            if (!CheckCondition(condition))
            {
                return; // Instruction not executed due to condition
            }

            switch (type)
            {
                case 0b000: // Data processing
                    ExecuteDataProcessing(instruction);
                    break;

                case 0b001: // Data processing immediate / Move shifted register
                    ExecuteDataProcessing(instruction);
                    break;

                case 0b010: // Load/Store immediate offset
                    ExecuteLoadStore(instruction);
                    break;

                case 0b011: // Load/Store register offset
                    ExecuteLoadStore(instruction);
                    break;

                case 0b100: // Load/Store multiple
                    ExecuteLoadStoreMultiple(instruction);
                    break;

                case 0b101: // Branch
                    ExecuteBranch(instruction);
                    break;

                case 0b110: // Coprocessor load/store
                case 0b111: // Coprocessor operations / Software interrupt
                    // Not commonly used in 3DO
                    break;

                default:
                    Console.WriteLine($"Unknown instruction type: 0b{type:B3} at PC=0x{PC:X8}");
                    break;
            }
        }

        private void ExecuteDataProcessing(uint instruction)
        {
            uint opcode = (instruction >> 21) & 0xF;
            uint rn = (instruction >> 16) & 0xF;
            uint rd = (instruction >> 12) & 0xF;
            bool immediate = ((instruction >> 25) & 1) == 1;

            uint operand2;
            if (immediate)
            {
                // Immediate value
                uint imm = instruction & 0xFF;
                uint rotate = ((instruction >> 8) & 0xF) * 2;
                operand2 = RotateRight(imm, (int)rotate);
            }
            else
            {
                // Register value
                uint rm = instruction & 0xF;
                operand2 = _registers[rm];
            }

            uint op1 = _registers[rn];
            uint result = 0;

            switch (opcode)
            {
                case 0x0: // AND
                    result = op1 & operand2;
                    break;
                case 0x1: // EOR (XOR)
                    result = op1 ^ operand2;
                    break;
                case 0x2: // SUB
                    result = op1 - operand2;
                    break;
                case 0x3: // RSB (Reverse subtract)
                    result = operand2 - op1;
                    break;
                case 0x4: // ADD
                    result = op1 + operand2;
                    break;
                case 0xD: // MOV
                    result = operand2;
                    break;
                case 0xF: // MVN (Move NOT)
                    result = ~operand2;
                    break;
                default:
                    Console.WriteLine($"Unimplemented ALU opcode: 0x{opcode:X}");
                    return;
            }

            // Write result to destination register
            if (rd < 16)
            {
                _registers[rd] = result;
            }

            // Update flags if S bit is set
            if (((instruction >> 20) & 1) == 1)
            {
                _flagZ = (result == 0);
                _flagN = ((result & 0x80000000) != 0);
            }
        }

        private void ExecuteLoadStore(uint instruction)
        {
            bool load = ((instruction >> 20) & 1) == 1;
            uint rn = (instruction >> 16) & 0xF;
            uint rd = (instruction >> 12) & 0xF;
            uint offset = instruction & 0xFFF;

            uint address = _registers[rn] + offset;

            if (load)
            {
                // LDR - Load register from memory
                _registers[rd] = _memory.ReadWord(address);
            }
            else
            {
                // STR - Store register to memory
                _memory.WriteWord(address, _registers[rd]);
            }
        }

        private void ExecuteLoadStoreMultiple(uint instruction)
        {
            // LDM/STM - Load/Store multiple registers
            bool load = ((instruction >> 20) & 1) == 1;
            uint rn = (instruction >> 16) & 0xF;
            uint registerList = instruction & 0xFFFF;

            uint address = _registers[rn];

            for (int i = 0; i < 16; i++)
            {
                if (((registerList >> i) & 1) == 1)
                {
                    if (load)
                    {
                        _registers[i] = _memory.ReadWord(address);
                    }
                    else
                    {
                        _memory.WriteWord(address, _registers[i]);
                    }
                    address += 4;
                }
            }
        }

        private void ExecuteBranch(uint instruction)
        {
            bool link = ((instruction >> 24) & 1) == 1;
            int offset = (int)(instruction & 0xFFFFFF);

            // Sign extend 24-bit offset to 32-bit
            if ((offset & 0x800000) != 0)
            {
                offset |= unchecked((int)0xFF000000);
            }

            // Offset is in words, multiply by 4 for byte address
            offset *= 4;

            if (link)
            {
                // BL - Branch with link (save return address)
                LR = PC + 4;
            }

            // Branch (PC will be incremented by Step(), so subtract 4)
            PC = (uint)((int)PC + offset - 4);
        }

        private bool CheckCondition(uint condition)
        {
            switch (condition)
            {
                case 0x0: return _flagZ;                          // EQ - Equal
                case 0x1: return !_flagZ;                         // NE - Not equal
                case 0xA: return _flagN == _flagV;                // GE - Greater or equal
                case 0xB: return _flagN != _flagV;                // LT - Less than
                case 0xE: return true;                            // AL - Always
                default: return true;
            }
        }

        private uint RotateRight(uint value, int amount)
        {
            amount %= 32;
            return (value >> amount) | (value << (32 - amount));
        }
    }
}
