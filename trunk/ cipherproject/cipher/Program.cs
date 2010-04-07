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
            Console.WriteLine("decrypting!!\n");
            Statistics c = new Statistics();
            
            try
            {
                String fileName = args[0];
                for (int i = 1; i < args.Length; i++)
                {
                    fileName = fileName + " " + args[i];
                }
                Console.WriteLine(fileName);
                
                Console.WriteLine("done!\n");
                c.getStatsFromFile(fileName);
                c.PrintKeysAndValues(c.LetterAppearances);
                Console.WriteLine("second print");
                int counter = 0;
                foreach (CharIntPair letAp in c.LetterAppearancesSorted)
                {
                    counter++;
                    Console.WriteLine(" {0} : letter: {1}  ,appearances: {2}",counter, letAp.Key, letAp.Value);
                }
                String temp = "word word2 word3";
                String[] temp2 = temp.Split();
                foreach (String str in temp2)
                {
                    Console.WriteLine(str);
                }
                
                foreach (KeyValuePair<String,int> word in c.TwoLetterWords)
                {
                    Console.WriteLine("word is: {0} , appears {1} times.",word.Key,word.Value);
                }
                Console.ReadLine();
                foreach (KeyValuePair<String, int> word in c.ThreeLetterWords)
                {
                    Console.WriteLine("word is: {0} , appears {1} times.", word.Key, word.Value);
                }
              //  foreach (KeyValuePair<String, int> word in c._lastThreeLetters)
              //  {
              //      Console.WriteLine("word is: {0} , appears {1} times.", word.Key, word.Value);
              //  }
                foreach (KeyValuePair<String, int> word in c.OneLetterWords)
                {
                    Console.WriteLine("word is: {0} , appears {1} times.", word.Key, word.Value);
                }
                foreach (KeyValuePair<char, int> ch in c.PossiblyCapital)
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
