using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherTextAttack
{
    // this class represent a string to int pair, sorted by the int value
    public class StringIntPair : IComparable
    {
        private String theKey;
        private int theValue;

        public String Key
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

        public StringIntPair(String key, int value)
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
                StringIntPair check = (StringIntPair)obj;
                return Value.CompareTo(check.Value);
            }
        }

    }
}


