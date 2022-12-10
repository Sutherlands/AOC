using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
  public static class Day8
  {
    public static void RunPart1()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay8.txt").ToList();
      Console.WriteLine(sw.ElapsedMilliseconds);
      var length = lines.First().Length;

      var treeHeights = new int[length][];
      var visibleMap = new bool[length][];
      for(var x = 0; x < length; ++x)
      {
        treeHeights[x] = new int[length];
        visibleMap[x] = new bool[length];

        for(var y = 0; y < length; ++y)
        {
          treeHeights[x][y] = int.Parse(lines[x][y].ToString());
        }
      }

      for (var x = 0; x < length; ++x)
      {
        var currentHeight = -1;
        for (var y = 0; y < length; ++y)
        {
          if (treeHeights[x][y] > currentHeight)
          {
            visibleMap[x][y] = true;
            currentHeight = treeHeights[x][y];
            if(currentHeight == 9)
            {
              break;
            }
          }
        }
      }

      for (var y = 0; y < length; ++y)
      {
        var currentHeight = -1;
        for (var x = 0; x < length; ++x)
        {
          if (treeHeights[x][y] > currentHeight)
          {
            visibleMap[x][y] = true;
            currentHeight = treeHeights[x][y];
            if (currentHeight == 9)
            {
              break;
            }
          }
        }
      }

      for (var x = 0; x < length; ++x)
      {
        var currentHeight = -1;
        for (var y = length - 1; y > 0; --y)
        {
          if (treeHeights[x][y] > currentHeight)
          {
            visibleMap[x][y] = true;
            currentHeight = treeHeights[x][y];
            if (currentHeight == 9)
            {
              break;
            }
          }
        }
      }

      for (var y = 0; y < length; ++y)
      {
        var currentHeight = -1;
        for (var x = length-1; x > 0; --x)
        {
          if (treeHeights[x][y] > currentHeight)
          {
            visibleMap[x][y] = true;
            currentHeight = treeHeights[x][y];
            if (currentHeight == 9)
            {
              break;
            }
          }
        }
      }

      var totalVisible = visibleMap.SelectMany(m => m).Where(i => i).Count();
      Console.WriteLine(sw.ElapsedMilliseconds);
      Console.WriteLine(totalVisible);
    }

    public static void RunPart2()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay8.txt").ToList();
      Console.WriteLine(sw.ElapsedMilliseconds);
      var length = lines.First().Length;

      var treeHeights = new int[length][];
      for (var x = 0; x < length; ++x)
      {
        treeHeights[x] = new int[length];
        for (var y = 0; y < length; ++y)
        {
          treeHeights[x][y] = int.Parse(lines[x][y].ToString());
        }
      }

      var maxScenicScore = 0;
      for(int x = 0; x < length; ++x)
      {
        for(int y = 0; y < length; ++y)
        {
          var score = GetScenicScore(treeHeights, x, y, length);
          maxScenicScore = Math.Max(maxScenicScore, score);
        }
      }

      Console.WriteLine(sw.ElapsedMilliseconds);
      Console.WriteLine(maxScenicScore);
    }

    private static int GetScenicScore(int[][] treeHeights, int x, int y, int length)
    {
      int leftX = x;
      int rightX = x;
      int upY = y;
      int downY = y;

      do
      {
        if (leftX == 0)
        {
          break;
        }
        leftX--;

      } while (treeHeights[leftX][y] < treeHeights[x][y]);

      do
      {
        if (rightX == length-1)
        {
          break;
        }
        rightX++;

      } while (treeHeights[rightX][y] < treeHeights[x][y]);

      do
      {
        if (downY == 0)
        {
          break;
        }
        downY--;

      } while (treeHeights[x][downY] < treeHeights[x][y]);

      do
      {
        if (upY == length - 1)
        {
          break;
        }
        upY++;

      } while (treeHeights[x][upY] < treeHeights[x][y]);

      return (x - leftX) * (rightX - x) * (y - downY) * (upY - y);
    }
  }
}
