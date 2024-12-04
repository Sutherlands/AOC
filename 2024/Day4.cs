using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2024
{
  public static class Day4
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay4.txt").ToList();
      var xmasCount = 0;

      for (int x = 0; x < lines.Count; ++x)
      {
        for (int y = 0; y < lines.Count; ++y)
        {
          for (int verticalDirection = -1; verticalDirection <= 1; ++verticalDirection)
          {
            for (int horizontalDirection = -1; horizontalDirection <= 1; ++horizontalDirection)
            {
              var targetString = "";
              for (int wordPosition = 0; wordPosition < 4; ++wordPosition)
              {
                var targetY = y + (verticalDirection * wordPosition);
                var targetX = x + (horizontalDirection * wordPosition);

                if (targetY < 0 || targetX < 0 || targetY >= lines.Count || targetX >= lines.Count)
                {
                  break;
                }

                targetString += lines[targetY][targetX];
              }

              if (targetString == "XMAS")
              {
                ++xmasCount;
              }
            }
          }
        }
      }

      Console.WriteLine(xmasCount);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay4.txt").ToList();
      var xmasCount = 0;

      for (int x = 1; x < lines.Count - 1; ++x)
      {
        for (int y = 1; y < lines.Count - 1; ++y)
        {
          if (lines[y][x] != 'A')
          {
            continue;
          }

          var point1X = x - 1;
          var point1Y = y + 1;
          var point2X = x + 1;
          var point2Y = y + 1;
          var point3X = x + 1;
          var point3Y = y - 1;
          var point4X = x - 1;
          var point4Y = y - 1;


          if ((lines[point1Y][point1X] == 'M' && lines[point2Y][point2X] == 'M')
            || (lines[point2Y][point2X] == 'M' && lines[point3Y][point3X] == 'M')
            || (lines[point3Y][point3X] == 'M' && lines[point4Y][point4X] == 'M')
            || (lines[point4Y][point4X] == 'M' && lines[point1Y][point1X] == 'M')
            )
          {
            if ((lines[point1Y][point1X] == 'S' && lines[point2Y][point2X] == 'S')
            || (lines[point2Y][point2X] == 'S' && lines[point3Y][point3X] == 'S')
            || (lines[point3Y][point3X] == 'S' && lines[point4Y][point4X] == 'S')
            || (lines[point4Y][point4X] == 'S' && lines[point1Y][point1X] == 'S')
            )
            {
              xmasCount++;
            }
          }
        }
      }

      Console.WriteLine(xmasCount);
    }

  }
}
