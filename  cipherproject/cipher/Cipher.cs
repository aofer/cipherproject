using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
namespace cipher
{
    public class Cipher
    {

        public SortedList<char, int> _letterFreq;
        public SortedList<char, int> _letterAppearances;
        public ArrayList _letterAppearancesSorted;
        public List<char> _possiblyCapital;
        public SortedList<String, int> _twoLetterWords;
        public SortedList<String, int> _threeLetterWords;
        public SortedList<String, int> _oneLetterWords;
        public SortedList<String, int> _lastThreeLetters;
        
        public Cipher()
        {
            this._letterFreq = initFreq();
            this._letterAppearances = new SortedList<char, int>();
            this._letterAppearancesSorted = new ArrayList();
            this._twoLetterWords = new SortedList<String, int>();
            this._threeLetterWords = new SortedList<String, int>();
            this._lastThreeLetters = new SortedList<String, int>();
            this._possiblyCapital = new List<char>();
            this._oneLetterWords = new SortedList<string, int>();
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
            String[] allWords = text.Split();
            foreach (String word in allWords)
            {
                getLetterAppearances(word);
                twoLetterWordsCheck(word);
                threeLetterWordsCheck(word);
                checkLast3Letters(word);
                oneLetterWordCheck(word);
                

            }
            this._letterAppearancesSorted = this.getLettersByValues(this._letterAppearances);
            
        }
        public void checkLast3Letters(String word)
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
        public void oneLetterWordCheck(String word)
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
        public void twoLetterWordsCheck(String word)
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
        public void threeLetterWordsCheck(String word)
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
        public void getLetterAppearances(String word)
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
        public ArrayList getLettersByValues(SortedList<char, int> list)
        {
            ArrayList res = new ArrayList();
            foreach (KeyValuePair<char, int> kvp in list)
            {
                res.Add(new LetterAppearancePair(kvp.Key, kvp.Value));
            }
            res.Sort();
            res.Reverse();
            return res;

        }

    }
}
