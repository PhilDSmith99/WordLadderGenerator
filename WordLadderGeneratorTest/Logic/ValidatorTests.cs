using System;
using Xunit;

namespace Synergenic.Windows.CLI.WordLadderGenerator.Logic.Tests
{
    /// <summary>
    /// Define a Set of Unit Tests for the Validator Methods contained in the Validator Class
    /// </summary>
    public class ValidatorTests
    {
        /// <summary>
        /// Test to ensure that the IsWord Validator reports correctly that the given string contains a letter sequence
        /// of the specified number of characters (4 is the default if not specified)
        /// </summary>
        [InlineData("", 0, true)]
        [InlineData("", null, false)]
        [InlineData("t5nt", null, false)]
        [InlineData("te*t", null, false)]
        [InlineData("bicyc!e", 7, false)]
        [InlineData("bicycle", 4, false)]
        [InlineData("       ", 7, false)]
        [InlineData("tent", 4, true)]
        [InlineData("tent", null, true)]
        [InlineData("bicycle", 7, true)]
        [Theory]
        public void CheckIsWord(string word, int? length, bool expectedTestResult)
        {
            //Assert
            if (length == null)
                Assert.Equal(expectedTestResult, Validator.IsWord(word));
            else
                Assert.Equal(expectedTestResult, Validator.IsWord(word, length.Value));
        }

        /// <summary>
        /// Test to ensure that the IsFileName Validator reports correctly that the given string appears to 
        /// detail a valid file name (excluding the directory)
        /// </summary>
        [InlineData(@"", false)]
        [InlineData(@"bicycle", true)]
        [InlineData(@"testdata1.txt", true)]
        [InlineData(@"testdata2.txt  ", false)]
        [InlineData(@"  testdata3.txt", false)]
        [InlineData(@"C:\test.dat", false)]
        [InlineData(@"C:\t*st.dat", false)]
        [InlineData(@"C:\\test.dat", false)]
        [InlineData(@"C:\\t*st.dat", false)]
        [Theory]
        public void CheckIsFileName(string fileName, bool expectedTestResult)
        {
            //Assert
            Assert.Equal(expectedTestResult, Validator.IsFileName(fileName));
        }

        /// <summary>
        /// Test to ensure that the IsEqual Validator reports correctly that the given two strings are equal
        /// </summary>
        [InlineData("", "", true)]
        [InlineData("1", "1", true)]
        [InlineData("bicycle", "", false)]
        [InlineData("hello", "tabletop", false)]
        [InlineData("hello", "23", false)]
        [InlineData("bicycle", "bicycle", true)]
        [InlineData("   ", "   ", true)]
        [Theory]
        public void CheckIsEqual(string firstString, string secondString, bool expectedTestResult)
        {
            //Assert
            Assert.Equal(expectedTestResult, Validator.IsEqual(firstString, secondString));
        }

        /// <summary>
        /// Test to ensure that the IsLess Validator reports correctly that the given first number is 
        /// less that the second number
        /// </summary>
        [InlineData(0, 0, false)]
        [InlineData(-1, 0, true)]
        [InlineData(1000, 1, false)]
        [InlineData(2, 1, false)]
        [InlineData(1, 2, true)]
        [InlineData(45, 133444, true)]
        [InlineData(27, 27, false)]
        [Theory]
        public void CheckIsLess(int number1, int number2, bool expectedTestResult)
        {
            //Assert
            Assert.Equal(expectedTestResult, Validator.IsLess(number1, number2));
        }

        /// <summary>
        /// Test to ensure that the IsNegative Validator reports correctly that the given integer is Negative
        /// </summary>
        [InlineData(0, false)]
        [InlineData(1000, false)]
        [InlineData(2, false)]
        [InlineData(-1, true)]
        [InlineData(-654, true)]
        [Theory]
        public void CheckIsNegative(int number, bool expectedTestResult)
        {
            Assert.Equal(expectedTestResult, Validator.IsNegative(number));
        }

        /// <summary>
        /// Test to ensure that the IsZero Validator reports correctly that the given integer is zero
        /// </summary>
        [InlineData(4, false)]
        [InlineData(767574, false)]
        [InlineData(-7675, false)]
        [InlineData(0, true)]
        [Theory]
        public void CheckIsZero(int number, bool expectedTestResult)
        {
            //Assert
            Assert.Equal(expectedTestResult, Validator.IsZero(number));
        }

        /// <summary>
        /// Test to ensure that the EqualLengthDiffersByCharacter Validator reports correctly that the given strings
        /// differ by the specified number of characters. The strings are assumed to be equal length due to the limitiations
        /// of this exercise. The number of character difference is set to one if not explicitly specified.
        /// </summary>
        [InlineData("test", "test", null, false)]
        [InlineData("algebra", "algebra", null, false)]
        [InlineData("algebra", "blgebra", null, true)]
        [InlineData("plankton", "!lankton", null, true)]
        [InlineData("plankton", "!lankto!", null, false)]
        [InlineData("plankton", "!!ankto!", 2, false)]
        [InlineData("plankton", "!lankto!", 2, true)]
        [InlineData("", "", null, false)]
        [InlineData("345", "445", null, true)]
        [Theory]
        public void CheckEqualLengthDiffersByCharacter(string firstWord, string secondWord, int? characters, bool expectedTestResult)
        {
            //Assert
            if (characters == null)
                Assert.Equal(expectedTestResult, Validator.EqualLengthDiffersByCharacter(firstWord, secondWord));
            else
                Assert.Equal(expectedTestResult, Validator.EqualLengthDiffersByCharacter(firstWord, secondWord, characters.Value));
        }
    }
}