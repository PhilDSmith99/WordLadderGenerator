using System.Collections.Generic;
using System.Linq;
using System.IO.Abstractions.TestingHelpers;
using Xunit;
using System.IO;

namespace Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure.Tests
{

    /// <summary>
    /// Define a Set of Unit Tests for the File IO related Methods contained in the FileIO Class
    /// </summary>
    public class FileIOTests
    {
        /// <summary>
        /// Holds a MockFileSystem allowing us to perform tests without a reliance on the actual File System
        /// </summary>
        MockFileSystem MockFileSystem { get; set; } = new MockFileSystem();


        /// <summary>
        /// Test to ensure that the Exists Method correctly identifies whether or not the specified file 
        /// is absent or present and accessible in the file system.
        /// </summary>
        [InlineData(@"C:\test.txt", true)]
        [InlineData(@"Z:\temp\test.txt", false)]
        [InlineData(@"C:\directory_not_present\file_not_present.txt", false)]
        [InlineData(@"C:\temp\file_not_present.txt", false)]
        [Theory]
        public void CheckExistsReturnsCorrectValue(string fileName, bool expectedTestResult)
        {
            // Arrange
            MockFileData mockInputFile = new MockFileData("line1\nline2\nline3");
            MockFileSystem.AddFile(@"C:\test.txt", mockInputFile);

            // Act
            FileIO fileIO = new FileIO(MockFileSystem);
            bool fileExists = fileIO.Exists(fileName);

            // Assert
            Assert.Equal(expectedTestResult, fileExists);
        }

        /// <summary>
        /// Test to ensure that the Read Dictionary Method correctly Reads the specified Word Dictionary File
        /// and performs initial optimisation such as removing any entries that are not four letter words and
        /// converting the dictionary to lower case.
        /// </summary>
        [Fact]
        public void CheckReadDictionaryOutputCorrect()
        {
            // Arrange
            string dictionaryFile = @"C:\temp\Dictionary.txt";
            MockFileData mockInputFile = new MockFileData("toolong1\ntoolong2\nasd4\ntree\nfire\nline");
            MockFileSystem.AddFile(dictionaryFile, mockInputFile);

            // Act
            FileIO fileIO = new FileIO(MockFileSystem);
            List<string> dictionary = fileIO.ReadDictionary(dictionaryFile);

            // Assert
            Assert.Equal("tree", dictionary[0]);
            Assert.Equal("fire", dictionary[1]);
            Assert.Equal("line", dictionary[2]);
            Assert.Equal(3, dictionary.Count());
        }

        /// <summary>
        /// Test to ensure that the Read Dictionary Method correctly throws an Exception if the Word Dictionary
        /// is not found.
        /// </summary>
        [Fact]
        public void CheckReadDictionaryHandlesIOException()
        {
            // Arrange
            string fileName = @"C:\directory_not_present\file_not_present.txt";
            MockFileData mockInputFile = new MockFileData("toolong1\ntoolong2\nasd4\ntree\nfire\nline");
            MockFileSystem.AddFile(@"C:\temp\Dictionary.txt", mockInputFile);

            // Act
            FileIO fileIO = new FileIO(MockFileSystem);

            // Assert
            Assert.Throws<FileNotFoundException>(() => fileIO.ReadDictionary(fileName));
        }

        /// <summary>
        /// Test to ensure that the Write Method correctly Writes the specified File to the correct location.
        /// </summary>
        [Fact]
        public void CheckWriteOutputCorrect()
        {
            // Arrange
            string outputFile = @"C:\temp\Output.txt";
            IEnumerable<string> information = new List<string>() { "one", "two", "three", "four", "five" };

            // Act
            FileIO fileIO = new FileIO(MockFileSystem);
            fileIO.Append(outputFile, information);
            List<string> inputLines = MockFileSystem.File.ReadLines(outputFile).ToList();

            // Assert
            Assert.Equal("one", inputLines[0]);
            Assert.Equal("two", inputLines[1]);
            Assert.Equal("three", inputLines[2]);
            Assert.Equal("four", inputLines[3]);
            Assert.Equal("five", inputLines[4]);
            Assert.Equal(5, inputLines.Count());
        }

        /// <summary>
        /// Test to ensure that the Write Method correctly throws an Exception if the specified File
        /// can not be written.
        /// </summary>
        [Fact]
        public void CheckWriteIOException()
        {
            // Arrange
            string outputFile = @"C:\dummy_directory\output.txt";
            IEnumerable<string> information = new List<string>() { "one", "two", "three", "four", "five" };

            // Act
            FileIO fileIO = new FileIO(MockFileSystem);
            
            // Assert
            Assert.Throws<DirectoryNotFoundException>(() => fileIO.Append(outputFile, information));
        }
    }
}