using System;
using System.IO;
using System.Collections.Generic;
namespace SpellChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Users\Jake\source\repos\SpellChecker\google-10000-english.txt";
            //string filePath = @"C:\Users\Jake\source\repos\SpellChecker\short-list.txt";
            // the @ sign indicates a string literal, so the backslashes do not need to be escaped
            StreamReader file = OpenFile(filePath);
            string line;
            LinkedList<string>[] values = new LinkedList<string>[50];
            // values is an array of size 50 with LinkedLists as the datatype
            while ((line = file.ReadLine()) != null)
            {
                LinkedListNode<string> newNode = new LinkedListNode<string>(line);
                int hash = HashFunction(line);
                if(values[hash] == null) // if bucket is not yet initialized
                {
                    values[hash] = new LinkedList<string>();
                    values[hash].AddLast(newNode);
                }
                else // if bucket has already been accessed
                {
                    values[hash].AddLast(newNode);
                }
            }
            // ==================== test for whether hashing operation was succesfful
            //for (int i = 0; i < 50; i++)
            //{
            //    if (values[i] != null)
            //    {
            //        Console.WriteLine(values[i].Count);

            //    }
            //}

            // ==================== spell check word
  
            if (SpellCheck("correct", values, HashFunction("correct")))
                Console.WriteLine("Correct!");
            else
                Console.WriteLine("Incorrect");

            file.Close();
            Console.ReadLine();

        }

        static StreamReader OpenFile(string filePath)
        {
            return new StreamReader(filePath);;
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
        static bool SpellCheck(string word, LinkedList<string>[] values, int hash)
        {
            // arrays are passed by reference
            if (values[hash] != null && values[hash].Contains(word))
            {
                return true;
            }
            return false;
        }
    }
}
