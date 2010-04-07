using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cipher
{
    public struct LetterAppearancePair : IComparable
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

        public LetterAppearancePair(char key, int value)
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
                LetterAppearancePair check = (LetterAppearancePair)obj;
                return Value.CompareTo(check.Value);
            }
        }
    }
}
