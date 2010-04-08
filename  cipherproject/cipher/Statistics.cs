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
//-----------------------fields-------------------------------------------------------
       // private SortedList<char, int> _letterFreq;

        private SortedList<char, int> _letterAppearances;
        private List<CharIntPair> _letterAppearancesSorted;
        
        private SortedList<char, int> _possiblyCapital;
        private List<CharIntPair> _possiblyCapitalSorted;
        
        private SortedList<String, int> _twoLetterWords;
        private List<StringIntPair> _twoLetterWordsSorted;
        
        private SortedList<String, int> _threeLetterWords;
        private List<StringIntPair> _threeLetterWordsSorted;
        
        private SortedList<String, int> _oneLetterWords;
        private List<StringIntPair> _oneLetterWordsSorted;

        private SortedList<String, int> _lastThreeLetters;
        private List<StringIntPair> _lastThreeLettersSorted;

        private SortedList<String, int> _bigrams;
        private List<StringIntPair> _bigramsSorted;

        private SortedList<String, int> _trigrams;
        private List<StringIntPair> _trigramsSorted;

        private SortedList<String, int> _doubleLetters;
        private List<StringIntPair> _doubleLettersSorted;


        private String _filename;
//----------------------getters/setters------------------------------------------------

        public List<StringIntPair> DoubleLettersSorted
        {
            get { return _doubleLettersSorted; }
        }
        public List<StringIntPair> TrigramsSorted
        {
            get { return _trigramsSorted; }
        }
        public List<StringIntPair> BigramsSorted
        {
            get { return _bigramsSorted; }
        }
        public List<StringIntPair> LastThreeLettersSorted
        {
            get { return _lastThreeLettersSorted; }
        }
        public List<CharIntPair> PossiblyCapitalSorted
        {
            get { return _possiblyCapitalSorted; }
        }
        public List<StringIntPair> ThreeLetterWordsSorted
        {
            get { return _threeLetterWordsSorted; }
        }
        public List<StringIntPair> TwoLetterWordsSorted
        {
            get { return _twoLetterWordsSorted; }
        }
        public List<StringIntPair> OneLetterWordsSorted
        {
            get { return _oneLetterWordsSorted; }
        }

        public List<CharIntPair> LetterAppearancesSorted
        {
            get { return _letterAppearancesSorted; }
        }



        public Statistics(String filename)
        {
            //this._letterFreq = initFreq();
            this._filename = filename;
            this._letterAppearances = new SortedList<char, int>();
            this._letterAppearancesSorted = new List<CharIntPair>();
            this._twoLetterWords = new SortedList<String, int>();
            this._threeLetterWords = new SortedList<String, int>();
            this._lastThreeLetters = new SortedList<String, int>();
            this._lastThreeLettersSorted = new List<StringIntPair>();
            this._possiblyCapital = new SortedList<char,int>();
            this._possiblyCapitalSorted = new List<CharIntPair>();
            this._oneLetterWords = new SortedList<string, int>();
            this._oneLetterWordsSorted = new List<StringIntPair>();
            this._twoLetterWordsSorted = new List<StringIntPair>();
            this._bigrams = new SortedList<string, int>();
            this._bigramsSorted = new List<StringIntPair>();
            this._trigrams = new SortedList<String, int>();
            this._trigramsSorted = new List<StringIntPair>();
            this._doubleLetters = new SortedList<string, int>();
            this._doubleLettersSorted = new List<StringIntPair>();
            initStatistics(this._filename);
        }

        public SortedList<char, int> initFreq()
        {
            SortedList<char, int> res = new SortedList<char, int>();
            res['a'] = 80;

            return res;
        }

        public void initStatistics(String filename)
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
                    findBiGrams(word);
                    findTriGrams(word);
                    findDoubleLetters(word);
                }
            }
            //sort all the statistics by appearances
            this._twoLetterWordsSorted = sortByValue(this._twoLetterWords);
            this._letterAppearancesSorted = this.sortByValue(this._letterAppearances);
            this._oneLetterWordsSorted = sortByValue(this._oneLetterWords);
            this._threeLetterWordsSorted = sortByValue(this._threeLetterWords);
            this._possiblyCapitalSorted = sortByValue(this._possiblyCapital);
            this._lastThreeLettersSorted = sortByValue(this._lastThreeLetters);
            this._bigramsSorted = sortByValue(this._bigrams);
            this._trigramsSorted = sortByValue(this._trigrams);
            this._doubleLettersSorted = sortByValue(this._doubleLetters);
        }
        private void checkLast3Letters(String word)
        {
            Regex last3Letters = new Regex("[a-zA-Z]{3}$");
            if (last3Letters.IsMatch(word) && word.Length > 3)
            {
                String ending = word.Substring(word.Length - 3, 3);
                if (!this._lastThreeLetters.ContainsKey(ending))
                {
                    this._lastThreeLetters.Add(ending, 1);
                }
                else
                {
                    this._lastThreeLetters[ending]++;
                }
            }
        }
        private void findDoubleLetters(String word)
        {
            if (word.Length >= 2)
            {
                for (int i = 0; i < word.Length - 1; i++)
                {
                    String pair = word.Substring(i, 2);
                    if (pair[0] == pair[1] && checkIfLetter(pair[0]))
                    {
                        if (this._doubleLetters.ContainsKey(pair))
                        {
                            this._doubleLetters[pair]++;
                        }
                        else
                        {

                            this._doubleLetters.Add(pair, 1);
                        }
                    }
                }
            }
        }
        private String[] findNgrams(String word, int n)
        {
            String[] res = new String[word.Length - (n - 1)];
            for (int i = 0; i < word.Length - (n - 1); i++)
            {
                res[i] = word.Substring(i, n);
            }
            return res;
        }
        private void findBiGrams(String word)
        {
            if (word.Length >= 2)
            {
                String[] bigrams = findNgrams(word, 2);
                foreach (String str in bigrams)
                {
                    if (this._bigrams.ContainsKey(str))
                    {
                        this._bigrams[str]++;
                    }
                    else
                    {
                        this._bigrams.Add(str, 1);
                    }
                }
            }
        }
        private void findTriGrams(String word)
        {
            if (word.Length >= 2)
            {
                String[] trigrams = findNgrams(word, 2);
                foreach (String str in trigrams)
                {
                    if (this._trigrams.ContainsKey(str))
                    {
                        this._trigrams[str]++;
                    }
                    else
                    {
                        this._trigrams.Add(str, 1);
                    }
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
        private void getNGram(String word,int length){

        }

    }
}
