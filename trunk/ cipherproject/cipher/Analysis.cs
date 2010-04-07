using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace cipher
{
    class Analysis
    {

        private PossibilitiesTable _table;   
        private Statistics _statistics;
        private SortedList<char, char> _encryptionKey;
        private List<char> _remainingLetters;
        public SortedList<char, char> EncryptionKey
        {
            get { return _encryptionKey; }
            set { _encryptionKey = value; }
        }
        public PossibilitiesTable Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public Analysis(Statistics _statistics)
        {
            this._table = new PossibilitiesTable();
            this._encryptionKey = new SortedList<char,char>();
            this._statistics = _statistics;
            this._remainingLetters = new List<char>();
            initLetters();
        }

        /**
         * Init the remaining letter list
         */
        private void initLetters(){
            for (char c = 'a'; c <= 'z'; c++)
                this._remainingLetters.Add(c);
            for (char c = 'A'; c <= 'Z'; c++)
                this._remainingLetters.Add(c);
        }

        /**
         * adding letters to the possiblilities table by their frequency
         */
        public void addLetterFreq()
        {
            char[]  freqArr= { 'a', 'o', 'i', 'n', 's', 'h', 'r', 'd'};
            this._encryptionKey.Add('e', this._statistics.LetterAppearancesSorted[0].Key);
            this._encryptionKey.Add('t', this._statistics.LetterAppearancesSorted[1].Key);
            this._remainingLetters.Remove(this._statistics.LetterAppearancesSorted[0].Key);
            this._remainingLetters.Remove(this._statistics.LetterAppearancesSorted[1].Key);
            for (int i = 2; i < 10; i++)
            {
                this._table.increaseGrade(freqArr[i-2], this._statistics.LetterAppearancesSorted[i].Key, 5);
            }    
        }

        /**
         * encrpte one l etter word
         */
        public void addOneLetterWord()
        {
            this._encryptionKey.Add('a', this._statistics.OneLetterWordsSorted[0].Key[0]);
            this._encryptionKey.Add('I', this._statistics.OneLetterWordsSorted[1].Key[0]);
   
        }

        /**
         * prints the substitutions
         */ 
        public string printSubstitutions()
        {
            string res = "";
            foreach (KeyValuePair<char, char> kvp in this._encryptionKey)
            {
                res = res + "letter : " + kvp.Key + " key: " + kvp.Value + "\n";
                
            }
            return res;
        }

        /**
         * fill in the remaining letters randomly
         */

        public void randomFill()
        {
            for (char ch = 'a'; ch <= 'z'; ch++)
            {
                if (!this._encryptionKey.ContainsKey(ch))
                {
                    Random rand = new Random();
                    int randIndex = rand.Next(0, this._remainingLetters.Count); //chooses a random letter to fill in the key
                    this._encryptionKey.Add(ch, this._remainingLetters[randIndex]);
                    this._remainingLetters.Remove(this._remainingLetters[randIndex]); //removes the random letter from the remaining letters
                }
            }
            for (char ch = 'A'; ch <= 'Z'; ch++)
            {
                if (!this._encryptionKey.ContainsKey(ch))
                {
                    Random rand = new Random();
                    int randIndex = rand.Next(0, this._remainingLetters.Count); //chooses a random letter to fill in the key
                    this._encryptionKey.Add(ch, this._remainingLetters[randIndex]);
                    this._remainingLetters.Remove(this._remainingLetters[randIndex]); //removes the random letter from the remaining letters
                }
            }
        }
        /**
         * prints the key as a long string
         */

        public String printKey()
        {
            String res = "";
            for (char ch = 'a'; ch <= 'z'; ch++)
            {
                res = res + this._encryptionKey[ch];
            }
            for (char ch = 'A'; ch <= 'Z'; ch++)
            {
                res = res + this._encryptionKey[ch];
            }
            return res;
        }

        /**
         * Calculate our grade
         */
        public float calcGrade(String fileName, String key)
        {
            float grade = 0;
            try
            {
                TextReader tr = new StreamReader(fileName);
                String correctKey = tr.ReadToEnd();
                for (int i = 0; i < 52; i++)
                {
                    if (key[i] == correctKey[i])
                    {
                        grade++;
                    }
                }
                grade = (grade / 52) * 100;
            }
            catch (IOException)
            {
                Console.WriteLine("file does not exist");
            }
            return grade;
        }

        /**
         * fill the encryption key by the possibilities table
         */
        public void encrypteByPossibilities()
        {
            for (char c = 'a'; c <= 'z'; c++)
            {
                if (!this._encryptionKey.ContainsKey(c))
                    if (mostPossibleLetter(this.Table.Table[c]) != '?')
                    {
                        this._encryptionKey[c] = mostPossibleLetter(this.Table.Table[c]);
                        this._remainingLetters.Remove(mostPossibleLetter(this.Table.Table[c]));
                    }
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (!this._encryptionKey.ContainsKey(c))
                    if (mostPossibleLetter(this.Table.Table[c]) != '?')
                    {
                        this._encryptionKey[c] = mostPossibleLetter(this.Table.Table[c]);
                        this._remainingLetters.Remove(mostPossibleLetter(this.Table.Table[c]));
                    }
            }

        }

        public char mostPossibleLetter(SortedList<char,int> gradingList)
        {
            int maxGrade = 0;
            char res='~';
            if (gradingList.Count == 0)
            {
                res = '?';
            }
            else
            {
                foreach (KeyValuePair<char, int> kvp in gradingList)
                {
                    if (kvp.Value > maxGrade)
                    {
                        maxGrade = kvp.Value;
                        res = kvp.Key;
                    }
                }
            }
            return res;
        }

    }
}
