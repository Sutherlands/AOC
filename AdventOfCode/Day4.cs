using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day4
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay4.txt").ToList();
      var numbers = lines[0].Split(',').Select(int.Parse).ToList();

      var boardCollection = new HashSet<int[]>();
      for(var index = 2; index < lines.Count; index+=6)
      {
        var board = new int[25];
        for(var boardIndex = 0; boardIndex < 5; ++boardIndex)
        {
          var boardLine = lines[index + boardIndex].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
          board[boardIndex * 5] = boardLine[0];
          board[boardIndex * 5+1] = boardLine[0+1];
          board[boardIndex * 5+2] = boardLine[0+2];
          board[boardIndex * 5+3] = boardLine[0+3];
          board[boardIndex * 5+4] = boardLine[0+4];
        }
        boardCollection.Add(board);
      }

      int[] winningBoard = default;
      var totalNumbers = new List<int>();
      var lastNumber = 0;
      foreach(var number in numbers)
      {
        totalNumbers.Add(number);
        lastNumber = number;
        winningBoard = boardCollection.FirstOrDefault(b => BoardWins(b, totalNumbers));
        if(winningBoard != default)
        {
          break;
        }
      }

      var unusedNumbers = winningBoard.Except(totalNumbers);

      Console.WriteLine(unusedNumbers.Sum() * lastNumber);
    }



    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay4.txt").ToList();
      var numbers = lines[0].Split(',').Select(int.Parse).ToList();

      var boardCollection = new HashSet<int[]>();
      for (var index = 2; index < lines.Count; index += 6)
      {
        var board = new int[25];
        for (var boardIndex = 0; boardIndex < 5; ++boardIndex)
        {
          var boardLine = lines[index + boardIndex].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
          board[boardIndex * 5] = boardLine[0];
          board[boardIndex * 5 + 1] = boardLine[0 + 1];
          board[boardIndex * 5 + 2] = boardLine[0 + 2];
          board[boardIndex * 5 + 3] = boardLine[0 + 3];
          board[boardIndex * 5 + 4] = boardLine[0 + 4];
        }
        boardCollection.Add(board);
      }

      int[] losingBoard = default;
      var totalNumbers = new List<int>();
      var lastNumber = 0;
      foreach (var number in numbers)
      {
        totalNumbers.Add(number);
        lastNumber = number;

        if (losingBoard == default)
        {
          var losingBoards = boardCollection.Where(b => !BoardWins(b, totalNumbers));

          if (losingBoards.Count() == 1)
          {
            losingBoard = losingBoards.Single();
          }
        }else
        {
          if(BoardWins(losingBoard, totalNumbers))
          {
            break;
          }
        }
      }

      var unusedNumbers = losingBoard.Except(totalNumbers);

      Console.WriteLine(unusedNumbers.Sum() * lastNumber);
    }

    public static bool BoardWins(int[] board, List<int> numbers)
    {
      for (int x = 0; x < 5; ++x)
      {
        if (numbers.Contains(board[x * 5 + 0])
          && numbers.Contains(board[x * 5 + 1])
          && numbers.Contains(board[x * 5 + 2])
          && numbers.Contains(board[x * 5 + 3])
          && numbers.Contains(board[x * 5 + 4])
          )
        {
          return true;
        }

        if(numbers.Contains(board[0 + x])
          && numbers.Contains(board[5 + x])
          && numbers.Contains(board[10 + x])
          && numbers.Contains(board[15 + x])
          && numbers.Contains(board[20 + x])
          )
        {
          return true;
        }
      }
      return false;
    }

  }
}
