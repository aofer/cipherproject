using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
namespace cipher
{
    public class Statistics
    {

       // private SortedList<char, int> _letterFreq;
        private List<StringIntPair> _oneLetterWordsSorted;
        private SortedList<char, int> _letterAppearances;
        private List<CharIntPair> _letterAppearancesSorted;
        private SortedList<char, int> _possiblyCapital;
        private SortedList<String, int> _twoLetterWords;
        private List<StringIntPair> _twoLetterWordsSorted;
        private SortedList<String, int> _threeLetterWords;
        private SortedList<String, int> _oneLetterWords;
        private SortedList<String, int> _lastThreeLetters;


        public List<StringIntPair> TwoLetterWordsSorted
        {
            get { return _twoLetterWordsSorted; }
            set { _twoLetterWordsSorted = value; }
        }
        public List<StringIntPair> OneLetterWordsSorted
        {
            get { return _oneLetterWordsSorted; }
            set { _oneLetterWordsSorted = value; }
        }

        public List<CharIntPair> LetterAppearancesSorted
        {
            get { return _letterAppearancesSorted; }
            set { _letterAppearancesSorted = value; }
        }

        public SortedList<char, int> LetterAppearances
        {
            get { return _letterAppearances; }
            set { _letterAppearances = value; }
        }

        public SortedList<String, int> TwoLetterWords
        {
            get { return _twoLetterWords; }
            set { _twoLetterWords = value; }
        }


        public SortedList<String, int> LastThreeLetters
        {
            get { return _lastThreeLetters; }
            set { _lastThreeLetters = value; }
        }


        public SortedList<String, int> ThreeLetterWords
        {
            get { return _threeLetterWords; }
            set { _threeLetterWords = value; }
        }


        public SortedList<String, int> OneLetterWords
        {
            get { return _oneLetterWords; }
            set { _oneLetterWords = value; }
        }

        public SortedList<char, int> PossiblyCapital
        {
            get { return _possiblyCapital; }
            set { _possiblyCapital = value; }
        }
        public Statistics()
        {
            //this._letterFreq = initFreq();
            this._letterAppearances = new SortedList<char, int>();
            this._letterAppearancesSorted = new List<CharIntPair>();
            this._twoLetterWords = new SortedList<String, int>();
            this._threeLetterWords = new SortedList<String, int>();
            this._lastThreeLetters = new SortedList<String, int>();
            this._possiblyCapital = new SortedList<char,int>();
            this._oneLetterWords = new SortedList<string, int>();
            this._oneLetterWordsSorted = new List<StringIntPair>();
            this._twoLetterWordsSorted = new List<StringIntPair>();
        }

        public SortedList<char, int> initFreq()
        {
            SortedList<char, int> res = new SortedList<char, int>();
            res['a'] = 80;

            return res;
        }

        public void getStatsFromFile(String filename)
        {
            
            TextReader tr = new StreamReader(filename);
            String text = tr.ReadToEnd();
            char[] sentenseDelim = { '.', '?', '!' };
            char[] wordDelim = { '.', '?', '!',' ', ';' ,',','\n','\r','\t','\"','+','-',':','*','=','_','[',']','{','}','(',')','/'};
            String[] sentenses = text.Split(sentenseDelim, StringSplitOptions.RemoveEmptyEntries);
            foreach (String sentense in sentenses)
            {
                String[] words = sentense.Split(wordDelim, StringSplitOptions.RemoveEmptyEntries);
                    if (checkIfLetter(words[0][0]))
                    {
                        if (words[0][0] == 'b' || words[0][0] == 'a')
                        {
        //                    Console.WriteLine("pause\n{0}\n{1}",sentense[0],sentense[sentense.Length-1]);
                        }
                    if (this._possiblyCapital.ContainsKey(words[0][0]))
                    {
                        this._possiblyCapital[words[0][0]]++;
                    }
                    else
                    {
                        this._possiblyCapital.Add(words[0][0],1);
                    }
                    }
                foreach (String word in words)
                {
                    getLetterAppearances(word);
                    twoLetterWordsCheck(word);
                    threeLetterWordsCheck(word);
                    checkLast3Letters(word);
                    oneLetterWordCheck(word);
                }
            }
            this._letterAppearancesSorted = this.sortByValue(this._letterAppearances);
            
        }
        private void checkLast3Letters(String word)
        {
            Regex last3Letters = new Regex("[a-zA-Z]{3}$");
            if (last3Letters.IsMatch(word))
            {
                if (!this._lastThreeLetters.ContainsKey(word))
                {
                    this._lastThreeLetters.Add(word, 1);
                }
                else
                {
                    this._lastThreeLetters[word]++;
                }
            }
        }
        // gets the 1 letter words (a and I)

        private void oneLetterWordCheck(String word)
        {
            Regex oneLetterWord = new Regex("^[a-zA-Z]{1}$");
            if (oneLetterWord.IsMatch(word))
            {
                if (!this._oneLetterWords.ContainsKey(word))
                {
                    this._oneLetterWords.Add(word, 1);
                }
                else
                {
                    this._oneLetterWords[word]++;
                }
            }
            this._oneLetterWordsSorted = sortByValue(this.OneLetterWords);

        }
        private void twoLetterWordsCheck(String word)
        {
            Regex twoLetterWord = new Regex("^[a-zA-Z]{2}$");
            if (twoLetterWord.IsMatch(word))
            {
                if (!this._twoLetterWords.ContainsKey(word))
                {
                    this._twoLetterWords.Add(word, 1);
                }
                else
                {
                    this._twoLetterWords[word]++;
                }
            }
            this._twoLetterWordsSorted = sortByValue(this._twoLetterWords);
        }
        private void threeLetterWordsCheck(String word)
        {
            Regex threeLetterWord = new Regex("^[a-zA-Z]{3}$");
            if (threeLetterWord.IsMatch(word))
                {
                    if (!this._threeLetterWords.ContainsKey(word))
                    {
                        this._threeLetterWords.Add(word, 1);
                    }
                    else
                    {
                        this._threeLetterWords[word]++;
                    }
                }
        }
        private void getLetterAppearances(String word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                char ch = word[i];
                if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
                {
                    if (this._letterAppearances.ContainsKey(ch))
                    {
                        int temp = this._letterAppearances[ch];
                        this._letterAppearances[ch] = temp + 1;
                    }
                    else
                    {
                        this._letterAppearances.Add(ch, 1);
                    }
                }

            }
        }
        public void PrintKeysAndValues(SortedList<char, int> myList)
        {
            foreach (KeyValuePair<char, int> kvp in myList)
            {
                Console.WriteLine("Letter = {0}, Number of appearances = {1}",
                    kvp.Key, kvp.Value);
            }

            Console.WriteLine();
            Console.ReadLine();
        }

        private List<CharIntPair> sortByValue(SortedList<char, int> list)
        {
            List<CharIntPair> res = new List<CharIntPair>();
            foreach (KeyValuePair<char, int> kvp in list)
            {
                res.Add(new CharIntPair(kvp.Key, kvp.Value));
            }
            res.Sort();
            res.Reverse();
            return res;

        }
        private List<StringIntPair> sortByValue(SortedList<String, int> list)
        {
            List<StringIntPair> res = new List<StringIntPair>();
            foreach (KeyValuePair<String, int> kvp in list)
            {
                res.Add(new StringIntPair(kvp.Key, kvp.Value));
            }
            res.Sort();
            res.Reverse();
            return res;
        }
        private bool checkIfLetter(char ch)
        {
            return (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        }

    }
}
