using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2024
{
  public static class Day9
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay9.txt").ToList();
      var writingBlock = true;
      var blockId = 0;
      List<int?> blockValues = new List<int?>();
      foreach (var number in lines[0].Select(c => int.Parse(c.ToString())))
      {
        var character = writingBlock ? blockId : (int?)null;
        for (int count = 0; count < number; ++count)
        {
          blockValues.Add(character);
        }

        if (writingBlock)
        {
          blockId++;
        }

        writingBlock = !writingBlock;
      }

      int startIndex = 0;
      int endIndex = blockValues.Count - 1;

      while (startIndex < endIndex)
      {
        if (blockValues[startIndex] == null)
        {
          blockValues[startIndex] = blockValues[endIndex];
          blockValues[endIndex] = null;

          while (blockValues[endIndex] == null)
          {
            endIndex--;
          }
        }

        ++startIndex;
      }

      var sum = 0L;
      for (int index = 0; blockValues[index] != null; ++index)
      {
        sum += index * (int)blockValues[index];
      }

      Console.WriteLine(sum);

    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay9.txt").ToList();
      var writingBlock = true;
      var blockId = 0;
      List<int?> blockValues = new List<int?>();

      foreach (var number in lines[0].Select(c => int.Parse(c.ToString())))
      {
        var character = writingBlock ? blockId : (int?)null;
        for (int count = 0; count < number; ++count)
        {
          blockValues.Add(character);
        }

        if (writingBlock)
        {
          blockId++;
        }

        writingBlock = !writingBlock;
      }

      for (int currentBlockId = blockValues.OfType<int>().Max(); currentBlockId >= 0; --currentBlockId)
      {
        var start = blockValues.IndexOf(currentBlockId);
        var end = blockValues.LastIndexOf(currentBlockId);
        var size = end - start + 1;

        var lastEmptyIndex = -1;
        var inEmptyBlock = false;

        for (int currentIndex = 0; currentIndex < start; ++currentIndex)
        {
          if (blockValues[currentIndex] != null)
          {
            inEmptyBlock = false;
          }
          else
          {
            if (!inEmptyBlock)
            {
              lastEmptyIndex = currentIndex;
              inEmptyBlock = true;
            }

            if (currentIndex - lastEmptyIndex + 1 >= size)
            {
              for (int sizeIndex = 0; sizeIndex < size; ++sizeIndex)
              {
                blockValues[lastEmptyIndex + sizeIndex] = currentBlockId;
                blockValues[start + sizeIndex] = null;
              }

              break;
            }
          }
        }
      }

      var sum = 0L;
      for (int index = 0; index < blockValues.Count; ++index)
      {
        sum += index * blockValues[index].GetValueOrDefault();
      }

      Console.WriteLine(sum);
    }
  }
}
