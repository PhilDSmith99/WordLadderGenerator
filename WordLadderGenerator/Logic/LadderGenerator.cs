using Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Synergenic.Windows.CLI.WordLadderGenerator.Logic
{

    /// <summary>
    /// 
    /// </summary>
    public class LadderGenerator
    {
        /// <summary>
        /// Properties to hold details of the Object that is to be called to perform Logging
        /// </summary>
        private ILogger Log { get; set; }

        /// <summary>
        /// Overload the Constructor to pass in and record the Logger Object that will be used by the Class.
        /// </summary>
        /// <param name="logger">The Logger that will be used by the class</param>
        public LadderGenerator(ILogger logger)
        {
            Log = logger;
        }

        /// <summary>
        /// Return details of the 'Word Ladders' that are assocaited with the given Word Dictionary.
        /// Use the Start and End Words to detail the start and end points of the ladder.
        /// Word Ladders were invented as a game on Christmas Day in 1877 by Lewis Carroll, 22 Years 
        /// after Alice fell down the rabbit hole.
        /// </summary>
        /// <param name="startWord">The Word that represents the starting point of the Word Ladder</param>
        /// <param name="endWord">The Word that represents the end point of the Word Ladder</param>
        /// <param name="wordDictionary">The Word Dictionary that is to be used to form the Word Ladder</param>
        /// <returns></returns>
        public IEnumerable<IList<string>> GetWordLadders(string startWord, string endWord, List<string> wordDictionary)
        {
            // If the specified Start Word is empty, the End Word is empty, or the Word Dictionary is empty
            // return an Empty Set of Word Ladders
            if (string.IsNullOrEmpty(startWord) || string.IsNullOrEmpty(endWord) || !wordDictionary.Any())
                return Enumerable.Empty<IList<string>>();

            // If the specified Start Word or End Word is not present in the Word Dictionary simply return an
            // Empty Set of Word Ladders.
            if (!wordDictionary.Contains(startWord) || !wordDictionary.Contains(endWord))
                return Enumerable.Empty<IList<string>>();

            // Create a graph provisionally detailing each word in the Word Dictionary (as a Word Node) and
            // any associated Relatives (those Word Nodes whose word differs by a single character). Later
            // additional detail will be added to aid with the construction of the Word Ladders.
            List<WordNode> graph = CreateGraph(wordDictionary);
            Log.Debug($"A graph contaning {graph.Count()} Word Nodes was generated.");

            // Find the Start and End Nodes - those nodes that represent the required Start and End Words
            WordNode startNode = graph.Single(n => n.Word.Equals(startWord));
            WordNode endNode = graph.Single(n => n.Word.Equals(endWord));
            Log.Debug($"The Start Word was found at Index {graph.IndexOf(startNode)} in the Grapth.");
            Log.Debug($"The End Word was found at Index {graph.IndexOf(endNode)} in the Grapth.");

            // Perform a Breadth-First Search over the Graph starting at the Start Node.
            // Visit the relevant Nodes and add them to the search Queue setting details 
            // of their level and recording details of the Shortest Path against the Parent.
            SearchGraphAndProcess(startNode, endNode);
            Log.Debug($"{graph.Where(n => n.IsVisited).Count()} Relatives/In-Direct Relatives were included in the Breadth-First Search.");

            // Extract details of the associated Word Ladders and return them.
            return BuildWordLadders(startNode, endNode, new List<string>());
        }

        /// <summary>
        /// Create a Graph of Nodes (aka Word Nodes) for all the words found in the dictionary and populate 
        /// each Word Node with details of its relatives. Relatives being those Word Nodes that detail a
        /// word that differs from the current Node by a single character.
        /// </summary>
        /// <param name="wordDictionary">The Word Dictionary that is to be used to form the graph</param>
        /// <returns>A Graph of Word Nodes</returns>
        private List<WordNode> CreateGraph(List<string> wordDictionary)
        {
            // Create an Empty graph and add a Node for each Word in the Word Dictionary
            List<WordNode> graph = new List<WordNode>();            
            graph.AddRange(wordDictionary.Select(w => new WordNode(w)));

            // For each Word Node in the Graph add details of its Relatives. Relatives being those Node whose words
            // differ from the current Node by a single character
            graph.ForEach(n1 => n1.Relatives.AddRange(graph.Where(n2 => Validator.EqualLengthDiffersByCharacter(n1.Word, n2.Word))));
            return graph;
        }

        /// <summary>
        /// Perform a Breadth-First Search over the graph starting at the Start Word Node. 
        /// Traverse the Graph and process all of its Related Nodes. If Valid to do so
        /// process the Relative setting its Level in the Tree/Graph and adding it to the 
        /// Parents Shortest Path
        /// </summary>
        /// <param name="startNode">The Node that represents the Start Word</param>
        /// <param name="endNode">The Node that represents the End Word</param>
        private void SearchGraphAndProcess(WordNode startNode, WordNode endNode)
        {
            // Set the EndNodeLevel to MaxValue until it is found and the true Level is known
            // and set the Start Node as Level 0
            int EndNodeLevel = int.MaxValue;
            startNode.Level = 0;

            // Create a Queue to detail the Word Nodes that will be included in the Search and to
            // manage the order in which they will be visited
            Queue<WordNode> queue = new Queue<WordNode>();

            // Add the Start Word Node to Head of Queue as this is the starting point 
            queue.Enqueue(startNode);

            // While there are Nodes to process start/continue to Traverse the Tree and process their relatives
            while (queue.Any())
            {
                // Remove the Next Node from the front of the Queue
                WordNode currentNode = queue.Dequeue();

                // If the Node has not been already Visited (Processed) then attempt to process it
                if (!currentNode.IsVisited)
                {
                    // Flag the Node as Visitied
                    currentNode.IsVisited = true;

                    // If the Current Node is the End Word Node we dont need to traverse further.
                    // So set the End Node Level to the Level the End Node is at (this is used later to govern
                    // depth) and advance to the next Node in the Queue
                    if (currentNode.Word.Equals(endNode.Word))
                    {
                        EndNodeLevel = currentNode.Level;
                        continue;
                    }

                    // Calculate the Level of the Relatives (i.e. Child Nodes) as the next level down
                    int relativesLevel = currentNode.Level + 1;

                    // Only process the Releatives if they are no Lower than The End Node
                    if (relativesLevel <= EndNodeLevel)
                    {
                        // Iterate through the Relatives Nodes
                        currentNode.Relatives.ForEach(relative =>
                        {
                            // If the relative has not already been processed ??????? (Default Level is MaxValue)
                            if (relativesLevel <= relative.Level)
                                {
                                    currentNode.ShortestPath.Add(relative);
                                    relative.Level = relativesLevel;
                                    queue.Enqueue(relative);
                                }
                            });
                    }
                }
            }
        }

        /// <summary>
        /// Build up and return any Word Ladders the form a route from the Start Word to the End Word.
        /// </summary>
        /// <param name="startNode">The Node that represents the Start Word</param>
        /// <param name="endNode">The Nide that represents the End Word</param>
        /// <param name="ladder">The ladder passed in recursively</param>
        /// <returns></returns>
        private IEnumerable<IList<string>> BuildWordLadders(WordNode startNode, WordNode endNode, List<string> ladder)
        {
            ladder.Add(startNode.Word);

            if (startNode.Word == endNode.Word)
            {
                List<string> copiedLadder = ladder.ToList();
                yield return copiedLadder;
                ladder.Remove(startNode.Word);
                yield break;
            }

            foreach (WordNode child in startNode.ShortestPath)
                foreach (IList<string> childLadder in BuildWordLadders(child, endNode, ladder))
                    yield return childLadder;

            ladder.Remove(startNode.Word);
        }
    }
}