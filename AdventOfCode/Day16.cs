using DotNetty.Common.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode
{
  public static class Day16
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay16.txt").ToList();
      var binaryString = string.Join(string.Empty, lines[0].Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

      int startingIndex = 0;

      var packet = ParsePacket(binaryString, ref startingIndex);

      Console.WriteLine(packet.GetTotalVersionNumbers());
      Console.WriteLine(packet.GetValue());
    }

    private static Packet ParsePacket(string binaryString, ref int startingIndex)
    {
      var version = GetValue(binaryString, ref startingIndex, 3);
      var typeId = GetValue(binaryString, ref startingIndex, 3);

      if (typeId == 4)
      {
        return ParseLiteralPacket(version, typeId, binaryString, ref startingIndex);
      }
      else
      {
        return ParseOperatorPacket(version, typeId, binaryString, ref startingIndex);
      }
    }

    private static Packet ParseOperatorPacket(long version, long typeId, string bits, ref int startingIndex)
    {
      var packets = new List<Packet>();
      var isTotalLengthMode = bits[startingIndex] == '0';
      startingIndex++;
      if(isTotalLengthMode)
      {
        var totalLength = (int)GetValue(bits, ref startingIndex, 15);
        var limitedBits = string.Concat(bits.Skip(startingIndex).Take(totalLength));
        var temporaryStartingIndex = 0;
        while(temporaryStartingIndex <  totalLength)
        {
          packets.Add(ParsePacket(limitedBits, ref temporaryStartingIndex));
        }
        startingIndex += totalLength;
      }
      else
      {
        var packetCount = GetValue(bits, ref startingIndex, 11);
        for(int count = 0; count < packetCount; ++count)
        {
          packets.Add(ParsePacket(bits, ref startingIndex));
        }
      }
      return new OperatorPacket { Version = version, TypeId = typeId, Packets = packets};
    }

    private static long GetValue(string binaryString, ref int startingIndex, int length)
    {

      var value = Convert.ToInt64(string.Join("", binaryString.Skip(startingIndex).Take(length)), 2);
      startingIndex += length;
      return value;
    }

    public static LiteralPacket ParseLiteralPacket(long version, long typeId, string bits, ref int startingIndex)
    {
      var totalBits = new StringBuilder();
      bool isLast;
      do
      {
        isLast = bits[startingIndex] == '0';
        totalBits = totalBits.Append(string.Concat(bits.Skip(startingIndex + 1).Take(4)));
        startingIndex += 5;
      } while (!isLast);

      if (totalBits.Length > 64)
      {
        throw new ArgumentException();
      }

      var value = Convert.ToInt64(string.Join("", totalBits), 2);
      return new LiteralPacket { Version = version, TypeId = typeId, Value = value };
    }

    public abstract class Packet
    {
      public long Version { get; set; }
      public long TypeId { get; set; }

      public abstract long GetTotalVersionNumbers();
      public abstract long GetValue();
    }

    public class OperatorPacket : Packet
    {
      public List<Packet> Packets { get; set; }

      public override long GetTotalVersionNumbers()
      {
        return Packets.Sum(p => p.GetTotalVersionNumbers()) + Version;
      }

      public override long GetValue()
      {
        switch(TypeId)
        {
          case 0:
            return Packets.Sum(p => p.GetValue());
          case 1:
            return Packets.Select(p => p.GetValue()).Aggregate((a, b) => a*b);
          case 2:
            return Packets.Select(p => p.GetValue()).Aggregate(Math.Min);
          case 3:
            return Packets.Select(p => p.GetValue()).Aggregate(Math.Max);
          case 5:
            return Packets[0].GetValue() > Packets[1].GetValue() ? 1 : 0;
          case 6:
            return Packets[0].GetValue() < Packets[1].GetValue() ? 1 : 0;
          case 7:
            return Packets[0].GetValue() == Packets[1].GetValue() ? 1 : 0;
        }
        throw new ArgumentException();
      }
    }

    public class LiteralPacket : Packet
    {
      public long Value { get; set; }

      public override long GetTotalVersionNumbers()
      {
        return Version;
      }

      public override long GetValue()
      {
        return Value;
      }
    }

    public static void RunPart2()
    {
    }
  }
}
