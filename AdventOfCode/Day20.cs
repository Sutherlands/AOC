using DotNetty.Common.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode
{
  public static class Day20
  {
    public static void RunPart1()
    {
      var sw = Stopwatch.StartNew();
      var conversionArray = new bool[512];
      bool[,] imageArray;
      var lines = File.ReadAllLines("./PuzzleInputDay20.txt").ToList();
      for (int x = 0; x < 512; ++x)
      {
        conversionArray[x] = lines[0][x] == '#';
      }

      var sizeX = lines[2].Length;
      var sizeY = lines.Count - 2;

      imageArray = new bool[sizeX+2, sizeY+2];
      for (int x = 0; x < sizeX; ++x)
      {
        for (int y = 0; y < sizeY; ++y)
        {
          imageArray[x+1, y+1] = lines[y + 2][x] == '#';
        }
      }

      for (int x = 0; x < 50; ++x)
      {
        imageArray = Enhance(imageArray, conversionArray);
      }

      int totalLit = 0;
      for (int x = 0; x < imageArray.GetLength(0); ++x)
      {
        for (int y = 0; y < imageArray.GetLength(1); ++y)
        {
          if (imageArray[x, y])
          {
            ++totalLit;
          }
        }
      }

      Console.WriteLine(sw.ElapsedMilliseconds);
      Console.WriteLine(totalLit);
    }

    private static bool[,] Enhance(bool[,] imageArray, bool[] conversion)
    {
      var sizeX = imageArray.GetLength(0);
      var sizeY = imageArray.GetLength(1);

      var newArray = new bool[sizeX + 2, sizeY + 2];

      for (int x = 0; x < sizeX + 2; ++x)
      {
        for (int y = 0; y < sizeY + 2; ++y)
        {
          var index = GetIndex(x - 1, y - 1, imageArray, sizeX, sizeY);
          var bit = conversion[index];
          newArray[x, y] = bit;
        }
      }

      return newArray;
    }

    public static int GetIndex(int x, int y, bool[,] imageArray, int maxX, int maxY)
    {
      int value = GetSingleValue(x - 1, y - 1, imageArray, maxX, maxY) ? 1 : 0;
      value = value << 1;
      value += GetSingleValue(x, y - 1, imageArray, maxX, maxY) ? 1 : 0;
      value = value << 1;
      value += GetSingleValue(x + 1, y - 1, imageArray, maxX, maxY) ? 1 : 0;
      value = value << 1;
      value += GetSingleValue(x - 1, y, imageArray, maxX, maxY) ? 1 : 0;
      value = value << 1;
      value += GetSingleValue(x, y, imageArray, maxX, maxY) ? 1 : 0;
      value = value << 1;
      value += GetSingleValue(x + 1, y, imageArray, maxX, maxY) ? 1 : 0;
      value = value << 1;
      value += GetSingleValue(x - 1, y + 1, imageArray, maxX, maxY) ? 1 : 0;
      value = value << 1;
      value += GetSingleValue(x, y + 1, imageArray, maxX, maxY) ? 1 : 0;
      value = value << 1;
      value += GetSingleValue(x + 1, y + 1, imageArray, maxX, maxY) ? 1 : 0;
      return value;
    }

    public static bool GetSingleValue(int x, int y, bool[,] imageArray, int maxX, int maxY)
    {
      x = Math.Max(0, Math.Min(x, maxX - 1));
      y = Math.Max(0, Math.Min(y, maxY - 1));

      return imageArray[x, y];
    }

    public static void RunPart2()
    {
    }
  }
}
