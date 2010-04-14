using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace cipher
{
    //this class is the main algroithm for the cipher. it gets the text statistics and analyze the most common
    // words in the text in order to find a match for each letter.
    class Analysis
    {

        private PossibilitiesTable _table;   
        private Statistics _statistics;
        private SortedList<char, char> _encryptionKey;
        private List<char> _remainingLetters;

        //-------------getters/setters-----------------------------------------
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
        //constructor
        public Analysis(Statistics _statistics)
        {
            this._table = new PossibilitiesTable();
            this._encryptionKey = new SortedList<char,char>();
            this._statistics = _statistics;
            this._remainingLetters = new List<char>();
            initLetters();
            analyze();
        }
        //analayze the statistics and produces a key
        private void analyze()
        {
            addLetterFreq();
            addOneLetterWord();
            mostPossibleLetter(this._table.Table['a']);
            addTwoLetterWords();
            add3LastLetters();
            addThreeLetterWords();
            addDoubleLetters();
            addTriGrams();
            addBiGrams();
            addFourLetterWords();
            encrypteByMaxPossibilities();//encrypte the lower case letters
            refreshTable();
            add7LetterWordUpper();
            add6LetterWordUpper();
            add5LetterWordUpper();
            add4LetterWordUpper();
            add3LetterWordUpper();
            encrypteByMaxPossibilities();
            randomFill();
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

        //used for debugging
        public String printTempkey()
        {
            String res = "";
            foreach (KeyValuePair<char, char> kvp in _encryptionKey)
            {
                res = res + "real: " + kvp.Key + "  encrypted: " + kvp.Value + "\n" ;
            }
            return res;
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
            refreshTable();
        }

        /**
         * encrpte one letter word
         */
        public void addOneLetterWord()
        {
            if (this._statistics.OneLetterWordsSorted.Count() > 0)
            {
                this._table.increaseGrade('a', this._statistics.OneLetterWordsSorted[0].Key[0], 2);
            }
            if (this._statistics.OneLetterWordsSorted.Count() > 1)
            {
                this._table.increaseGrade('I', this._statistics.OneLetterWordsSorted[1].Key[0], 2);
            }
            if (this._statistics.OneLetterWordsSorted.Count() > 1)
            {
                this._table.increaseGrade('A', this._statistics.OneLetterWordsSorted[2].Key[0], 1);
            }
            refreshTable();  
        }
        //add the most common double letter combinations to the table
        public void addDoubleLetters()
        {
            String doubleL = this._statistics.DoubleLettersSorted[0].Key;
            this._table.increaseGrade('l', doubleL[0], 4);
            refreshTable();
            char[] commonDoubles = { 'l', 'e', 'o', 't', 's', 'r', 'p', 'f', 'n', 'd', 'g', 'm' };
            int size = Math.Min(10, this._statistics.DoubleLettersSorted.Count());
            for (int i = 0; i < size; i++)
            {
                foreach (char ch in commonDoubles)
                {
                    this._table.increaseGrade(ch, this._statistics.DoubleLettersSorted[i].Key[0], 2);
                }
            }
            refreshTable();
        }
        //add the most common biGrams to the table
        public void addBiGrams()
        {
            String[] commonBiGrams = {"th","he","in","er","an","re","nd","on","en","at","ou","ed","ha","to","or","it","is","hi","as","ng","ve"};
            WordMatch match = null;
            foreach (String word in commonBiGrams)
            {
                int size = Math.Min(25, this._statistics.BigramsSorted.Count());
                for (int i = 0; i < size; i++)
                {
                    match = new WordMatch(word, this._statistics.BigramsSorted[i].Key, _encryptionKey);
                    if (match.MatchPrecentage > 0)
                    {
                        insertMatchToTable(match,1);
                    }
                }

            }
            refreshTable();

        }
        //add the most commong 3 letter endings to the table
        public void add3LastLetters()
        {
            String tIng = this._statistics.LastThreeLettersSorted[0].Key;
            WordMatch match = new WordMatch("ing", tIng, _encryptionKey);
            foreach (KeyValuePair<char, char> kvp in match.Subs)
            {
                insertMatchToTable(match, 2);
            }
            String[] commonEndings = { "ing", "hat", "ere", "gth", "ted", "ith", "red", "ent", "ion", "aid", "nce", "ter", "uld", "ess", "ore", "ave", "ver", "rom", "ned", "hen", "ick" };
            int size = Math.Min(25, this._statistics.LastThreeLettersSorted.Count());
            foreach (String str in commonEndings)
            {
                for (int i = 0; i < size; i++)
                {
                    match = new WordMatch(str, this._statistics.LastThreeLettersSorted[i].Key, _encryptionKey);
                    if (match.MatchPrecentage > 50)
                    {
                        insertMatchToTable(match, 2);
                    }
                    else if (match.MatchPrecentage > 0)
                    {
                        insertMatchToTable(match, 1);
                    }
                }
            }
            refreshTable();
        }
        //add the most common TriGrams to the table
        public void addTriGrams()
        {
            WordMatch match = null;
            String[] commonTriGrams = { "hat", "and","tha","ent","ion","tio","for","nde","has","nce","edt","tis","oft","sth","men","you","wit","thi","all","was","ver" };
            int size = Math.Min(40, this._statistics.TrigramsSorted.Count());
            for (int i = 0; i < size; i++)
            {
                foreach (String word in commonTriGrams)
                {
                    match = new WordMatch(word, this._statistics.TrigramsSorted[i].Key, _encryptionKey);
                    if (match.MatchPrecentage > 50)
                    {
                        insertMatchToTable(match, 2);
                    }
                    else if (match.MatchPrecentage > 30)
                    {
                        insertMatchToTable(match, 1);
                    }
                }
            }
            refreshTable();
        }
        //analyze the  most common four letter words and add gradings to the table
        public void addFourLetterWords()
        {
            String[] commonWords = { "make", "like", "take", "such", "much", "from", "some", "them", "just", "very", "that", "with" };
            WordMatch match = null;
            int size = Math.Min(100, this._statistics.FourLetterWordsSorted.Count);
            for (int i = 0; i < size; i++)
            {
                foreach (String word in commonWords)
                {
                    match = new WordMatch(word, this._statistics.FourLetterWordsSorted[i].Key, _encryptionKey);
                    if (match.MatchPrecentage > 70)
                    {
                        insertMatchToTable(match, 3);
                    }
                    else if (match.MatchPrecentage > 0){
                        insertMatchToTable(match,1);
                    }
                }
            }
            refreshTable();

        }
        //analyze the  most common three letter words and add gradings to the table
        public void addThreeLetterWords()
        {
            String encryptedThe = this._statistics.ThreeLetterWordsSorted[0].Key;
            String encryptedAnd = this._statistics.ThreeLetterWordsSorted[1].Key;
            String[] commonWords = {"for","are","but","not","you","all","any","can","had","her","was","one","our","out","has","him",
                                       "the","and","day","get","his","how" };
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
            refreshTable();
            int size = Math.Min(30, this._statistics.ThreeLetterWordsSorted.Count());
            for (int i = 2; i < size; i++)
            {
                foreach (String word in commonWords)
                {
                    match = new WordMatch(word, this._statistics.ThreeLetterWordsSorted[i].Key, _encryptionKey);
                    if (match.MatchPrecentage > 50)
                    {
                        insertMatchToTable(match, 2);
                    }
                    else if (match.MatchPrecentage > 30)
                    {
                        insertMatchToTable(match, 1);
                    }
                }
            }
            refreshTable();
        }
        //iteretes over the match substitutions list and puts the letters in the key
        private void useWordMatch(WordMatch match)
        {
            foreach (KeyValuePair<char,char> kvp in match.Subs)
            {
                this._encryptionKey[kvp.Key] = kvp.Value;
                this._remainingLetters.Remove(kvp.Value);

            }
        }
        //iterates over the match substitutions and insert gradings to the table
        private void insertMatchToTable(WordMatch match,int grade)
        {
            foreach (KeyValuePair<char,char> kvp in match.Subs)
            {
                this._table.increaseGrade(kvp.Key, kvp.Value,grade);
            }
        }
        //analyze the  most common two letter words and add gradings to the table
        public void addTwoLetterWords()
        {
            String[] commonWords = { "of", "to", "in", "it", "is", "be", "as", "at", "so", "we", "he", "by", "or", "on", "do", "if", "me", "my", "up", "an", "go", "no", "us", "am" }; 
            WordMatch match = null;
            int size = Math.Min(5, this._statistics.TwoLetterWordsSorted.Count());
            //finding "to"
            for (int i = 0; i < size; i++)
            {
                match = new WordMatch("to", this._statistics.TwoLetterWordsSorted[i].Key, _encryptionKey);
                if (match.MatchPrecentage == 50)
                {
                    useWordMatch(match);
                    break;
                }
            }
            //finding "of"
            for (int i = 0; i < size; i++)
            {
                match = new WordMatch("of", this._statistics.TwoLetterWordsSorted[i].Key, _encryptionKey);
                if (match.MatchPrecentage == 50)
                {
                    useWordMatch(match);
                    break;
                }
            }
            size = Math.Min(20, this._statistics.TwoLetterWordsSorted.Count());
            foreach (String str in commonWords){
                for (int i = 0; i < size; i++)
                {
                    match = new WordMatch(str, this._statistics.TwoLetterWordsSorted[i].Key, _encryptionKey);
                    if (match.MatchPrecentage == 50)
                    {
                        insertMatchToTable(match, 1);
                    }
                }
            }
            refreshTable();

        }

        /**
         * prints the substitutions, used for debugging
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
         * Calculate our grade , used for debugging
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
        //used for debugging and statistics analysis, prints all the mismatches
        public void printMisMatches(String fileName,String key)
        {
            try
            {
                TextReader tr = new StreamReader(fileName);
                String correctKey = tr.ReadToEnd();
                char letter = 'a';
                for (int i = 0; i < 52; i++)
                {
                    if (correctKey[i] != key[i])
                    {
                        Console.WriteLine("letter: {0} should have got: {1} but got {2} instead.",letter, correctKey[i], key[i]);
                    }
                    if (letter == 'z')
                    {
                        letter = 'A';
                    }
                    else
                    {
                        letter++;
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("file does not exist");
            }
        }



        /**
         * fill the encryption key by the possibilities table, from the highest grade to the lowest
         * old method. not used by this algorithm.
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
                        refreshTable();
                    }
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (!this._encryptionKey.ContainsKey(c))
                    if (mostPossibleLetter(this.Table.Table[c]) != '?')
                    {
                        this._encryptionKey[c] = mostPossibleLetter(this.Table.Table[c]);
                        this._remainingLetters.Remove(mostPossibleLetter(this.Table.Table[c]));
                        refreshTable();
                    }
            }

        }
        //gets the given letter's best possible substitution.
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

        //fill the encryption key by the possibilities table, from the highest grade to the lowest
        public void encrypteByMaxPossibilities()
        {
            int counter = this._remainingLetters.Count;
            for (int k = 0; k < counter ; k++)
            {
                char letter = '?';
                KeyValuePair<char, int> tMax = new KeyValuePair<char, int>('!', 0);
                for (int i = 0; i < this._table.Table.Count; i++)
                {
                    SortedList<char, int> tList = this._table.Table.ElementAt<KeyValuePair<char, SortedList<char, int>>>(i).Value;
                    if (tList.Count != 0)
                    {
                        for (int j = 0; j < tList.Count; j++)
                        {
                            KeyValuePair<char, int> tPair = tList.ElementAt<KeyValuePair<char, int>>(j);
                            if (tPair.Value > tMax.Value)
                            {
                                tMax = tPair;
                                letter = this._table.Table.ElementAt<KeyValuePair<char, SortedList<char, int>>>(i).Key;
                            }
                        }
                    }
                }
                this._encryptionKey[letter] = tMax.Key;
                this._remainingLetters.Remove(tMax.Key);
                this._encryptionKey.Remove('?');
                refreshTable();
            }
        }

        //used for debugging and finding the repeating char
        public void printKeyErrors(String key)
        {
            for (char i = 'a'; i <= 'z'; i++)
            {
                int counter = 0;
                for (int j = 0; j < key.Length; j++)
                {
                    if (i == key[j])
                    {
                        counter++;
                    }
                    if (counter > 1)
                    {
                        Console.WriteLine("error found: {0}", i);
                    }
                }
            }
            for (char i = 'A'; i <= 'Z'; i++)
            {
                int counter = 0;
                for (int j = 0; j < key.Length; j++)
                {
                    if (i == key[j])
                    {
                        counter++;
                    }
                    if (counter > 1)
                    {
                        Console.WriteLine("error found: {0}", i);
                    }
                }
            }
        }

        //also used for debugging the key
        public void printMissingLetter()
        {
            for (char ch = 'a'; ch <= 'z'; ch++)
            {
                if (!_encryptionKey.ContainsValue(ch))
                {
                    Console.WriteLine("letter : {0} is missing", ch);
                }
            }
            for (char ch = 'A'; ch <= 'Z'; ch++)
            {
                if (!_encryptionKey.ContainsValue(ch))
                {
                    Console.WriteLine("letter : {0} is missing", ch);
                }
            }
            if (this._remainingLetters.Count != 0)
            {
                Console.WriteLine("remaining letters still contains: {0}", this._remainingLetters[0]);
            }
        }
        //used for refreshing the table in order to remove letters that were already substituted
        public void refreshTable()
        {
            //foreach (KeyValuePair<char, SortedList<char,int>> kvp in this._table.Table)
            for (char j = 'a' ; j <= 'z';j++)
            {
                if (_encryptionKey.ContainsKey(j))
                {
                    this._table.removeLetterList(j);
                }
                else
                {
                    if (_table.Table[j].Count > 0)
                    {
                        for (int i = 0; i < _table.Table[j].Count; i++)
                        {
                            if (!this._remainingLetters.Contains(_table.Table[j].ElementAt<KeyValuePair<char, int>>(i).Key))
                            {
                                this._table.removeLetter(j, _table.Table[j].ElementAt<KeyValuePair<char, int>>(i).Key);
                            }
                        }
                    }
                }
            }
            for (char j = 'A'; j <= 'Z'; j++)
            {
                if (_encryptionKey.ContainsKey(j))
                {
                    this._table.removeLetterList(j);
                }
                else
                {
                    if (_table.Table[j].Count > 0)
                    {
                        for (int i = 0; i < _table.Table[j].Count; i++)
                        {
                            if (!this._remainingLetters.Contains(_table.Table[j].ElementAt<KeyValuePair<char, int>>(i).Key))
                            {
                                this._table.removeLetter(j, _table.Table[j].ElementAt<KeyValuePair<char, int>>(i).Key);
                            }
                        }
                    }
                }
            }
        }
        //analyze 8 letter words in order to find capital letters
        public void add8LetterWordUpper()
        {
            for (int i = 0; i < this._statistics.SevenLetterWordsSorted.Count; i++)
            {
                String word1 = this._statistics.SevenLetterWordsSorted.ElementAt<StringIntPair>(i).Key;
                for (int j = 0; j < this._statistics.SevenLetterWordsSorted.Count; j++)
                {
                    String word2 = this._statistics.SevenLetterWordsSorted.ElementAt<StringIntPair>(j).Key;
                    if (word1.Substring(word1.Length - 7, 7) == word2.Substring(word2.Length - 7, 7))
                    {
                        Char firstLetter1 = word1[0];
                        Char firstLetter2 = word2[0];
                        if (this._encryptionKey.ContainsValue(firstLetter1))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter1)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter2))
                                this._table.increaseGrade(upperCase, firstLetter2, 8);
                        }
                        else if (this._encryptionKey.ContainsValue(firstLetter2))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter2)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter1))
                                this._table.increaseGrade(upperCase, firstLetter1, 8);
                        }
                    }

                }
            }
            refreshTable();
        }
        //analyze 7 letter words in order to find capital letters
        public void add7LetterWordUpper()
        {
            for (int i = 0; i < this._statistics.SevenLetterWordsSorted.Count; i++)
            {
                String word1 = this._statistics.SevenLetterWordsSorted.ElementAt<StringIntPair>(i).Key;
                for (int j = 0; j < this._statistics.SevenLetterWordsSorted.Count; j++)
                {
                    String word2 = this._statistics.SevenLetterWordsSorted.ElementAt<StringIntPair>(j).Key;
                    if (word1.Substring(word1.Length - 6, 6) == word2.Substring(word2.Length - 6, 6))
                    {
                        Char firstLetter1 = word1[0];
                        Char firstLetter2 = word2[0];
                        if (this._encryptionKey.ContainsValue(firstLetter1))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter1)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter2))
                                this._table.increaseGrade(upperCase, firstLetter2, 7);
                        }
                        else if (this._encryptionKey.ContainsValue(firstLetter2))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter2)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter1))
                                this._table.increaseGrade(upperCase, firstLetter1, 7);
                        }
                    }

                }
            }
            refreshTable();
        }

        //analyze 6 letter words in order to find capital letters
        public void add6LetterWordUpper()
        {
            for (int i = 0; i < this._statistics.SixLetterWordsSorted.Count; i++)
            {
                String word1 = this._statistics.SixLetterWordsSorted.ElementAt<StringIntPair>(i).Key;
                for (int j = 0; j < this._statistics.SixLetterWordsSorted.Count; j++)
                {
                    String word2 = this._statistics.SixLetterWordsSorted.ElementAt<StringIntPair>(j).Key;
                    if (word1.Substring(word1.Length - 5, 5) == word2.Substring(word2.Length - 5, 5))
                    {
                        Char firstLetter1 = word1[0];
                        Char firstLetter2 = word2[0];
                        if (this._encryptionKey.ContainsValue(firstLetter1))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter1)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter2)) 
                                this._table.increaseGrade(upperCase, firstLetter2, 2);
                        }
                        else if (this._encryptionKey.ContainsValue(firstLetter2))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter2)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter1)) 
                                this._table.increaseGrade(upperCase, firstLetter1, 2);
                        }
                    }

                }
            }
            refreshTable();
        }

        //analyze 5 letter words in order to find capital letters
        public void add5LetterWordUpper()
        {
            for (int i = 0; i < this._statistics.FiveLetterWordsSorted.Count; i++)
            {
                String word1 = this._statistics.FiveLetterWordsSorted.ElementAt<StringIntPair>(i).Key;
                for (int j = 0; j < this._statistics.FiveLetterWordsSorted.Count; j++)
                {
                    String word2 = this._statistics.FiveLetterWordsSorted.ElementAt<StringIntPair>(j).Key;
                    if (word1.Substring(word1.Length - 4, 4) == word2.Substring(word2.Length - 4, 4))
                    {
                        Char firstLetter1 = word1[0];
                        Char firstLetter2 = word2[0];
                        if (this._encryptionKey.ContainsValue(firstLetter1))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter1)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter2)) 
                                this._table.increaseGrade(upperCase, firstLetter2, 2);
                        }
                        else if (this._encryptionKey.ContainsValue(firstLetter2))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter2)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter1)) 
                                this._table.increaseGrade(upperCase, firstLetter1, 2);
                        }
                    }

                }
            }
            refreshTable();
        }

        //analyze 4 letter words in order to find capital letters
        public void add4LetterWordUpper()
        {
            for (int i = 0; i < this._statistics.FourLetterWordsSorted.Count; i++)
            {
                String word1 = this._statistics.FourLetterWordsSorted.ElementAt<StringIntPair>(i).Key;
                for (int j = 0; j < this._statistics.FourLetterWordsSorted.Count; j++)
                {
                    String word2 = this._statistics.FourLetterWordsSorted.ElementAt<StringIntPair>(j).Key;
                    if( word1.Substring(word1.Length-3,3) == word2.Substring(word2.Length-3,3)){
                        Char firstLetter1 = word1[0];
                        Char firstLetter2 = word2[0];
                        if (this._encryptionKey.ContainsValue(firstLetter1))
                         {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char,char>>(this._encryptionKey.IndexOfValue(firstLetter1)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter2)) 
                                this._table.increaseGrade(upperCase, firstLetter2, 1);
                         }
                        else if (this._encryptionKey.ContainsValue(firstLetter2))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter2)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter1)) 
                                this._table.increaseGrade(upperCase, firstLetter1, 1);
                        }
                    }

                }
            }
            refreshTable();
        }
        //analyze 3 letter words in order to find capital letters
        public void add3LetterWordUpper()
        {
            for (int i = 0; i < this._statistics.ThreeLetterWordsSorted.Count; i++)
            {
                String word1 = this._statistics.ThreeLetterWordsSorted.ElementAt<StringIntPair>(i).Key;
                for (int j = 0; j < this._statistics.ThreeLetterWordsSorted.Count; j++)
                {
                    String word2 = this._statistics.ThreeLetterWordsSorted.ElementAt<StringIntPair>(j).Key;
                    if (word1.Substring(word1.Length - 2, 2) == word2.Substring(word2.Length - 2, 2))
                    {
                        Char firstLetter1 = word1[0];
                        Char firstLetter2 = word2[0];
                        if (this._encryptionKey.ContainsValue(firstLetter1))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter1)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter2)) 
                                this._table.increaseGrade(upperCase, firstLetter2, 1);
                        }
                        else if (this._encryptionKey.ContainsValue(firstLetter2))
                        {
                            Char upperCase = char.ToUpper(this._encryptionKey.ElementAt<KeyValuePair<char, char>>(this._encryptionKey.IndexOfValue(firstLetter2)).Key);
                            if (!this._encryptionKey.ContainsValue(firstLetter1)) 
                                this._table.increaseGrade(upperCase, firstLetter1, 1);
                        }
                    }

                }
            }
            refreshTable();
        }

    }
}
