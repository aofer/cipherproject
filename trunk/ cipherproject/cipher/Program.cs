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
                int counter = 0;
                foreach (StringIntPair word in c.TwoLetterWordsSorted)
                {
                    Console.WriteLine("word is: {0} , appears {1} times.", word.Key, word.Value);
                }
                Console.ReadLine();
                foreach (StringIntPair word in c.ThreeLetterWordsSorted)
                {
                    Console.WriteLine("word is: {0} , appears {1} times.", word.Key, word.Value);
                }
                Console.ReadLine();
                foreach (StringIntPair word in c.BigramsSorted)
                {
                    if (counter++ > 30)
                    {
                        break;
                    }
                    Console.WriteLine("Bigram is: {0} , appears {1} times.", word.Key, word.Value);
                }
                counter = 0;
                Console.ReadLine();
                foreach (StringIntPair word in c.TrigramsSorted)
                {
                    if (counter++ > 30)
                    {
                        break;
                    }
                    Console.WriteLine("Trigram is: {0} , appears {1} times.", word.Key, word.Value);
                }
                Console.ReadLine();
                counter = 0;
                foreach (StringIntPair word in c.LastThreeLettersSorted)
                {
                    Console.WriteLine("word is: {0} , appears {1} times.", word.Key, word.Value);
                    if (counter++ > 100)
                    {
                        break;
                    }

                }
                counter = 0;
                foreach (StringIntPair word in c.QuadGramsSorted)
                {
                    Console.WriteLine("quadgram is: {0} , appears {1} times.", word.Key, word.Value);
                    if (counter++ > 100)
                    {
                        break;
                    }

                }
                Console.ReadLine();
                foreach (StringIntPair word in c.DoubleLettersSorted)
                {
                    Console.WriteLine("letters: {0} , appears {1} times.", word.Key, word.Value);
                }
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
                test.encrypeTwoLetterWord();

                test.addThreeLetterWords();
                test.addDoubleLetters();
                test.add3LastLetters();
                test.addTriGrams();
                test.addBiGrams();
              //  test.addRemainingFreq();
                Console.WriteLine("The table is : \n {0}", test.Table.ToString());
                test.encrypteByPossibilities();
             //   test.fillPossibilities();
                test.randomFill();
                Console.WriteLine("subs are: \n{0}", test.printSubstitutions());
                Console.WriteLine("The key is : \n {0}", test.printKey());
                Console.WriteLine("key length is: {0}", test.EncryptionKey.Count);
                Console.WriteLine("your grade is: {0}", test.calcGrade("The Wonderful Wizard of OZ.txt.key.txt", test.printKey()));
                // Console.WriteLine("the key of C is {0}", test.getKeyByValue(test.EncryptionKey,'C'));
                test.printMisMatches(fileName.Substring(0,fileName.Length - 11) + ".key.txt", test.printKey());
            }
            catch (IOException)
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
