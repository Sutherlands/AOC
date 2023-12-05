using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2023
{
  public static class Day2
  {
    public static void RunPart1()
    {
      var bagTotal = new Dictionary<string, int> { { "red", 12 }, { "green", 13 }, { "blue", 14 } };
      var total = 0;
      var lines = File.ReadAllLines("./PuzzleInputDay2.txt").ToList();
      foreach(var line in lines)
      {
        var possible = true;
        var parts = line.Split(": ");
        var id = int.Parse(parts[0].Split(' ')[1]);
        var pulls = parts[1].Split(";");
        foreach(var pull in pulls)
        {
          var sorts = pull.Trim().Split(", ");
          foreach(var sort in sorts)
          {
            var sortSplit = sort.Split(" ");
            var amount = int.Parse(sortSplit[0]);
            var color = sortSplit[1];

            if(!bagTotal.ContainsKey(color) || bagTotal[color] < amount)
            {
              possible = false;
            }
          }
        }
          
        if(possible)
        {
          total += id;
        }
      }
      Console.WriteLine(total);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay2.txt").ToList();
      var total = 0;
      foreach (var line in lines)
      {
        var bagTotal = new Dictionary<string, int> { };
        var parts = line.Split(": ");
        var id = int.Parse(parts[0].Split(' ')[1]);
        var pulls = parts[1].Split(";");
        foreach (var pull in pulls)
        {
          var sorts = pull.Trim().Split(", ");
          foreach (var sort in sorts)
          {
            var sortSplit = sort.Split(" ");
            var amount = int.Parse(sortSplit[0]);
            var color = sortSplit[1];

            bagTotal.TryGetValue(color, out var current);
            bagTotal[color] = Math.Max(current, amount);
          }
        }

        var power = bagTotal.Values.Aggregate(1, (a, b) => a * b);
        total += power;
      }
      Console.WriteLine(total);
    }
  }
}
