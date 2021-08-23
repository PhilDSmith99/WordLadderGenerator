using System.Collections.Generic;

namespace Synergenic.Windows.CLI.WordLadderGenerator.Logic
{
    /// <summary>
    /// Class to represent a Node in the Graph that maps Words that were found in the Word Dictionary.
    /// The class details the Word, its Relatives (Nodes that are related in terms that the words they 
    /// represent differ from the current Node's word by a single character), whether the Node has been 
    /// visited as part of a Breadth-First Search and details of its Level from the Start Word and the
    /// Shortest Path that may be taken through the Graph.
    /// </summary>
    public class WordNode
    {
        /// <summary>
        /// The Word that the Word Node represents
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// Details of any related Nodes.
        /// </summary>
        public List<WordNode> Relatives { get; } = new List<WordNode>();

        /// <summary>
        /// Details the Shortlest Path Found
        /// </summary>
        public List<WordNode> ShortestPath { get; } = new List<WordNode>();

        /// <summary>
        /// The Level the Node is from the Start Word Node in terms of a Tree Structure
        /// </summary>
        public int Level { get; set; } = int.MaxValue;

        /// <summary>
        /// Details of whether or not the Node has been processed (visisted by the Breadth-First Search Algorithm)
        /// </summary>
        public bool IsVisited { get; set; } = false;

        /// <summary>
        /// Constuctor that allows us to set the Word Name
        /// </summary>
        /// <param name="word">The name of the Word that the Node represents</param>
        public WordNode(string word)
        {
            Word = word;
        }
    }
}
