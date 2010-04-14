using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherTextAttack
{
    //this class represent the scoring table for the letters
    //by keeping a list of possibilities for each letter we can choose the best match for it.
    class PossibilitiesTable
    {
        private SortedList<char, SortedList<char,int>> _table;

        //constructor
        public PossibilitiesTable()
        {
            this._table = new SortedList<char,SortedList<char,int>>();
            for (char i = 'A'; i <= 'Z'; i++)
            {
                this._table.Add(i, new SortedList<char,int>());
            }
            for (char i = 'a'; i <= 'z'; i++)
            {
                this._table.Add(i, new SortedList<char, int>());
            }
        }
        //getter for the table
        public SortedList<char, SortedList<char, int>> Table
        {
            get { return _table; }
            set { _table = value; }
        }
        //increase or insert a letter possibility with grading into the table
        public void increaseGrade(char letter, char possibleSub, int value)
        {
            if (this._table[letter].ContainsKey(possibleSub)){
                this._table[letter][possibleSub] +=  value;
            }
            else{
                this._table[letter].Add(possibleSub,value);
            }

        }
        //removes a possibility from a letter
        public void removeLetter(char letter, char possibleSub)
        {
            this._table[letter].Remove(possibleSub);
        }
        //removes a letter from the table when we already found a match for it.
        public void removeLetterList(char letter)
        {
            this._table[letter] = new SortedList<char, int>();
        }

        //to string method
        public String ToString()
        {
            String res = "";
            for (char i = 'a'; i <= 'z';i++)
            {
                res = res + i + ":\n";
                foreach (KeyValuePair<char,int> kvp in this._table[i])
                {
                    res = res + "\t" + kvp.Key + "\t" + kvp.Value + "\n";
                }
            }
            for (char i = 'A'; i <= 'Z'; i++)
            {
                res = res + i + ":\n";
                foreach (KeyValuePair<char, int> kvp in this._table[i])
                {
                    res = res + "\t" + kvp.Key + "\t" + kvp.Value + "\n";
                }
            }
            return res;
        }
        //gets the best possible sub out of the table
        public KeyValuePair<char, int> getBestSub()
        {
            KeyValuePair<char, int> tKvp = new KeyValuePair<char, int>('?', 0);
            foreach (KeyValuePair<char, SortedList<char, int>> outter in this._table)
            {
                foreach (KeyValuePair<char, int> inner in outter.Value)
                {
                    if (inner.Value > tKvp.Value)
                    {
                        tKvp = inner;
                    }
                }
            }

            return tKvp;
        }


    }
}
