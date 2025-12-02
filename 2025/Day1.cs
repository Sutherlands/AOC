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
    public static class Day1
    {
        public static void RunPart1()
        {
            var lines = File.ReadAllLines("./PuzzleInputDay1.txt").ToList();

            var position = 50;
            var timesOnZero = 0;

            foreach (var line in lines)
            {
                var isLeft = line[0] == 'L';
                var distance = int.Parse(new string(line.Skip(1).ToArray()));

                var newPosition = position + (isLeft ? -distance : distance);
                position = newPosition % 100;
                if (position == 0)
                    timesOnZero++;
            }

            Console.WriteLine(timesOnZero);
        }

        public static void RunPart2()
        {
            var lines = File.ReadAllLines("./PuzzleInputDay1.txt").ToList();

            var position = 50;
            var timesOnZero = 0;

            foreach (var line in lines)
            {
                var isLeft = line[0] == 'L';
                var distance = int.Parse(new string(line.Skip(1).ToArray()));

                timesOnZero += distance / 100;
                distance = distance % 100;

                var oldPosition = position;
                var newPosition = position + (isLeft ? -distance : distance);
                position = (newPosition + 100) % 100;
                if ((position != newPosition || position == 0) && oldPosition != 0)
                    timesOnZero++;
            }

            Console.WriteLine(timesOnZero);
        }
    }
}
