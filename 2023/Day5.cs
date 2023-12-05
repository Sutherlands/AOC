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
  public static class Day5
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay5.txt").ToList();

      var seedToSoilMap = new List<MapInput>();
      var soilToFertilizerMap = new List<MapInput>();
      var fertilizerToWaterMap = new List<MapInput>();
      var waterToLightMap = new List<MapInput>();
      var lightToTemperatureMap = new List<MapInput>();
      var temperatureToHumidityMap = new List<MapInput>();
      var humidityToLocationMap = new List<MapInput>();

      var seeds = lines[0].Split(": ")[1].Split(" ").Select(long.Parse);

      var currentMap = seedToSoilMap;

      for (int x = 2; x < lines.Count; ++x)
      {
        switch(lines[x])
        {
          case "":
            continue;
          case "seed-to-soil map:":
            currentMap = seedToSoilMap;
            continue;
          case "soil-to-fertilizer map:":
            currentMap = soilToFertilizerMap;
            continue;
          case "fertilizer-to-water map:":
            currentMap = fertilizerToWaterMap;
            continue;
          case "water-to-light map:":
            currentMap = waterToLightMap;
            continue;
          case "light-to-temperature map:":
            currentMap = lightToTemperatureMap;
            continue;
          case "temperature-to-humidity map:":
            currentMap = temperatureToHumidityMap;
            continue;
          case "humidity-to-location map:":
            currentMap = humidityToLocationMap;
            continue;
        }

        currentMap.Add(new MapInput(lines[x]));
      }

      var seedLocations = seeds.Select(seed => GetMapValue(humidityToLocationMap, GetMapValue(temperatureToHumidityMap, GetMapValue(lightToTemperatureMap, GetMapValue(waterToLightMap, GetMapValue(fertilizerToWaterMap, GetMapValue(soilToFertilizerMap, GetMapValue(seedToSoilMap, seed))))))));
      Console.WriteLine(seedLocations.Min());
    }

    public static long GetMapValue(List<MapInput> map, long value)
    {
      foreach(var input in map)
      {
        if(input.Contains(value))
        {
          return input.GetNewValue(value);
        }
      }
      return value;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay5.txt").ToList();

      var seedToSoilMap = new List<MapInput>();
      var soilToFertilizerMap = new List<MapInput>();
      var fertilizerToWaterMap = new List<MapInput>();
      var waterToLightMap = new List<MapInput>();
      var lightToTemperatureMap = new List<MapInput>();
      var temperatureToHumidityMap = new List<MapInput>();
      var humidityToLocationMap = new List<MapInput>();

      var seedNumbers = lines[0].Split(": ")[1].Split(" ").Select(long.Parse).ToList();
      var seeds = new List<Range>();
      for(var x = 0; x < seedNumbers.Count; x+=2)
      {
        seeds.Add(new Range(seedNumbers[x], seedNumbers[x] + seedNumbers[x + 1] - 1));
      }

      var currentMap = seedToSoilMap;

      for (int x = 2; x < lines.Count; ++x)
      {
        switch (lines[x])
        {
          case "":
            continue;
          case "seed-to-soil map:":
            currentMap = seedToSoilMap;
            continue;
          case "soil-to-fertilizer map:":
            currentMap = soilToFertilizerMap;
            continue;
          case "fertilizer-to-water map:":
            currentMap = fertilizerToWaterMap;
            continue;
          case "water-to-light map:":
            currentMap = waterToLightMap;
            continue;
          case "light-to-temperature map:":
            currentMap = lightToTemperatureMap;
            continue;
          case "temperature-to-humidity map:":
            currentMap = temperatureToHumidityMap;
            continue;
          case "humidity-to-location map:":
            currentMap = humidityToLocationMap;
            continue;
        }

        currentMap.Add(new MapInput(lines[x]));
      }

      seeds = ProcessMap(seedToSoilMap, seeds).ToList();
      seeds = ProcessMap(soilToFertilizerMap, seeds).ToList();
      seeds = ProcessMap(fertilizerToWaterMap, seeds).ToList();
      seeds = ProcessMap(waterToLightMap, seeds).ToList();
      seeds = ProcessMap(lightToTemperatureMap, seeds).ToList();
      seeds = ProcessMap(temperatureToHumidityMap, seeds).ToList();
      seeds = ProcessMap(humidityToLocationMap, seeds).ToList();
      Console.WriteLine(seeds.Select(s => s.Start).Min());
    }

    private class Range
    {
      public Range(long start, long end)
      {
        Start = start;
        End = end;
      }

      public long Start { get; }
      public long End { get; }

      public IEnumerable<Range> BreakAt(long position)
      {
        if (position < Start || position > End)
          yield return this;
        else
        {
          yield return new Range(Start, position - 1);
          yield return new Range(position, End);
        }
      }
    }

    private static IEnumerable<Range> ProcessMap(List<MapInput> map, IEnumerable<Range> seedRanges)
    {
      var breaks = map.SelectMany(m => new []{ m.SourceStart, m.SourceStart + m.Size });
      foreach(var b in breaks)
      {
        seedRanges = seedRanges.SelectMany(sr => sr.BreakAt(b));
      }

      seedRanges = seedRanges.Select(sr => { var newStart = GetMapValue(map, sr.Start); return new Range(newStart, sr.End - sr.Start + newStart); });
      return seedRanges;
    }

    public class MapInput
    {
      public MapInput(string line)
      {
        var parts = line.Split(" ").Select(long.Parse).ToList();
        DestinationStart = parts[0];
        SourceStart = parts[1];
        Size = parts[2];
      }
      public long SourceStart { get; set; }
      public long DestinationStart { get; set; }
      public long Size { get; set; }

      public bool Contains(long value)
      {
        return value >= SourceStart && value < (SourceStart + Size);
      }

      public long GetNewValue(long value)
      {
        var newValue = value - SourceStart + DestinationStart;
        return newValue;
      }
    }

    public class SeedRanges
    {
      public List<(long, long)> InputRanges { get; set; }
      public List<(long, long)> OutputRanges { get; set; }

    }
  }
}
