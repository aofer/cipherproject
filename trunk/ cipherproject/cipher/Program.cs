using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

// the main method of our algorithm
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
                foreach (StringIntPair word in c.FourLetterWordsSorted)
                {
                    if (counter++ > 50)
                    {
                        break;
                    }
                    Console.WriteLine(" 4 letter word is: {0} , appears {1} times.", word.Key, word.Value);
                }
                Analysis test = new Analysis(c);

                Console.WriteLine("subs are: \n{0}", test.printSubstitutions());
                Console.WriteLine("The key is : \n {0}", test.printKey());
                Console.WriteLine("key length is: {0}", test.EncryptionKey.Count);
                Console.WriteLine("your grade is: {0}", test.calcGrade(fileName.Substring(0, fileName.Length - 11) + ".key.txt", test.printKey()));
                test.printMissingLetter();
                test.printMisMatches(fileName.Substring(0,fileName.Length - 11) + ".key.txt", test.printKey());
                test.printKeyErrors(test.printKey());
                TextWriter tw = new StreamWriter(fileName + "_key.txt");
                tw.Write(test.printKey());
                tw.Flush();
                tw.Close();
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
