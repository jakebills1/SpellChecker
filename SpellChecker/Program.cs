using System;
using System.IO;
using System.Text.RegularExpressions;
namespace SpellChecker
{
  public class Trie
  {
    static readonly int ALPHABET_SIZE = 26;
    class TrieNode
    {
      public TrieNode[] children = new TrieNode[ALPHABET_SIZE];
      public bool EndOfWord;

      public TrieNode()
      {
        EndOfWord = false;
        for (int i = 0; i < ALPHABET_SIZE; i++)
        {
          children[i] = null;
        }
      }
    }
    static TrieNode root = new TrieNode();
    public static void Insert(String key)
    {
      int level;
      int length = key.Length;
      int index;

      TrieNode current = root;

      for (level = 0; level < length; level++)
      {
        index = key[level] - 'a';
        if (current.children[index] == null)
          current.children[index] = new TrieNode();

        current = current.children[index];
      }

      current.EndOfWord = true;
    }
    public static bool Search(String key)
    {
      TrieNode current = root;

      int index;
      for (int i = 0; i < key.Length; i++)
      {
        index = key[i] - 'a';
        if (index >= 0 && index <= 25)
        {
          if (current.children[index] == null)
          {
            return false;
          }
          else
          {
            current = current.children[index];
          }
        }
      }
      return true;
    }

  }
  class Program
  {
    enum ExitCode
    {
      NoError = 0,
      NoCommandLineArg = 1
    }

    static int Main(string[] args)
    {
      string dictFilePath;
      string fileToCheck;
      string writeFilePath;
      if (args.Length == 3) // validate command line arguments
      {
        dictFilePath = args[0];
        fileToCheck = args[1];
        writeFilePath = args[2];
      }
      else
      {
        Console.WriteLine("Usage: dotnet run <path to dictionary file> <word to spell check> <out file>");
        return (int)ExitCode.NoCommandLineArg;
      }
      File.WriteAllText(writeFilePath, String.Empty);
      // ensures outfile is empty
      // the @ sign indicates a string literal, so the backslashes do not need to be escaped
      StreamReader dictFile = OpenFile(dictFilePath);
      StreamReader checkFile = OpenFile(fileToCheck);
      StreamWriter writeFile = new StreamWriter(writeFilePath);
      writeFile.WriteLine("Incorrectly spelled words:");
      string line;
      while ((line = dictFile.ReadLine()) != null)
      {
        Trie.Insert(line.ToLower());
      }
      int misspelledWordCount = 0;
      string[] words = checkFile.ReadToEnd().Split(' ');

      foreach (string word in words)
      {
        if (!Trie.Search(Normalize(word)))
        {
          if (!CheckForControl(word))
          {
            misspelledWordCount++;
            writeFile.WriteLine(Normalize(word));
          }
        }
      }
      dictFile.Close();
      checkFile.Close();
      writeFile.Close();
      Console.WriteLine("Spellcheck completed. There were " + misspelledWordCount + " misspelled words. Press enter to exit");
      Console.ReadLine();
      return (int)ExitCode.NoError;
    }

    static StreamReader OpenFile(string filePath)
    {
      return new StreamReader(filePath); ;
    }
    static string Normalize(string word)
    {
      string normWord = Regex.Replace(word, @"[^\u0009\u000A\u000D\u0020-\u007E]", "*");
      return normWord.ToLower();
    }
    static bool CheckForControl(string word)
    {
      for (int i = 0; i < word.Length; i++)
      {
        if (Char.IsControl(word[i]))
          return false;
      }
      return true;
    }
  }
}
/*
 * optimizations:
 * 1. parameters to Search must be valid words, no punctuation
 * 2. write function to validate words and return an array of words with no punctuation, blank space, etc 
 */

