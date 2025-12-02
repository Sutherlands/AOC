using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2024
{
    public static class Day17
    {
        private static int RegisterA { get; set; }
        private static int RegisterB { get; set; }
        private static int RegisterC { get; set; }

        public static void RunPart1()
        {
            var lines = File.ReadAllLines("./PuzzleInputDay17.txt").ToList();

            RegisterA = int.Parse(lines[0].Split(": ")[1]);
            RegisterB = int.Parse(lines[1].Split(": ")[1]);
            RegisterC = int.Parse(lines[2].Split(": ")[1]);

            var program = lines[4].Split(": ")[1].Split(',').Select(int.Parse).ToList();

            var instructionIndex = 0;
            var outputs = new List<int>();

            while (instructionIndex < program.Count)
            {
                var instruction = program[instructionIndex];
                var operand = program[instructionIndex + 1];
                var shouldJump = true;

                switch (instruction)
                {
                    case 0: //adv
                        RegisterA = RegisterA / (int)Math.Pow(2, GetComboOperandValue(operand));
                        break;
                    case 1:
                        RegisterB = RegisterB ^ operand;
                        break;
                    case 2:
                        RegisterB = GetComboOperandValue(operand) % 8;
                        break;
                    case 3:
                        if (RegisterA != 0)
                        {
                            instructionIndex = operand;
                            shouldJump = false;
                        }
                        break;
                    case 4:
                        RegisterB = RegisterB ^ RegisterC;
                        break;
                    case 5:
                        outputs.Add(GetComboOperandValue(operand) % 8);
                        break;
                    case 6:
                        RegisterB = RegisterA / (int)Math.Pow(2, GetComboOperandValue(operand));
                        break;
                    case 7:
                        RegisterC = RegisterA / (int)Math.Pow(2, GetComboOperandValue(operand));
                        break;

                }

                if (shouldJump)
                {
                    instructionIndex += 2;
                }
            }

            Console.WriteLine(string.Join(",", outputs));
        }

        private static int GetComboOperandValue(int operand)
        {
            switch (operand)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return operand;
                case 4:
                    return RegisterA;
                case 5:
                    return RegisterB;
                case 6:
                    return RegisterC;
                default:
                    throw new Exception();
            }
        }

        public static void RunPart2()
        {
        }
    }
}
