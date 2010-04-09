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
                this._table.increaseGrade(freqArr[i-2], this._statistics.LetterAppearancesSorted[i].Key, 1);
            }    
        }

        /**
         * encrpte one letter word
         */
        public void addOneLetterWord()
        {
            this._encryptionKey.Add('a', this._statistics.OneLetterWordsSorted[0].Key[0]);
            this._remainingLetters.Remove(this._statistics.OneLetterWordsSorted[0].Key[0]);
            this._table.removeLetterList('a'); //remove the posibilities for letter a
            this._encryptionKey.Add('I', this._statistics.OneLetterWordsSorted[1].Key[0]);
            this._remainingLetters.Remove(this._statistics.OneLetterWordsSorted[1].Key[0]);
            this._table.removeLetterList('I');
   
        }
        public void addDoubleLetters()
        {
            String doubleL = this._statistics.DoubleLettersSorted[0].Key;
            this._encryptionKey['l'] = doubleL[0];
            this._remainingLetters.Remove(doubleL[0]);
        }
        public void addThreeLetterWords()
        {
            String encryptedThe = this._statistics.ThreeLetterWordsSorted[0].Key;
            String encryptedAnd = this._statistics.ThreeLetterWordsSorted[1].Key;
            String[] commonWords = {"for","are","but","not","you","all","any","can","had","her","was","one","our","out","has","him" };
            WordMatch match = new WordMatch("the", encryptedThe, _encryptionKey);
            foreach (KeyValuePair<char, char> kvp in match.Subs)
            {
                useWordMatch(match);
            }
            match = new WordMatch("and", encryptedAnd, _encryptionKey);
            foreach (KeyValuePair<char, char> kvp in match.Subs)
            {
                useWordMatch(match);
            }
            for (int i = 2; i < 18; i++)
            {
                foreach (String word in commonWords)
                {
                    match = new WordMatch(word, this._statistics.ThreeLetterWordsSorted[i].Key, _encryptionKey);
                    if (match.MatchPrecentage > 50)
                    {
                        useWordMatch(match);
                    }
                    else if (match.MatchPrecentage > 30)
                    {
                        insertMatchToTable(match, 1);
                    }
                }
            }
        }
        private void useWordMatch(WordMatch match)
        {
            foreach (KeyValuePair<char,char> kvp in match.Subs)
            {
                this._encryptionKey[kvp.Key] = kvp.Value;
                this._remainingLetters.Remove(kvp.Value);
            }
        }
        private void insertMatchToTable(WordMatch match,int grade)
        {
            foreach (KeyValuePair<char,char> kvp in match.Subs)
            {
                this._table.increaseGrade(kvp.Key, kvp.Value,grade);
                this._remainingLetters.Remove(kvp.Value);
            }
        }
        /**
         * encrpte two letter words
         */
        public void encrypeTwoLetterWord()
        {
            String mostCommon = this._statistics.TwoLetterWordsSorted[0].Key;
            String secondCommon = this._statistics.TwoLetterWordsSorted[1].Key;
            if (getKeyByValue(this._encryptionKey,mostCommon[0]) == 't')
            {
                this._encryptionKey.Add('o', mostCommon[1]);
                this._remainingLetters.Remove(mostCommon[1]);
                this._table.removeLetterList('o');
                if (getKeyByValue(this._encryptionKey, secondCommon[0]) == 'o')
                    this._encryptionKey.Add('f', secondCommon[1]);
                    this._remainingLetters.Remove(secondCommon[1]);
                    this._table.removeLetterList('f');
            }
            else if (getKeyByValue(this._encryptionKey, mostCommon[0]) == 'o')
            {
                this._encryptionKey.Add('f', mostCommon[1]);
                this._remainingLetters.Remove(mostCommon[1]);
                this._table.removeLetterList('f');
                if (getKeyByValue(this._encryptionKey, secondCommon[0]) == 't')
                    this._encryptionKey.Add('o', secondCommon[1]);
                    this._remainingLetters.Remove(secondCommon[1]);
                    this._table.removeLetterList('o');
            }
           
            String[] freqArr = { "in", "it", "is", "be", "as", "at", "so", "we"};
            /*for (int i = 0; i < 6; i++)
            {

                for (int j = 0; j < freqArr.Length; j++)
                {
                    float maxPrec = 0;
                    WordMatch w = new WordMatch(this._statistics.TwoLetterWordsSorted[i].Key, freqArr[j], _encryptionKey);
                    float precentage = w.MatchPrecentage;
                    if (precentage > maxPrec)
                    {
                        maxPrec = precentage;
                        rightWord = w;
                    }
                }
                for (int k = 0; k < rightWord.Subs.Count; k++)
                {
                    if(!this._encryptionKey.ContainsKey(rightWord.Subs.ElementAt(i).Key))
                        this._table.increaseGrade((rightWord.Subs.ElementAt(i)).Key, (rightWord.Subs.ElementAt(i)).Value, 1);
                }
                
            }*/
            for (int i = 2; i < 9; i++)//8 top two letters 
            {
                String word = this._statistics.TwoLetterWordsSorted[i].Key;
                if (this._encryptionKey.ContainsValue(word[0]))
                {
                    for (int j = 0; j < 8; j++)
                    {          
                        if (this.getKeyByValue(this._encryptionKey,word[0]) == freqArr[j][0])
                        {
                            this._table.increaseGrade(freqArr[j][1], word[1], 2);
                        }
                    }

                }
                if (this._encryptionKey.ContainsValue(word[1]))
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (this.getKeyByValue(this._encryptionKey, word[1]) == freqArr[j][1])
                        {
                            this._table.increaseGrade(freqArr[j][0], word[0], 2);
                        }
                    }

                }
            }
        }


        /**
         * returns the key by its value
         */ 
        public char getKeyByValue(SortedList<char, char>  lst,char c){
            int index = lst.IndexOfValue(c);
            return lst.ElementAt<KeyValuePair<char, char>>(index).Key;
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
                    if (this._remainingLetters.Count == 0)
                    {
                        this._encryptionKey[ch] = '?';
                        break;
                    }
                    this._encryptionKey[ch] =  this._remainingLetters[randIndex];
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
