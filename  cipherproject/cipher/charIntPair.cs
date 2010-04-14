using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherTextAttack
{
    // this class represent a char to int pair, sorted by the int value
    public class CharIntPair : IComparable
    {
        private char theKey;
        private int theValue;

        public char Key
        {
            get
            {
                return theKey;
            }
        }

        public int Value
        {
            get
            {
                return theValue;
            }
        }

        public CharIntPair(char key, int value)
        {
            theKey = key;
            theValue = value;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            else
            {
                CharIntPair check = (CharIntPair)obj;
                return Value.CompareTo(check.Value);
            }
        }

    }
}


