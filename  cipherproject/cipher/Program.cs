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
                foreach (StringIntPair word in c.FourLetterWordsSorted)
                {
                    if (counter++ > 50)
                    {
                        break;
                    }
                    Console.WriteLine(" 4 letter word is: {0} , appears {1} times.", word.Key, word.Value);
                }
                Console.ReadLine();
                counter = 0;
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
                    Console.WriteLine("last three letters are: {0} , appears {1} times.", word.Key, word.Value);
                    if (counter++ > 100)
                    {
                        break;
                    }

                }
                Console.ReadLine();
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
                counter = 0;
                foreach (StringIntPair word in c.FourLetterWordsSorted)
                {
                    Console.WriteLine("word is: {0} , appears {1} times.", word.Key, word.Value);
                    if (counter++ > 100)
                    {
                        break;
                    }

                }
                Console.ReadLine();
                counter = 0;
                foreach (StringIntPair word in c.FiveLetterWordsSorted)
                {
                    Console.WriteLine("5 word is: {0} , appears {1} times.", word.Key, word.Value);
                    if (counter++ > 100)
                    {
                        break;
                    }

                }
                Console.ReadLine();
                counter = 0;
                foreach (StringIntPair word in c.SixLetterWordsSorted)
                {
                    Console.WriteLine("6 word is: {0} , appears {1} times.", word.Key, word.Value);
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


                Analysis test = new Analysis(c);


                test.addLetterFreq();
                test.addOneLetterWord();
                //
                test.mostPossibleLetter(test.Table.Table['a']);
                //
                //test.encrypeTwoLetterWord();
                test.addTwoLetterWords();
                test.add3LastLetters();
                test.addThreeLetterWords();
                test.addDoubleLetters();
                test.addTriGrams();
                test.addBiGrams();
                test.addFourLetterWords();


                Console.WriteLine(test.Table.ToString());
                Console.ReadLine();
                test.encrypteByMaxPossibilities();//encrypte the small letters
                test.add7LetterWordUpper();
                test.add6LetterWordUpper();
                test.add5LetterWordUpper();
                test.add4LetterWordUpper();
                test.add3LetterWordUpper();
                Console.WriteLine("The table is : \n {0}", test.Table.ToString());
               // test.encrypteByPossibilities();
                test.encrypteByMaxPossibilities();  
                //  test.fillPossibilities();
                Console.WriteLine("temp key is:");
                Console.WriteLine(test.printTempkey());
                test.randomFill();
                Console.WriteLine("subs are: \n{0}", test.printSubstitutions());
                Console.WriteLine("The key is : \n {0}", test.printKey());
                Console.WriteLine("key length is: {0}", test.EncryptionKey.Count);
                Console.WriteLine("your grade is: {0}", test.calcGrade(fileName.Substring(0, fileName.Length - 11) + ".key.txt", test.printKey()));
                test.printMissingLetter();
                test.printMisMatches(fileName.Substring(0,fileName.Length - 11) + ".key.txt", test.printKey());
                TextWriter tw = new StreamWriter(fileName + "_key.txt");
                tw.Write(test.printKey());
                tw.Flush();
                tw.Close();
            }
            catch (IOException)
            {
                Console.WriteLine("file does not exist");
            }
      //      catch (IndexOutOfRangeException)
       ////     {
        //        Console.WriteLine("mising argument\n");
         //   }
        }


    }


}
