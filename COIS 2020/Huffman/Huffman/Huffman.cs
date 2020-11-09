using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PriorityQueue;
/// <summary>
/// This is assignment 2. COIS 2020H. Huffman.
/// 
/// authors: Yllka Bokju and  Sam ...
/// </summary>
namespace HuffmanProgram
{
    //-----------------------------------------------------------------------
    /// <summary>
    /// Class name: Node
    /// description: creates a node for the tree. it also implements CompareTo method.
    /// </summary>
    public class Node : IComparable
    {
        #region Field

        public char Character { get; set; }
        public int Frequency { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        #endregion

        #region Constructor
        public Node(char character, int frequency, Node left, Node right)
        {
            Character = character;
            Frequency = frequency;
            Left = left;
            Right = right;
        }
        #endregion

        #region CompareTo
        //Returns >0 if the current node has more frequency than obj
        //Returns 0 if the current node is equal to obj
        //Returns <0 if the current node is less than obj
        public int CompareTo(Object obj)
        {
            if (obj != null)
            {
                Node other = (Node)obj;   // Explicit cast
                if (other != null)
                    return Frequency - other.Frequency;
                else
                    return 1;
            }
            else
                return 1;
        }
        #endregion
    }

    //------------------------------------------------------------------------
    /// <summary>
    /// Class name: Huffman
    /// description: This class creates a binary tree using priority queue. It takes the string entered by the user and ecodes 
    ///              it into 1's and 0's. which can also decode the 1's and 0's to string.
    /// </summary>
    public class Huffman
    {
        #region Field
        private Node HT; // Huffman tree to create codes and decode text
        private Dictionary<char, string> D; // Dictionary to encode text
        #endregion

        #region Constructor
        public Huffman(string S) 
        {
            D = new Dictionary<char, string>();   //create dictionary 
            Build(AnalyzeText(S));                //builds the binary heap
            CreateCodes();                        //creates code for the binary heap

        }
        #endregion

        #region AnalyzeText Method
        // 15 marks
        // Return the frequency of each character in the given text (invoked by Huffman)
        // The method first removes all the characters that are repeated in the string to remain with unique characters.
        // this is stored in variable unique.
        // it also initializes an array of size twice the length of unique (unique.Length x 2), with name frequency.
        // the array frequency stores each character with its frequency.
        // for example for the word: aaabbcdf a -> has frequecy of 3, b -> has 2, c -> 1, d -> 1 and f-> 1
        // this can be stored in frequency array as:         {['a'],[3],['b'],[2],['c'],[1],['d'],[1],['f'],[1]}
        // the array keeps characters in even indices and frequencies in odd indices.
        // it also removes all the none letter characters, for simplicity 
        private int[] AnalyzeText(string S) 
        {
            
            S = Regex.Replace(S, "[^a-zA-Z, ]", String.Empty);  //removes all the non letter characters except space.
            var unique = new HashSet<char>(S);                //removes the duplicate charactes
            int[] frequency = new int[unique.Count*2];        //creates an array of size number of unique characters in the string
            int i = 0, j = 1;


            // time complexity O(n ^ 2).
            foreach (char item in unique)  //go through each character in unique
            {
                frequency[i] = (int)item;

                foreach(char c in S)
                {
                    if (item == c)
                        frequency[j]++;
                }
                //iterate
                i += 2;    // moves to next even number                 
                j += 2;    // moves to next odd number
            }

            return frequency;            //return the array
        }
        #endregion

        #region Build
        // 20 marks
        // Build a Huffman tree based on the character frequencies greater than 0 (invoked by Huffman)
        private void Build(int[] F)
        {
            PriorityQueue<Node> PQ;
            PQ = new PriorityQueue<Node>(F.Length);     //create an instance of the priority queue
            Node left, right;


            //add all the unique character in the priotity queue
            for (int i = 0, j = 1; i < F.Length; i += 2, j +=2)
            {
                PQ.Add(new Node((char)F[i], F[j], null, null));       //add each node to the priority Queue
            }

            //it checks wether the priority queue has only one node 
            if (PQ.Size() == 1)
            {
                HT = PQ.Front();
            }

            //builds a binary tree 
            else
            {
                while (PQ.Size() > 1)
                {
                    left = PQ.Front();
                    PQ.Remove();
                    right = PQ.Front();
                    PQ.Remove();
                    PQ.Add(new Node('/', left.Frequency + right.Frequency, left, right));
                }
                HT = PQ.Front();
            }

        }
        #endregion

        #region CreateCodes method
        // 20 marks
        // Create the code of 0s and 1s for each character by traversing the Huffman tree (invoked by Huffman)
        // Store the codes in Dictionary D using the char as the key
        private void CreateCodes() 
        {
            Node cur = HT;       //root of the tree
            String bits = "";   //0s and 1s


            //invoke the method
            addBits(cur, bits);


            //creating  local recursive method
            void addBits(Node temp, string bits)
            {
                //traverses through the binary tree
                if (temp.Left != null && temp.Right != null)
                {
                    addBits(temp.Left, bits + "0");
                    addBits(temp.Right, bits + "1");
                }

                else
                {
                    D.Add(temp.Character, bits);      //stores the character and bits in the dictionary

                }
            }
        }
        #endregion

        #region Encode Method
        // 10 marks
        // Encode the given text and return a string of 0s and 1s
        public string Encode(string S) 
        {
            string encode = "";

            foreach (char item in S)
            {
                char m = (char)item;
                foreach (KeyValuePair<char, string> val in D)
                {
                    if (m == val.Key)
                        encode = encode + val.Value;
                }
            }

            return encode;
        }
        #endregion

        #region Decode Method
        // 10 marks
        // Decode the given string of 0s and 1s and return the original text
        public string Decode(string S) 
        {
            Node curr = HT;
            string decode = "";

            foreach (char ch in S)
            {
                if (curr.Left == null)
                {
                    decode += curr.Character;
                    curr = HT;
                }
                else
                {
                    if (ch == '0')
                        curr = curr.Left;
                    else
                        curr = curr.Right;
                }

                if (curr.Left == null)
                {
                    decode += curr.Character;
                    curr = HT;
                }
            }

            return decode;
        }
        #endregion
    }

    //--------------------------------------------------------------------------------------------
    /// <summary>
    /// Class name: Test
    /// description: This class test the huffman class. This ask users to enter a text which will be encoded and decoded
    /// </summary>
    public class Test
    {
        // main method
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Huffman program.");
            Console.WriteLine("Enter a word: ");
            string man = Convert.ToString(Console.ReadLine());


            Huffman huff = new Huffman(man);  //instance of huffman class 

            String encode = huff.Encode(man);
            Console.Write("\n Encoded string: ");
            Console.Write(encode);
            Console.Write("\n\n\n decoded string: ");
            Console.WriteLine(huff.Decode(encode));
           

        }
    }

}
