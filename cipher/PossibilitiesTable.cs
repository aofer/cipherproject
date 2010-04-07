using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cipher
{
    class PossibilitiesTable
    {
        private SortedList<char, SortedList<char,int>> _table;


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
        public SortedList<char, SortedList<char, int>> Table
        {
            get { return _table; }
            set { _table = value; }
        }
        public void increaseGrade(char letter, char possibleSub, int value)
        {
            if (this._table[letter].ContainsKey(possibleSub)){
                this._table[letter][possibleSub] +=  value;
            }
            else{
                this._table[letter].Add(possibleSub,value);
            }

        }

        public void removeLetter(char letter, char possibleSub)
        {
            this._table[letter].Remove(possibleSub);
        }
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


    }
}
