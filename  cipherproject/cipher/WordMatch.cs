using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cipher
{
   /* this class will check if a word matches an ecrypted word. 
      both words will have the same length
      a match precentage will be given by comparing letters
      the SortedList _subs will hold all the new substitutions need to made in case we decide that the two word matches.
     */


    class WordMatch
    {
        //--------------------fields------------------------------------
        private String _word;
        private String _encryptedWord;
        private float _matchPrecentage;
        private SortedList<char, char> _subs;
    
        //----------------------getters----------------------------------------
        public String Word
        {
            get { return _word; }
        }


        public String EncryptedWord
        {
            get { return _encryptedWord; }
        }


        public float MatchPrecentage
        {
            get { return _matchPrecentage; }
        }


        public SortedList<char, char> Subs
        {
            get { return _subs; }
        }
        // constructor
        public WordMatch(String word, String encyptedWord, SortedList<char, char> currentKey)
        {
            this._word = word;
            this._encryptedWord = encyptedWord;
            this._subs = new SortedList<char, char>();
            this._matchPrecentage = 0;
            computeMatch(currentKey);
        }
        //initialize the wordMatch by calculating the precentage of matching letters and saving the required substitutions.
        private void computeMatch(SortedList<char,char> currentKey){
            if (this._word.Length == this._encryptedWord.Length)
            {
                float grade = 0;
                for (int i = 0;i< this._word.Length;i++)
                {
                    if (currentKey.ContainsKey(this._word[i]) &&currentKey[this._word[i]] == this._encryptedWord[i])
                    {
                        grade++;
                    }
                    else if (currentKey.ContainsKey(this._word[i]) && currentKey[this._word[i]] != this._encryptedWord[i])
                    {
                        this._subs = new SortedList<char, char>();
                        grade = 0;
                        break;
                    }
                    else
                    {
                        this._subs.Add(this._word[i], this._encryptedWord[i]);
                    }
                }
                this._matchPrecentage = (grade / (float)this._word.Length) * 100;
            }
        }
    }
}
