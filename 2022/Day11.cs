using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
  public static class Day11
  {
    public static void RunPart1()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay11.txt").ToList();
      var monkeys = new List<Monkey>();

      var currentIndex = 0;
      while (currentIndex < lines.Count)
      {
        var monkeyNumber = int.Parse(lines[currentIndex].Split(" :".ToCharArray())[1]);
        var items = lines[currentIndex + 1].Split(": ")[1].Split(", ").Select(int.Parse).Select(i => new Item { WorryLevel = i });
        if(!lines[currentIndex + 2].StartsWith("  Operation: new = old "))
        {
          throw new Exception();
        }
        var operationSplit = string.Join("", lines[currentIndex + 2].Skip(23)).Split(' ');
        if(operationSplit.Length != 2)
        {
          throw new Exception();
        }

        Action<Item> update;
        int? updateValue = null;
        switch (operationSplit[1])
        {
          case "old":
            break;
          default:
            updateValue = int.Parse(operationSplit[1]);
            break;
        }

        switch (operationSplit[0])
        {
          case "+":
            update = (item) => item.WorryLevel += updateValue.HasValue ? updateValue.Value : item.WorryLevel;
            break;
          case "*":
            update = (item) => item.WorryLevel *= updateValue.HasValue ? updateValue.Value : item.WorryLevel;
            break;
          default:
            throw new Exception();
        }

        if(!lines[currentIndex+3].StartsWith("  Test: divisible by "))
        {
          throw new Exception();
        }
        var testSplit = lines[currentIndex + 3].Split(' ');
        Func<Item, bool> test = (item) => item.WorryLevel % int.Parse(testSplit.Last()) == 0;

        if(!lines[currentIndex+4].StartsWith("    If true: throw to monkey "))
        {
          throw new Exception();
        }
        var trueMonkey = int.Parse(lines[currentIndex + 4].Split(' ').Last());

        if (!lines[currentIndex + 5].StartsWith("    If false: throw to monkey "))
        {
          throw new Exception();
        }
        var falseMonkey = int.Parse(lines[currentIndex + 5].Split(' ').Last());

        var monkey = new Monkey
        {
          Number = monkeyNumber,
          UpdateItem = update,
          TestAndThrow = (item) => monkeys[test(item) ? trueMonkey : falseMonkey].Items.Add(item)
        };
        monkey.Items.AddRange(items);

        monkeys.Add(monkey);

        currentIndex += 7;
      }

      for(int round = 0; round < 20; ++round)
      {
        foreach(var monkey in monkeys)
        {
          monkey.Inspect();
        }
      }


      foreach(var monkey in monkeys)
      {
        Console.WriteLine($"Monkey {monkey.Number} inspected items {monkey.ItemsInspected} times");
      }

      var itemsInspectedList = monkeys.Select(m => m.ItemsInspected).OrderByDescending(m => m).ToList();
      Console.WriteLine(itemsInspectedList[0] * itemsInspectedList[1]);
    }

    public static void RunPart2()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay11.txt").ToList();
      var monkeys = new List<Monkey>();
      var superModulo = 1;

      var currentIndex = 0;
      while (currentIndex < lines.Count)
      {
        var monkeyNumber = int.Parse(lines[currentIndex].Split(" :".ToCharArray())[1]);
        var items = lines[currentIndex + 1].Split(": ")[1].Split(", ").Select(int.Parse).Select(i => new Item { WorryLevel = i }).ToList();
        if (!lines[currentIndex + 2].StartsWith("  Operation: new = old "))
        {
          throw new Exception();
        }
        var operationSplit = string.Join("", lines[currentIndex + 2].Skip(23)).Split(' ');
        if (operationSplit.Length != 2)
        {
          throw new Exception();
        }

        Action<Item> update;
        int? updateValue = null;
        switch (operationSplit[1])
        {
          case "old":
            break;
          default:
            updateValue = int.Parse(operationSplit[1]);
            break;
        }

        switch (operationSplit[0])
        {
          case "+":
            update = (item) => item.WorryLevel += updateValue.HasValue ? updateValue.Value : item.WorryLevel;
            break;
          case "*":
            update = (item) => item.WorryLevel *= updateValue.HasValue ? updateValue.Value : item.WorryLevel;
            break;
          default:
            throw new Exception();
        }

        if (!lines[currentIndex + 3].StartsWith("  Test: divisible by "))
        {
          throw new Exception();
        }
        var testSplit = lines[currentIndex + 3].Split(' ');
        Func<Item, bool> test = (item) => item.WorryLevel % int.Parse(testSplit.Last()) == 0;
        superModulo *= int.Parse(testSplit.Last());

        if (!lines[currentIndex + 4].StartsWith("    If true: throw to monkey "))
        {
          throw new Exception();
        }
        var trueMonkey = int.Parse(lines[currentIndex + 4].Split(' ').Last());

        if (!lines[currentIndex + 5].StartsWith("    If false: throw to monkey "))
        {
          throw new Exception();
        }
        var falseMonkey = int.Parse(lines[currentIndex + 5].Split(' ').Last());

        var monkey = new Monkey
        {
          Number = monkeyNumber,
          UpdateItem = update,
          TestAndThrow = (item) => monkeys[test(item) ? trueMonkey : falseMonkey].Items.Add(item),
          DivideWorryLevel = false
        };
        monkey.Items.AddRange(items);

        monkeys.Add(monkey);

        currentIndex += 7;
      }

      foreach(var monkey in monkeys)
      {
        monkey.Supermodulo = superModulo;
      }

      for (int round = 0; round < 10000; ++round)
      {
        if(round % 100 == 0)
        {
          Console.WriteLine($"Round {round}");
        }
        foreach (var monkey in monkeys)
        {
          monkey.Inspect();
        }
      }


      foreach (var monkey in monkeys)
      {
        Console.WriteLine($"Monkey {monkey.Number} inspected items {monkey.ItemsInspected} times");
      }

      var itemsInspectedList = monkeys.Select(m => m.ItemsInspected).OrderByDescending(m => m).ToList();
      Console.WriteLine(itemsInspectedList[0] * itemsInspectedList[1]);
    }


    public class Monkey
    {
      public long? Supermodulo { get; set; }
      public bool DivideWorryLevel { get; set; } = true;
      public int Number { get; set; }
      public Action<Item> UpdateItem { get; set; }
      public Action<Item> TestAndThrow { get; set; }
      public List<Item> Items { get; } = new List<Item>();
      public long ItemsInspected { get; set; }

      public void Inspect()
      {
        foreach(var item in Items)
        {
          UpdateItem(item);
          if (DivideWorryLevel)
          {
            item.WorryLevel = item.WorryLevel / 3;
          }
          if(Supermodulo.HasValue)
          {
            item.WorryLevel = item.WorryLevel % Supermodulo.Value;
          }
          TestAndThrow(item);
          ItemsInspected++;
        }

        Items.Clear();
      }
    }

    public class Item
    {
      public BigInteger WorryLevel { get; set; }
    }

  }
}
