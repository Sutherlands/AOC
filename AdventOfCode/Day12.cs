using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day12
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay12.txt").ToList();
      var caves = new Dictionary<string, Cave>();

      foreach(var line in lines)
      {
        var parts = line.Split('-');
        var cave1 = GetCave(caves, parts[0]);
        var cave2 = GetCave(caves, parts[1]);

        cave1.ConnectedCaves.Add(cave2);
        cave2.ConnectedCaves.Add(cave1);
      }

      var startCave = GetCave(caves, "start");
      int paths = GetPaths(startCave, new Stack<Cave>(), new List<Cave>(), false);
      Console.WriteLine(paths);
    }

    private static int GetPaths(Cave currentCave, Stack<Cave> visitedCaves, IEnumerable<Cave> cavesThatCantBeDoubled, bool canVisitSmallCaveTwice = false)
    {
      if(currentCave.Name == "end")
      {
        //Console.WriteLine(string.Join(',', visitedCaves.Select(c => c.Name)));
        return 1;
      }

      var paths = 0;
      currentCave.TimesVisited++;
      foreach(var nextCave in currentCave.ConnectedCaves)
      {
        if(nextCave.IsBig || nextCave.TimesVisited == 0)
        {
          visitedCaves.Push(currentCave); 
          paths += GetPaths(nextCave, visitedCaves, cavesThatCantBeDoubled, canVisitSmallCaveTwice);
          visitedCaves.Pop();
        }
        else if(canVisitSmallCaveTwice && nextCave.TimesVisited == 1)
        {
          visitedCaves.Push(currentCave);
          paths += GetPaths(nextCave, visitedCaves,cavesThatCantBeDoubled, false);
          visitedCaves.Pop();
        }
      }
      currentCave.TimesVisited--;
     
      return paths;
    }

    private static Cave GetCave(Dictionary<string, Cave> caves, string name)
    {
      if(!caves.ContainsKey(name))
      {
        caves[name] = new Cave(name);
      }

      return caves[name];
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay12.txt").ToList();
      var sw = Stopwatch.StartNew();
      var caves = new Dictionary<string, Cave>();

      foreach (var line in lines)
      {
        var parts = line.Split('-');
        var cave1 = GetCave(caves, parts[0]);
        var cave2 = GetCave(caves, parts[1]);

        cave1.ConnectedCaves.Add(cave2);
        cave2.ConnectedCaves.Add(cave1);
      }

      var startCave = GetCave(caves, "start");
      var endCave = GetCave(caves, "end");
      int paths = GetPaths(startCave, new Stack<Cave>(), new List<Cave> { startCave, endCave }, true);
      Console.WriteLine(sw.ElapsedMilliseconds);
      Console.WriteLine(paths);
    }

    public class Cave
    {
      public string Name { get; set; } 
      public bool IsBig { get; set; }
      public int TimesVisited { get; set; }

      public List<Cave> ConnectedCaves { get; set; } = new List<Cave>();

      public Cave(string name)
      {
        Name = name;
        IsBig = Name.ToUpper() == Name;
      }
    }
  }
}
