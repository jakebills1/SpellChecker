using System;
using System.IO;
using System.Collections.Generic;
namespace SpellChecker
{
    class Program
    {
        enum ExitCode
        {
            NoError = 0,
            NoCommandLineArg = 1
        }

        static int Main(string[] args)
        {
            string filePath;
            string fileToCheck;
            string writeFilePath = @"C:\Users\Jake\source\repos\SpellChecker\out.txt";
            File.WriteAllText(writeFilePath, String.Empty);
            if (args.Length == 2) // validate command line arguments
            {
                filePath = args[0];
                fileToCheck = args[1];
            }
            else
            {
                Console.WriteLine("Usage: dotnet run <path to dictionary file> <word to spell check>");
                return (int)ExitCode.NoCommandLineArg;
            }
            // the @ sign indicates a string literal, so the backslashes do not need to be escaped
            StreamReader file = OpenFile(filePath);
            StreamReader checkFile = OpenFile(fileToCheck);
            StreamWriter writeFile = new StreamWriter(writeFilePath);
            LinkedList<string>[] values = new LinkedList<string>[50];
            LoadHashTable(file, values);
            int misspelledCount = 0;
            // read file line by line, and seperate by word, seperately send each word to spellcheck, if true, incremenet a counter to return a count of misspelled words.
            string[] words = checkFile.ReadToEnd().Split(' ');
            foreach(string word in words)
            {
                if (!SpellCheck(word.ToLower().Trim(), values, writeFile))
                    misspelledCount++;
            }

            Console.WriteLine("There were " + misspelledCount + " misspelled words");

            file.Close();
            checkFile.Close();
            writeFile.Close();
            Console.ReadLine();
            return (int)ExitCode.NoError;
        }

        static StreamReader OpenFile(string filePath)
        {
            return new StreamReader(filePath);;
        }
        static void LoadHashTable(StreamReader file, LinkedList<string>[] values)
        {
            int valuesLoaded = 0;
            string line;
            // values is an array of size 50 with LinkedLists as the datatype
            while ((line = file.ReadLine()) != null)
            {
                LinkedListNode<string> newNode = new LinkedListNode<string>(line.ToLower());
                int hash = HashFunction(line);
                if (values[hash] == null) // if bucket is not yet initialized
                {
                    values[hash] = new LinkedList<string>();
                    values[hash].AddLast(newNode);
                    valuesLoaded++;
                }
                else // if bucket has already been accessed
                {
                    values[hash].AddLast(newNode);
                    valuesLoaded++;
                }
            }
            Console.WriteLine(valuesLoaded);
        }
        static int HashFunction(string s)
        {
            // adds the ascii values, and return that value mod the size of the hash table
            int sum = 0;
            for(int i = 0; i < s.Length; i++)
            {
                sum += s[i];
            }
            return sum % 50;
        }
        static bool SpellCheck(string word, LinkedList<string>[] values, StreamWriter file)
        {
            // returns true if word spelled correctly
            // arrays are passed by reference
            if (word.EndsWith(",") || word.EndsWith(".") || word.EndsWith(";"))
            {
                char[] chars = { ',', '.', ';' };
                word = word.TrimEnd(chars);
                int hash = HashFunction(word);
                if (values[hash].Contains(word))
                {
                    return true;
                }
                else
                {
                    file.WriteLine(word);
                    return false;
                }

            }
            else
            {
                int hash = HashFunction(word);
                if (values[hash].Contains(word))
                {
                    return true;
                }
                else
                {
                    file.WriteLine(word);
                    return false;
                }
            }
        }
    }
}
/*
 * optimizations:
 * 1. ignore blank lines
 * 2. speed up testing for punctuation and invalid words and such
 */

