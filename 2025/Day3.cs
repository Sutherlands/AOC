using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2025
{
    public static class Day3
    {
        public static void RunPart1()
        {
            var lines = File.ReadAllLines("./PuzzleInputDay3.txt").ToList();
            var maxJoltage = 0;
            foreach(var line in lines)
            {
                var highestFirstNumber = line.Take(line.Length-1).Max();
                var highestSecondNumber = line.SkipWhile(c => c != highestFirstNumber).Skip(1).Max();
                var number = int.Parse(highestFirstNumber.ToString() + highestSecondNumber);
                maxJoltage += number;
            }

            Console.WriteLine(maxJoltage);
        }

        public static void RunPart2()
        {
            var lines = File.ReadAllLines("./PuzzleInputDay3.txt").ToList();
            long maxJoltage = 0;
            foreach (var line in lines)
            {
                var remainingString = line;
                var numberString = "";

                for (int x = 0; x < 12; ++x)
                {
                    (var highestNumber, remainingString) = GetHighestNumber(remainingString, 11-x);
                    numberString += highestNumber;
                }
                
                var number = long.Parse(numberString);
                maxJoltage += number;
            }

            Console.WriteLine(maxJoltage);
        }

        public static (char, string) GetHighestNumber(string line, int spacesAtEndToLeave)
        {
            var highestNumber = line.Take(line.Length - spacesAtEndToLeave).Max();
            var remainingString = string.Join("", line.SkipWhile(c => c != highestNumber).Skip(1));
            return (highestNumber, remainingString);
        }
    }
}
