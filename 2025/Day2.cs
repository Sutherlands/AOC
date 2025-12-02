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
    public static class Day2
    {
        public static void RunPart1()
        {
            var lines = File.ReadAllLines("./PuzzleInputDay2.txt").ToList();

            var ranges = lines[0].Split(',').ToList();
            long totalOfInvalidNumbers = 0;

            foreach (var range in ranges)
            {
                var parts = range.Split('-');
                var rangeStart = long.Parse(parts[0]);
                var rangeEnd = long.Parse(parts[1]);

                var startDigits = (int)Math.Log10(rangeStart) + 1;
                var endDigits = (int)Math.Log10(rangeEnd) + 1;

                var possibleMultipliers = new List<long>();

                for (int digits = startDigits; digits <= endDigits; digits++)
                {
                    if (digits % 2 == 1)
                    {
                        continue;
                    }

                    possibleMultipliers.Add((long)Math.Pow(10, digits / 2) + 1);
                }

                foreach (var multiplier in possibleMultipliers)
                {
                    var targetDigits = (int)Math.Log10(multiplier) * 2;

                    for (long x = rangeStart / multiplier; x * multiplier <= rangeEnd; x++)
                    {
                        var targetNumber = x * multiplier;
                        if ((int)Math.Log10(targetNumber) + 1 != targetDigits)
                        {
                            continue;
                        }

                        if (targetNumber >= rangeStart && targetNumber <= rangeEnd)
                        {
                            totalOfInvalidNumbers += targetNumber;
                        }
                    }
                }

            }
            Console.WriteLine(totalOfInvalidNumbers);
        }

        public static void RunPart2()
        {
            var lines = File.ReadAllLines("./PuzzleInputDay2.txt").ToList();

            var ranges = lines[0].Split(',').ToList();
            long totalOfInvalidNumbers = 0;

            foreach (var range in ranges)
            {
                var parts = range.Split('-');
                var rangeStart = long.Parse(parts[0]);
                var rangeEnd = long.Parse(parts[1]);

                var startDigits = (int)Math.Log10(rangeStart) + 1;
                var endDigits = (int)Math.Log10(rangeEnd) + 1;

                var possibleMultipliers = new List<long>();

                for (int digits = startDigits; digits <= endDigits; digits++)
                {
                    for (int timesRepeated = 2; timesRepeated <= digits; timesRepeated++)
                    {
                        var lengthOfRepeat = digits / timesRepeated;
                        if (lengthOfRepeat * timesRepeated != digits)
                        {
                            continue;
                        }

                        long multiplier = 1;
                        for (int x = 1; x <= timesRepeated - 1; x++)
                        {
                            multiplier *= (long)Math.Pow(10, lengthOfRepeat);
                            multiplier++;
                        }

                        possibleMultipliers.Add(multiplier);
                    }
                }

                var invalidNumbersInRange = new HashSet<long>();

                foreach (var multiplier in possibleMultipliers)
                {
                    var lengthOfRepeat = multiplier.ToString().Skip(1).TakeWhile(c => c == '0').Count() + 1;
                    var targetDigits = (int)Math.Log10(multiplier) + lengthOfRepeat;

                    for (long x = rangeStart / multiplier; x * multiplier <= rangeEnd; x++)
                    {
                        var targetNumber = x * multiplier;
                        if ((int)Math.Log10(targetNumber) + 1 != targetDigits)
                        {
                            continue;
                        }

                        if (targetNumber >= rangeStart && targetNumber <= rangeEnd)
                        {
                            invalidNumbersInRange.Add(targetNumber);
                        }
                    }
                }

                totalOfInvalidNumbers += invalidNumbersInRange.Sum();

            }
            Console.WriteLine(totalOfInvalidNumbers);
        }
    }
}
