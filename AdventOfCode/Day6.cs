using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day6
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay6.txt").ToList();
      var fishNumbers = lines[0].Split(',').Select(long.Parse).ToList();

      var fishNumbersDict = fishNumbers.GroupBy(f => f).ToDictionary(f => f.Key, f => (long)f.Count());
      for(int index = 0; index <= 8; ++index)
      {
        if(!fishNumbersDict.ContainsKey(index))
        {
          fishNumbersDict[index] = 0;
        }
      }

      for(int day = 0; day < 256; ++day)
      {
        fishNumbersDict = new Dictionary<long, long> { 
          { 0L, fishNumbersDict[1] } ,
          { 1L, fishNumbersDict[2] } ,
          { 2L, fishNumbersDict[3] } ,
          { 3L, fishNumbersDict[4] } ,
          { 4L, fishNumbersDict[5] } ,
          { 5L, fishNumbersDict[6] } ,
          { 6L, fishNumbersDict[7] + fishNumbersDict[0] } ,
          { 7L, fishNumbersDict[8] } ,
          { 8L, fishNumbersDict[0] } ,
        };
      }


      Console.WriteLine(
        fishNumbersDict[0] +
        fishNumbersDict[1] +
        fishNumbersDict[2] +
        fishNumbersDict[3] +
        fishNumbersDict[4] +
        fishNumbersDict[5] +
        fishNumbersDict[6] +
        fishNumbersDict[7] +
        fishNumbersDict[8]
        );
    }

    public static void RunPart2()
    {
    }


  }
}
