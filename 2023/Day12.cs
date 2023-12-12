using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2023
{
  public static class Day12
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay12.txt").ToList();
      var parsedLines = lines.Select(l =>
      {
        var parts = l.Split(" ");
        return (parts[0], parts[1].Split(",").Select(int.Parse).ToList());
      });
      var solutions = parsedLines.Select(l => GetSolutions(l.Item1, l.Item2));

      Console.WriteLine(solutions.Sum());
    }


    static Regex replacementRegex = new Regex(@"\?");

    public static int GetSolutions(string record, List<int> groups)
    {
      if(record.Count(r => r == '#') == groups.Sum() || !record.Any(r => r == '?'))
      {
        return CanBeSolution(record, groups) ? 1 : 0;
      }

      var removed = replacementRegex.Replace(record, ".", 1);
      var added = replacementRegex.Replace(record, "#", 1);

      return GetSolutions(removed, groups) + GetSolutions(added, groups);
    }

    public static bool CanBeSolution(string record, List<int> groups)
    {
      int currentGroupIndex = 0;
      int groupSize = 0;
      record = record + ".";

      for (var recordIndex = 0; recordIndex < record.Length; ++recordIndex)
      {
        if(record[recordIndex] == '#')
        {
          groupSize++;
        }
        else
        {
          if(groupSize > 0)
          {
            if(currentGroupIndex > groups.Count || groups[currentGroupIndex] != groupSize)
            {
              return false;
            }
            currentGroupIndex++;
            groupSize = 0;
          }
        }
      }

      return currentGroupIndex == groups.Count;
    }

    static Dictionary<string, Dictionary<string, long>> cache = new Dictionary<string, Dictionary<string, long>>();

    public static void AddCacheValue(string record, List<int> groups, long value)
    {
      if(!cache.ContainsKey(record))
      {
        cache[record] = new Dictionary<string, long>();
      }

      cache[record][string.Join(",", groups)] = value;
    }

    public static long GetSolutionsWithShortcut(string record, List<int> groups)
    {
      if(cache.TryGetValue(record, out var innerCache))
      {
        if(innerCache.TryGetValue(string.Join(",", groups), out var value))
        {
          return value;
        }
      }

      if (!groups.Any())
      {
        var value = record.Any(c => c == '#') ? 0 : 1;
        AddCacheValue(record, groups, value);
        return value;
      }
      else if(!record.Any())
      {
        AddCacheValue(record, groups, 0);
        return 0;
      }

      if(record.Length < (groups.Sum() + groups.Count - 1))
      {
        AddCacheValue(record, groups, 0);
        return 0;
      }

      var groupMax = groups.Max();
      var groupIndex = groups.IndexOf(groupMax);

      var newRecord = Regex.Replace(record, @$"[\.\?]{new string('#', groupMax)}[\.\?]", $".{new string('#', groupMax)}.");
      if(newRecord != record)
      {
        record = newRecord;
      }

      var totalRecords = 0L;
      for(int x = 0; x < record.Length; ++x)
      {
        var targetString = record.Skip(x).Take(groupMax);
        var frontBoundaryChar = x == 0 ? '.' : record[x - 1];
        var backBoundaryChar = x + groupMax + 1 > record.Length ? '.' : record[x + groupMax];
        if (targetString.Count() == groupMax && targetString.All(c => c == '#' || c == '?') && frontBoundaryChar != '#' && backBoundaryChar != '#')
        {
          var firstPartOfRecord = string.Join("", record.Take(x-1));
          var firstPartOfGroups = groups.Take(groupIndex).ToList();

          var secondPartOfRecord = string.Join("", record.Skip(x + groupMax + 1));
          var secondPartOfGroups = groups.Skip(groupIndex + 1).ToList();

          var firstPartTotal = GetSolutionsWithShortcut(firstPartOfRecord, firstPartOfGroups);
          if(firstPartTotal == 0)
          {
            continue;
          }
          var additionalTotal = firstPartTotal * GetSolutionsWithShortcut(secondPartOfRecord, secondPartOfGroups);

          totalRecords += additionalTotal;
        }
      }

      AddCacheValue(record, groups, totalRecords);
      return totalRecords;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay12.txt").ToList();
      var parsedLines = lines.Select(l =>
      {
        var parts = l.Split(" ");
        var record = string.Join("?", new[] { parts[0], parts[0], parts[0], parts[0], parts[0] });
        var singleGroup = parts[1].Split(",").Select(int.Parse).ToList();
        var groups = (new[] { singleGroup, singleGroup, singleGroup, singleGroup, singleGroup }).SelectMany(g => g).ToList();
        return (record, groups);
      }).ToList();

      var total = 0L;
      foreach (var pl in parsedLines)
      {
        var s = GetSolutionsWithShortcut(pl.record, pl.groups);
        Console.WriteLine($"{pl.record} - {string.Join(',', pl.groups)}, {s}");
        total += s;
      }

      Console.WriteLine(total);
    }
  }
}
