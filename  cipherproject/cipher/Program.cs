using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

// the main method of our algorithm
namespace CipherTextAttack
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
                Analysis an = new Analysis(c);
                TextWriter tw = new StreamWriter(fileName.Substring(0, fileName.Length - 4) + "_key.txt");
                tw.Write(an.printKey());
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
