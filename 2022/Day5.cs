using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
  public static class Day5

  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay5.txt").ToList();
      var size = (lines.First().Count() / 4) + 1;
      List<Stack<char>> piles = new List<Stack<char>>();
      for (int pos = 0; pos < size; ++pos)
      {
        piles.Add(new Stack<char>());
      }

      foreach (var line in lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Reverse().Skip(1))
      {
        for(int pos = 0; pos < size; ++pos)
        {
          var c = line[pos * 4 + 1];
          if(c != ' ')
          {
            piles[pos].Push(c);
          }
        }
      }

      Regex positionRegex = new Regex(@"move (\d*) from (\d*) to (\d*)");

      foreach (var line in lines.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(1))
      {
        var match = positionRegex.Match(line);
        var numberToMove = int.Parse(match.Groups[1].Value);
        var sourceColumn = int.Parse(match.Groups[2].Value) - 1;
        var destinationColumn = int.Parse(match.Groups[3].Value) - 1;

        var intermediateStack = new Stack<char>();
        for (int x = 0; x < numberToMove; ++x)
        {
          piles[destinationColumn].Push(piles[sourceColumn].Pop());
        }
      }

      
      Console.WriteLine(piles.Select(p => p.Peek()).ToArray());
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay5.txt").ToList();
      var size = (lines.First().Count() / 4) + 1;
      List<Stack<char>> piles = new List<Stack<char>>();
      for (int pos = 0; pos < size; ++pos)
      {
        piles.Add(new Stack<char>());
      }

      foreach (var line in lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Reverse().Skip(1))
      {
        for (int pos = 0; pos < size; ++pos)
        {
          var c = line[pos * 4 + 1];
          if (c != ' ')
          {
            piles[pos].Push(c);
          }
        }
      }

      Regex positionRegex = new Regex(@"move (\d*) from (\d*) to (\d*)");

      foreach (var line in lines.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(1))
      {
        var match = positionRegex.Match(line);
        var numberToMove = int.Parse(match.Groups[1].Value);
        var sourceColumn = int.Parse(match.Groups[2].Value) - 1;
        var destinationColumn = int.Parse(match.Groups[3].Value) - 1;

        var intermediateStack = new Stack<char>();
        for (int x = 0; x < numberToMove; ++x)
        {
          intermediateStack.Push(piles[sourceColumn].Pop());
        }
        for (int x = 0; x < numberToMove; ++x)
        {
          piles[destinationColumn].Push(intermediateStack.Pop());
        }
      }


      Console.WriteLine(piles.Select(p => p.Peek()).ToArray());
    }
  }
}
