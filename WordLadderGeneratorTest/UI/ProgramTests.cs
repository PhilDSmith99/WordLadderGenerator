using Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure;
using Synergenic.Windows.CLI.WordLadderGenerator.UI.Enums;
using System.Collections.Generic;
using Xunit;

namespace Synergenic.Windows.CLI.WordLadderGenerator.UI
{
    public class ProgramTests
    {

        [Fact]
        public void CheckInvalidNumberOfArugumentsValidated()
        {
            // Arrange
            Program program = new Program { Log = new Logger() };
            program.IO = new FileIO(program.Log);
            Program.ValidationStatus = ValidationStatusTypes.AllOK;
            string[] args = new string[] { @"C:\BluePrism\words-english.txt", "spin", "spot" };

            // Act
            Program.ValidationStatus = program.ValidateArguments(args);

            // Assert
            Assert.Equal(ValidationStatusTypes.InvalidNumberOfArguments, Program.ValidationStatus);
        }


        [InlineData(@"C:\BluePrism\words-english-no-such-file.txt", "spin", "spot", "ResultFile.txt", ValidationStatusTypes.DictionaryFileNotFound )] // InvalidDictionaryFileName
        [InlineData(@"C:\BluePrism\words-english.txt", "spinning", "spot", "ResultFile.txt", ValidationStatusTypes.StartWordInvalid )]                // InvalidStartWord
        [InlineData(@"C:\BluePrism\words-english.txt", "spin", "1234", "ResultFile.txt", ValidationStatusTypes.EndWordInvalid )]                      // InvalidEndWord
        [InlineData(@"C:\BluePrism\words-english.txt", "spin", "spin", "ResultFile.txt", ValidationStatusTypes.StartAndEndWordsEqual )]               // InvalidStartAndEndWordsEqual
        [InlineData(@"C:\BluePrism\words-english.txt", "spin", "spot", "ResultFile****.txt", ValidationStatusTypes.ResultsFileInvalid )]              // InvalidReesultsFileName
        [InlineData(@"C:\BluePrism\words-english.txt", "spin", "spot", "ResultFile.txt", ValidationStatusTypes.AllOK)]                                // All OK
        [Theory]
        public void CheckInvalidArgumentsValidated(string dictionaryFile, string startWord, string endWord, string resultsFile, ValidationStatusTypes validationStatus)
        {
            // Arrange
            Program program = new Program { Log = new Logger() };
            program.IO = new FileIO(program.Log);
            Program.ValidationStatus = ValidationStatusTypes.AllOK;
            string[] args = new string[] { dictionaryFile, startWord, endWord, resultsFile };

            // Act
            Program.ValidationStatus = program.ValidateArguments(args);

            // Assert
            Assert.Equal(validationStatus, Program.ValidationStatus);
        }

        [InlineData(@"C:\BluePrism\words-english-no-such-file.txt", "spin", "spot", "ResultFile.txt", ValidationStatusTypes.DictionaryHasNoValidWords)] // InvalidDictionaryNoWords
        [InlineData(@"C:\BluePrism\words-english.txt", "zzzz", "spot", "ResultFile.txt", ValidationStatusTypes.StartWordNotInDictionary)]                // InvalidStartWordNotInDictionary
        [InlineData(@"C:\BluePrism\words-english.txt", "spin", "zzzz", "ResultFile.txt", ValidationStatusTypes.EndWordNotInDictionary)]                      // InvalidEndWordNotInDictionary
        [InlineData(@"C:\BluePrism\words-english.txt", "spot", "spin", "ResultFile.txt", ValidationStatusTypes.EndWordFallsBeforeStartWord)]               // InvalidEndFallsBeforeStartWord
        [Theory]
        public void CheckInvalidDictionaryValidated(string dictionaryFile, string startWord, string endWord, string resultsFile, ValidationStatusTypes validationStatus)
        {

            // Arrange
            Program program = new Program { Log = new Logger() };
            program.IO = new FileIO(program.Log);
            Program.ValidationStatus = ValidationStatusTypes.AllOK;
            string[] args = new string[] { dictionaryFile, startWord, endWord, resultsFile };
            Program.ValidationStatus = program.ValidateArguments(args);

            // Act
            List<string> cleansedDictionary = program.IO.ReadDictionary(Program.DictionaryFile);
            cleansedDictionary.Sort();
            Program.ValidationStatus = program.ValidateWordDictionary(cleansedDictionary);

            // Assert
            Assert.Equal(validationStatus, Program.ValidationStatus);
        }
    }
}
