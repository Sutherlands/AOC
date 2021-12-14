using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day14
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay14.txt").ToList();
      var polymer = lines[0];

      var rulesTuple = lines.Skip(2).Select(l => new Tuple<char, char, char>(l[0], l[1], l[6])).ToList();
      var rulesDictionary = rulesTuple.GroupBy(t => t.Item1).ToDictionary(k => k.Key, v => v.ToDictionary(innerKey => innerKey.Item2, innerValue => innerValue.Item3));

      for (int x = 0; x < 10; ++x)
      {
        polymer = ApplyPolymerTemplate(polymer, rulesDictionary);
      }

      var count = polymer.GroupBy(c => c).Select(g => new Tuple<char, int>(g.Key, g.Count()));
      var orderedCount = count.OrderByDescending(c => c.Item2).ToList();

      Console.WriteLine(orderedCount[0].Item2 - orderedCount.Last().Item2);
    }

    public static void RunPart2()
    {
      const int totalIterations = 40;

      var lines = File.ReadAllLines("./PuzzleInputDay14.txt").ToList();
      var polymer = lines[0];

      var rulesTuple = lines.Skip(2).Select(l => new Tuple<char, char, char>(l[0], l[1], l[6])).ToList();
      var rulesDictionary = rulesTuple.GroupBy(t => t.Item1).ToDictionary(k => k.Key, v => v.ToDictionary(innerKey => innerKey.Item2, innerValue => innerValue.Item3));

      //iteration, pair, count
      var polymerCountCache = new Dictionary<int, Dictionary<string, Count>>();
      var totalCount = new Count();

      for (int index = 0; index < polymer.Length - 1; ++index)
      {
        totalCount = Count.Combine(totalCount, GetPolymerCount(totalIterations, string.Concat(polymer[index], polymer[index + 1]), polymerCountCache, rulesDictionary), polymer[index]);
        if(index == 0)
        {
          totalCount.CountDictionary[polymer[index]]++;
        }
      }

      //foreach (var key in totalCount.CountDictionary.Keys)
      //{
      //  Console.WriteLine($"{key}, {totalCount.CountDictionary[key]}");
      //}
      var count = totalCount.CountDictionary.Values.Max() - totalCount.CountDictionary.Values.Min();

      Console.WriteLine(count);
    }

    private static Count GetPolymerCount(int iteration, string polymerPair, Dictionary<int, Dictionary<string, Count>> polymerCountCache, Dictionary<char, Dictionary<char, char>> rules)
    {
      Dictionary<string, Count> iterationCache;
      if (!polymerCountCache.TryGetValue(iteration, out iterationCache))
      {
        iterationCache = new Dictionary<string, Count>();
        polymerCountCache[iteration] = iterationCache;
      }

      Count polymerCount;
      if (!iterationCache.TryGetValue(polymerPair, out polymerCount))
      {
        if (iteration == 0)
        {
          polymerCount = new Count();
          polymerCount.CountDictionary[polymerPair[0]] = 1;
          polymerCount.CountDictionary[polymerPair[1]] = polymerPair[0] == polymerPair[1] ? 2 : 1;
        }
        else
        {

          var newElement = rules[polymerPair[0]][polymerPair[1]];
          var firstPair = string.Concat(polymerPair[0], newElement);
          var secondPair = string.Concat(newElement, polymerPair[1]);
          polymerCount = Count.Combine(GetPolymerCount(iteration - 1, firstPair, polymerCountCache, rules), GetPolymerCount(iteration - 1, secondPair, polymerCountCache, rules), newElement);
        }

        iterationCache[polymerPair] = polymerCount;
      }

      return polymerCount;
    }

    public class Count
    {
      public Dictionary<char, long> CountDictionary { get; set; } = new Dictionary<char, long>();

      public static Count Combine(Count lhs, Count rhs, char sharedLetter)
      {
        var newKeys = lhs.CountDictionary.Keys.Concat(rhs.CountDictionary.Keys).Distinct();

        var newDictionary = newKeys.ToDictionary(k => k, v =>
        {
          lhs.CountDictionary.TryGetValue(v, out var lhsValue);
          rhs.CountDictionary.TryGetValue(v, out var rhsValue);
          return lhsValue + rhsValue;
        });

        newDictionary[sharedLetter]--;
        return new Count { CountDictionary = newDictionary };
      }
    }

    private static string ApplyPolymerTemplate(string polymer, Dictionary<char, Dictionary<char, char>> rules)
    {
      var sb = new StringBuilder();
      var first = polymer[0];
      var second = polymer[0];

      foreach (var p in polymer.Skip(1))
      {
        first = second;
        second = p;
        sb.Append(first).Append(rules[first][second]);
      }
      sb.Append(second);
      return sb.ToString();
    }
  }
}
