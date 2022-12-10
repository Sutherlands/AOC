using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
  public static class Day7

  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay7.txt").ToList();

      List<string> directoryPath = new List<string>();

      Directory rootDirectory = new Directory();
      Directory currentDirectory = rootDirectory;


      foreach(var line in lines)
      {
        var parsed = line.Split(' ');
        if(parsed[0] == "$")
        {
          switch(parsed[1])
          {
            case "cd":
              switch(parsed[2])
              {
                case "/":
                  directoryPath = new List<string>();
                  currentDirectory = rootDirectory;
                  break;
                case "..":
                  directoryPath.RemoveAt(directoryPath.Count - 1);
                  currentDirectory = FindDirectory(rootDirectory, directoryPath);
                  break;
                default:
                  directoryPath.Add(parsed[2]);
                  currentDirectory = FindDirectory(rootDirectory, directoryPath);
                  break;
              }
              break;
          }
        }
        else
        {
          if(parsed[0] == "dir")
          {
            currentDirectory.Entries.Add(new Directory { Name = parsed[1] });
          }
          else
          {
            currentDirectory.Entries.Add(new FileEntry { Size = int.Parse(parsed[0]), Name = parsed[1] });
          }
        }
      }

      var size = rootDirectory.GetDirectories().Where(d => d.GetSize() < 100000).Sum(d => d.GetSize());
      
      Console.WriteLine(size);
    }

    private static Directory FindDirectory(Directory rootDirectory, List<string> directoryPath)
    {
      var currentDirectory = rootDirectory;
      foreach(var path in directoryPath)
      {
        currentDirectory = (Directory)currentDirectory.Entries.Where(e => e.Name == path).First();
      }
      return currentDirectory;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay7.txt").ToList();

      List<string> directoryPath = new List<string>();

      Directory rootDirectory = new Directory();
      Directory currentDirectory = rootDirectory;


      foreach (var line in lines)
      {
        var parsed = line.Split(' ');
        if (parsed[0] == "$")
        {
          switch (parsed[1])
          {
            case "cd":
              switch (parsed[2])
              {
                case "/":
                  directoryPath = new List<string>();
                  currentDirectory = rootDirectory;
                  break;
                case "..":
                  directoryPath.RemoveAt(directoryPath.Count - 1);
                  currentDirectory = FindDirectory(rootDirectory, directoryPath);
                  break;
                default:
                  directoryPath.Add(parsed[2]);
                  currentDirectory = FindDirectory(rootDirectory, directoryPath);
                  break;
              }
              break;
          }
        }
        else
        {
          if (parsed[0] == "dir")
          {
            currentDirectory.Entries.Add(new Directory { Name = parsed[1] });
          }
          else
          {
            currentDirectory.Entries.Add(new FileEntry { Size = int.Parse(parsed[0]), Name = parsed[1] });
          }
        }
      }

      var allowedSpace = 40000000;
      var usedSpace = rootDirectory.GetSize();
      var neededSpace = usedSpace - allowedSpace;

      var size = rootDirectory.GetDirectories().OrderBy(d => d.GetSize()).Where(d => d.GetSize() > neededSpace).First().GetSize();

      Console.WriteLine(size);
    }

    public interface IEntry
    {
      string Name { get; }
      int GetSize();
    }

    public class FileEntry : IEntry
    {
      public int Size { get; set; }
      public string Name { get; set; }
      public int GetSize() { return Size; }
    }

    public class Directory : IEntry
    {
      public string Name { get; set; }
      public List<IEntry> Entries { get; private set; } = new List<IEntry>();
      public int GetSize()
      {
        return Entries.Select(e => e.GetSize()).Sum();
      }
      public IEnumerable<Directory> GetDirectories()
      {
        yield return this;
        foreach(var child in Entries.OfType<Directory>())
        {
          foreach(var directory in child.GetDirectories())
          {
            yield return directory;
          }
        }
      }
    }
  }
}
