using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

// test comment for SVN
namespace cipher
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                String fileName = args[0];
                for (int i = 1; i < args.Length; i++)
                {
                    fileName = fileName + " " + args[i];
                }
                Console.WriteLine(fileName);
                Statistics c = new Statistics(fileName);
                
                foreach (StringIntPair word in c.TwoLetterWordsSorted)
                {
                    Console.WriteLine("word is: {0} , appears {1} times.",word.Key,word.Value);
                }
                Console.ReadLine();
                foreach (StringIntPair word in c.ThreeLetterWordsSorted)
                {
                    Console.WriteLine("word is: {0} , appears {1} times.", word.Key, word.Value);
                }
              //  foreach (KeyValuePair<String, int> word in c._lastThreeLetters)
              //  {
              //      Console.WriteLine("word is: {0} , appears {1} times.", word.Key, word.Value);
              //  }
                Console.ReadLine();
                foreach (StringIntPair word in c.OneLetterWordsSorted)
                {
                    Console.WriteLine("word is: {0} , appears {1} times.", word.Key, word.Value);
                }
                Console.ReadLine();
                foreach (CharIntPair ch in c.PossiblyCapitalSorted)
                {
                    Console.WriteLine("letter is: {0} , appears {1} times.", ch.Key, ch.Value);
                }


                Analysis test = new Analysis(c);
                test.addLetterFreq();
                test.addOneLetterWord();
                test.encrypteByPossibilities();
                test.randomFill();
                Console.WriteLine("The table is : \n {0}",test.Table.ToString());
                Console.WriteLine("subs are: \n{0}", test.printSubstitutions());
                Console.WriteLine("The key is : \n {0}", test.printKey());
                Console.WriteLine("key length is: {0}", test.EncryptionKey.Count);
                Console.WriteLine("your grade is: {0}", test.calcGrade("The Wonderful Wizard of OZ.txt.key2.txt", test.printKey()));

           
                
            }
            catch (IOException e)
            {
                Console.WriteLine("file does not exist");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("mising argument\n");
            }
        }
        

    }

    
}
