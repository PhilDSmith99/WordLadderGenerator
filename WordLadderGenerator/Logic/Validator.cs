using System.IO;
using System.Linq;

namespace Synergenic.Windows.CLI.WordLadderGenerator.Logic
{
    /// <summary>
    /// Defines common validation methods uses throughout the application 
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Check if the specifed string is a letter sequence of N characters. 
        /// Where N defaults to 4 if not specified.
        /// </summary>
        /// <param name="value">The string that is to be tested</param>
        /// <param name="length">The expected length of the string. Defaults to 4 if not specified</param>
        /// <returns>True if the given string conforms to a four letter sequence, false otherwise</returns>
        public static bool IsWord(string value, int length = 4)
        {
            return value.Length == length && value.All(char.IsLetter);
        }

        /// <summary>
        /// Compare the given two words (of Equal Length) and return true if they differ (by a single character by default)
        /// </summary>
        /// <param name="firstWord">The first word that is to be compared</param>
        /// <param name="secondWord">The second word that is to be compared</param>
        /// <param name="numberOfDifferences">The number of differences that are expected. Defaults to 1</param>
        /// <returns>True if the words differ by the required number differences</returns>
        public static bool EqualLengthDiffersByCharacter(string firstWord, string secondWord, int numberOfDifferences = 1)
        {
            int differences = 0;

            for (int i = 0; i < firstWord.Length && differences < numberOfDifferences + 1; ++i)
                if (firstWord[i] != secondWord[i])
                    differences++;
            return (differences == numberOfDifferences);
        }

        /// <summary>
        /// Check if the specifed string appears to detail a valid file name (excluding the directory)
        /// </summary>
        /// <param name="fileName">The filename that is to be tested</param>
        /// <returns>True if the string appears to contain a valid file name (excluding the directory)</returns>
        public static bool IsFileName(string fileName)
        {
            try
            {
                if (fileName == string.Empty || Path.GetDirectoryName(fileName) != string.Empty)
                    return false;

                fileName = Path.GetFileName(fileName);
                bool containsValidChars = fileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
                return (containsValidChars && fileName != string.Empty && !fileName.StartsWith(" ") && !fileName.EndsWith(" "));
            }
            catch 
            {
                return false;
            }

        }

        /// <summary>
        /// Check if the specifed strings are equal
        /// </summary>
        /// <param name="firstString">The first string that is to be tested for equality</param>
        /// <param name="secondString">The second string that is to be tested for equality</param>
        /// <returns>True if the strings are equal, false otherwise.
        public static bool IsEqual(string firstString, string secondString)
        {
            return firstString == secondString;
        }

        /// <summary>
        /// Check if the first integer is less than the second number
        /// </summary>
        /// <param name="firstNumber">The first integer to be tested</param>
        /// <param name="secondNumber">The second integer to be tested</param>
        /// <returns>True if the first integer is less than the second, false otherwise</returns>
        public static bool IsLess(int firstNumber, int secondNumber)
        {
            return firstNumber < secondNumber;
        }

        /// <summary>
        /// Check if the given integer is negative
        /// </summary>
        /// <param name="number">The interger that is to be tested</param>
        /// <returns>True if the integer is negative, false otherwise</returns>
        public static bool IsNegative(int number)
        {
            return number < 0;
        }

        /// <summary>
        /// Check if the given integer is equal to zero
        /// </summary>
        /// <param name="number">The interger that is to be tested</param>
        /// <returns>True if the integer is zero, false otherwise</returns>
        public static bool IsZero(int number)
        {
            return number == 0;
        }
    }
}
