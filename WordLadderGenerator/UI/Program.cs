using Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure;
using Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure.Interfaces;
using Synergenic.Windows.CLI.WordLadderGenerator.Logic;
using Synergenic.Windows.CLI.WordLadderGenerator.UI.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Synergenic.Windows.CLI.WordLadderGenerator.UI
{
    /// <summary>
    /// Blue Prism Technical Assessment Console Application
    /// </summary>
    public class Program
    {
        public ILogger Log { get; set; }
        public FileIO IO { get; set; }

        public static ValidationStatusTypes ValidationStatus { get; set; }

        /// <summary>
        /// Details the path to the Text File that contains the four letter words (as passed via the Console Arguments)
        /// </summary>
        public static string DictionaryFile { get; set; }

        /// <summary>
        /// Details the path to the Result File to which the resultant words will be written (as passed via the Console Arguments)
        /// </summary>
        private static string ResultFile { get; set; }

        /// <summary>
        /// Details the Start Word (as passed via the Console Arguments)
        /// </summary>
        public static string StartWord { get; set; }

        /// <summary>
        /// Details the End Word (as passed via the Console Arguments)
        /// </summary>
        public static string EndWord { get; set; }

        /// <summary>
        /// Details the Index of the Start Word in the ValidWords List
        /// </summary>
        private static int StartIndex { get; set; }

        /// <summary>
        /// Details the Index of the End Word in the ValidWords List
        /// </summary>
        private static int EndIndex { get; set; }

        /// <summary>
        /// The main entry point of the application. 
        /// Parse and validate the command line arguments reporting and exitting if validation issues are 
        /// encountered. Read, clense and sort the Dictionary File storing the resultant words as a list. 
        /// Validate the resultant list and attempt to find the required Start & End Words. Report and exit 
        /// if validation issues are encountered. Finally attempt to generate the associated Word Ladders 
        /// persisting them to the results file and inform the user.
        /// </summary>
        /// <param name="args">The arguments passed to the application from the command line</param>
        public static void Main(string[] args)
        {
            Program Program = new Program { Log = new Logger() };
            Program.IO = new FileIO(Program.Log);
            Program.ValidationStatus = ValidationStatusTypes.AllOK;

            LadderGenerator generator = new LadderGenerator(Program.Log);

            Program.ValidationStatus = Program.ValidateArguments(args);
            if (Program.ValidationStatus != ValidationStatusTypes.AllOK)
                Environment.Exit((int)Program.ValidationStatus);

            // Sort the Dictionary alphabetically descending (the default comparer)
            List<string> cleansedDictionary = Program.IO.ReadDictionary(DictionaryFile);
            cleansedDictionary.Sort();

            Program.ValidationStatus = Program.ValidateWordDictionary(cleansedDictionary);
            if (Program.ValidationStatus != ValidationStatusTypes.AllOK)
                Environment.Exit((int)Program.ValidationStatus);

            Program.Log.Info($"{ File.ReadLines(DictionaryFile).Count()} words/lines were found in the raw Dictionary File.");
            Program.Log.Info($"{cleansedDictionary.Count} valid words were found in the cleansed Dictionary File.");
            Program.Log.Info($"Calling Word Ladder to find Ladder For {StartWord} to {EndWord}.");

            List<IList<string>> ladders = generator.GetWordLadders(StartWord, EndWord, cleansedDictionary).ToList();

            int currentLadder = 1;
            ladders.ForEach(ladder =>
            {
                if (ladders.Count() > 1)
                    Program.IO.Append(ResultFile, new List<string>() { $"Ladder {currentLadder} Found :" });
                Program.IO.Append(ResultFile, ladder);
                Program.Log.Debug($"Ladder {currentLadder} Found.");
                //ladder.ToList().ForEach(w => Program.Log.Debug($"\t{w}"));
                currentLadder++;
            });
        }

        /// <summary>
        /// Validate the arguments passed to the application and set associated properties to aid readability etc.
        /// Inform the user if any validation issues are encounterd with the arguments.
        /// </summary>
        /// <param name="args">An array of command line arguments</param>
        /// <returns>A ValidationStatusType detailing Success or Failure</returns>
        public ValidationStatusTypes ValidateArguments(string[] args)
        {
            if (args.Length != 4)
            {
                Log.Error("Four Arguments are expected: [DictionaryFile] [StartWord] [EndWord] [ResultFile].\nFor Example: WordLadder words-english.txt spin spot ResultFile.txt");
                return ValidationStatusTypes.InvalidNumberOfArguments;
            }

            DictionaryFile = args[0];
            StartWord = args[1].ToLower();
            EndWord = args[2].ToLower();
            ResultFile = args[3];

            if (!IO.Exists(DictionaryFile))
            {
                Log.Error($"The DictionaryFile ({DictionaryFile}) must be present and accessible.");
                return ValidationStatusTypes.DictionaryFileNotFound;
            }

            if (!Validator.IsWord(StartWord))
            {
                Log.Error("The StartWord must be specified and be a four letter word.");
                return ValidationStatusTypes.StartWordInvalid;
            }

            if (!Validator.IsWord(EndWord))
            {
                Log.Error("The EndWord must be specified and be a four letter word.");
                return ValidationStatusTypes.EndWordInvalid;
            }

            if (Validator.IsEqual(StartWord, EndWord))
            {
                Log.Error("The StartWord and EndWord must specify different Words.");
                return ValidationStatusTypes.StartAndEndWordsEqual;
            }

            if (!Validator.IsFileName(ResultFile))
            {
                Log.Error($"The specified ResultFile ({ResultFile}) details an invalid file name.");
                return ValidationStatusTypes.ResultsFileInvalid;
            }

            return ValidationStatusTypes.AllOK;
        }

        /// <summary>
        /// Validate the resultant word list dictionary and attempt to find the required Start and 
        /// End Words. Inform the user if any validation issues are encounterd.
        /// </summary>
        /// <returns>A ValidationStatusType detailing Success or Failure</returns>
        public ValidationStatusTypes ValidateWordDictionary(List<string> wordDictionary)
        {
            // Count the total number of valid words found in the list
            // Inform the user if the list did not contain any valid words.
            int totalValidWords = wordDictionary.Count;
            if (Validator.IsZero(totalValidWords))
            {
                Log.Error("The Dictionary File does not contain any valid four letter words.");
                return ValidationStatusTypes.DictionaryHasNoValidWords;
            }

            // Attempt to find the positions of the start word and end word in the list
            StartIndex = wordDictionary.IndexOf(StartWord);
            EndIndex = wordDictionary.IndexOf(EndWord);

            // If either was not present inform the user
            if (Validator.IsNegative(StartIndex))
            {
                Log.Error(($"The StartWord ({StartWord}) was not found in the Dictionary File."));
                return ValidationStatusTypes.StartWordNotInDictionary;
            }

            if (Validator.IsNegative(EndIndex))
            {
                Log.Error(($"The EndWord ({EndWord}) was not found in the Dictionary File."));
                return ValidationStatusTypes.EndWordNotInDictionary;
            }

            // If the end word falls before the start inform the user
            if (Validator.IsLess(EndIndex, StartIndex))
            {
                Log.Error("The EndWord falls before the StartWord. Please check your arguments.");
                return ValidationStatusTypes.EndWordFallsBeforeStartWord;
            }

            return ValidationStatusTypes.AllOK;
        }
    }
}
