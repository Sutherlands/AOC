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
  public static class Day24
  {
    public static Stopwatch sw;

    public static void RunPart1()
    {
      sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay24.txt").ToList();

      var commands = lines.Select(ParseLine).ToList();
      var commandsByInput = SplitByInput(commands);
      commandsByInput.Reverse();

      foreach(var commandList in commandsByInput)
      {
      }


      var existingStates = new List<State>();
      existingStates.Add(new State { W = 0, X = 0, Y = 0, Z = 0 });

      foreach(var commandList in commandsByInput)
      {
        var newStates = new List<State>();

        for (var input = 1; input <= 9; ++input)
        {
          foreach(var state in existingStates)
          {

          }
        }

        existingStates = newStates;
      }
      long w = 0, x = 0, y = 0, z = 0;
      var number = GetNumber(commandsByInput, w, x, y, z);

    }

    public class State
    {
      public long W { get; set; }
      public long X { get; set; }
      public long Y { get; set; }
      public long Z { get; set; }
      public List<long> Numbers { get; set; } = new List<long>();
    }

    private static string GetNumber(List<List<Command>> commandsByInput, long w, long x, long y, long z)
    {
      for (var input = 9; input > 0; --input)
      {

        if (commandsByInput.Count > 7)
        {
          Console.WriteLine(new string('x', 14 - commandsByInput.Count) + input + new string('x', commandsByInput.Count-1) + $" {sw.ElapsedMilliseconds}ms");
        }

        try
        {
          long tempW = w, tempX = x, tempY = y, tempZ = z;
          var commandsToProcess = commandsByInput[0];
          var queue = new Queue<int>();
          queue.Enqueue(input);
          foreach (var command in commandsToProcess)
          {
            command.Execute(ref tempW, ref tempX, ref tempY, ref tempZ, queue);
          }

          var nextCommands = commandsByInput.Skip(1).ToList();
          if(!nextCommands.Any())
          {
            if (tempZ == 0)
            {
              return input.ToString();
            }
            else
            {
              return default;
            }
          }

          var number = GetNumber(nextCommands, tempW, tempX, tempY, tempZ);
          if(number != default)
          {
            return input + number;
          }
        }
        catch (ModuloException)
        {
          continue;
        }
        catch (DivideByZeroException)
        {
          continue;
        }
      }

      return default;
    }

    private static List<List<Command>> SplitByInput(List<Command> commands)
    {
      var result = new List<List<Command>>();

      var currentCommandList = new List<Command>();
      foreach (var command in commands)
      {
        if (command.Type == CommandType.Input)
        {
          currentCommandList = new List<Command>();
          result.Add(currentCommandList);
        }

        currentCommandList.Add(command);
      }

      return result;
    }

    private static Command ParseLine(string line)
    {
      var parts = line.Split(' ');
      var command = new Command();
      command.Type = ParseType(parts[0]);
      command.FirstParameter = parts[1];
      if (parts.Length > 2)
      {
        command.SecondParameter = int.TryParse(parts[2], out var result) ? (int?)result : (object)parts[2];
      }
      return command;
    }

    private static CommandType ParseType(string v)
    {
      switch (v)
      {
        case "inp":
          return CommandType.Input;
        case "add":
          return CommandType.Add;
        case "mul":
          return CommandType.Multiply;
        case "div":
          return CommandType.Divide;
        case "mod":
          return CommandType.Modulo;
        case "eql":
          return CommandType.Equals;
      }
      throw new InvalidDataException();
    }

    public class Command
    {
      public CommandType Type { get; set; }

      public object FirstParameter { get; set; }
      public object SecondParameter { get; set; }

      public void Execute(ref long w, ref long x, ref long y, ref long z, Queue<int> values)
      {
        var firstValue = GetParameter(FirstParameter, w, x, y, z);
        var secondValue = GetParameter(SecondParameter, w, x, y, z);

        switch (Type)
        {
          case CommandType.Input:
            UpdateValue(values.Dequeue(), ref w, ref x, ref y, ref z);
            break;
          case CommandType.Add:
            UpdateValue(firstValue + secondValue, ref w, ref x, ref y, ref z);
            break;
          case CommandType.Multiply:
            UpdateValue(firstValue * secondValue, ref w, ref x, ref y, ref z);
            break;
          case CommandType.Divide:
            if (secondValue == 0)
            {
              throw new DivideByZeroException();
            }

            UpdateValue(firstValue / secondValue, ref w, ref x, ref y, ref z);
            break;
          case CommandType.Modulo:
            if (firstValue < 0 || secondValue <= 0)
            {
              throw new ModuloException();
            }

            UpdateValue(firstValue % secondValue, ref w, ref x, ref y, ref z);
            break;
          case CommandType.Equals:
            UpdateValue(firstValue == secondValue ? 1 : 0, ref w, ref x, ref y, ref z);
            break;
        }
      }



      private void UpdateValue(long value, ref long w, ref long x, ref long y, ref long z)
      {
        switch (FirstParameter)
        {
          case "w":
            w = value;
            break;
          case "x":
            x = value;
            break;
          case "y":
            y = value;
            break;
          case "z":
            z = value;
            break;
          default:
            throw new InvalidDataException();
        }
      }

      private long GetParameter(object value, long w, long x, long y, long z)
      {
        var intValue = value as int?;
        if(intValue != null)
        {
          return intValue.GetValueOrDefault();
        }

        switch (value)
        {
          case null: return 0;
          case "w": return w;
          case "x": return x;
          case "y": return y;
          case "z": return z;
        }

        throw new InvalidDataException();
      }
    }

    private class ModuloException : Exception { }

    public enum CommandType
    {
      Input,
      Add,
      Multiply,
      Divide,
      Modulo,
      Equals
    }

  }
}
