using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  public static class Day13
  {
    public static void RunPart1()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay13.txt").ToList();

      var validIndices = new List<int>();
      for(int index = 0; index < lines.Count; index += 3)
      {
        var lhs = Parse(GetStreamReader(lines[index]));
        var rhs = Parse(GetStreamReader(lines[index+1]));

        if(IsValid(lhs, rhs).Value)
        {
          validIndices.Add(index/3 + 1);
        }
      }
      Console.WriteLine(validIndices.Sum()) ;
      Console.WriteLine($"Processed in {sw.ElapsedMilliseconds}");
    }


    public static void RunPart2()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay13.txt").ToList()
        .Concat(new[] { "[[2]]", "[[6]]" });

      var dataList = new List<IData>();
      foreach(var line in lines)
      {
        if(string.IsNullOrWhiteSpace(line))
        {
          continue;
        }
        var data = Parse(GetStreamReader(line));
        data.Line = line;
        dataList.Add(data);
      }

      dataList.Sort(new DataComparer());

      var twoIndex = dataList.FindIndex(i => i.Line == "[[2]]") + 1;
      var sixIndex = dataList.FindIndex(i => i.Line == "[[6]]") + 1;

      Console.WriteLine(twoIndex*sixIndex);
      Console.WriteLine($"Processed in {sw.ElapsedMilliseconds}");
    }

    public class DataComparer : IComparer<IData>
    {
      public int Compare([AllowNull] IData x, [AllowNull] IData y)
      {
        return IsValid(x, y).Value ? -1 : 1;
      }
    }


    public static StreamReader GetStreamReader(string line)
    {
      var stream = new MemoryStream();
      var writer = new StreamWriter(stream);
      writer.Write(line);
      writer.Flush();
      stream.Position = 0;
      return new StreamReader(stream);
    }

    public static IData Parse(StreamReader data)
    {
      switch((char)data.Peek())
      {
        case '[':
          data.Read();
          return ParseList(data);
        default:
          return ParseInteger(data);
      }
    }
    public static IData ParseList(StreamReader data)
    {
      var listData = new ListData();
      while(!data.EndOfStream)
      {
        switch((char)data.Peek())
        {
          case ']':
            data.Read();
            return listData;
          case '[':
            data.Read();
            listData.Value.Add(ParseList(data));
            break;
          case ',':
            data.Read();
            break;
          default:
            listData.Value.Add(ParseInteger(data));
            break;
        }
      }
      return listData;
    }

    public static IData ParseInteger(StreamReader data)
    {
      string intString = "";
      while(char.IsNumber((char)data.Peek()))
      {
        intString += (char)data.Read();
      }
      return new IntegerData { Value = int.Parse(intString) };
    }

    public interface IData
    {
      ListData ToList();
      string Line { get; set; }
    }

    public class IntegerData : IData
    {
      public int Value { get; set; }
      public string Line { get; set; }

      public ListData ToList()
      {
        return new ListData { Value = new List<IData> { this } };
      }
    }

    public class ListData : IData
    {
      public List<IData> Value { get; set; } = new List<IData>();
      public string Line { get; set; }

      public ListData ToList()
      {
        return this;
      }
    }

    public static bool? IsValid(IData lhs, IData rhs)
    {
      if(lhs is IntegerData && rhs is IntegerData)
      {
        if(((IntegerData)lhs).Value == ((IntegerData)rhs).Value)
        {
          return null;
        }
        return ((IntegerData)lhs).Value < ((IntegerData)rhs).Value;
      }

      var lhsList = lhs.ToList();
      var rhsList = rhs.ToList();

      for(int x = 0; x < lhsList.Value.Count && x < rhsList.Value.Count; ++x)
      {
        var validValue = IsValid(lhsList.Value[x], rhsList.Value[x]);
        if(validValue.HasValue)
        {
          return validValue;
        }
      }

      if (lhsList.Value.Count == rhsList.Value.Count)
      {
        return null;
      }
      return lhsList.Value.Count < rhsList.Value.Count;
    }
  }
}
