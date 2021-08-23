using Xunit;
using System.IO;
using System;

namespace Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure.Tests
{
    /// <summary>
    /// Define a Set of Unit Tests for the Logging Methods contained in the Logger Class
    /// </summary>
    public class LoggerTests
    {
        /// <summary>
        /// Test to ensure that the Info Level Logger logs to the console and correctly identifies the Log Level.
        /// </summary>
        [Fact]
        public void CheckInfoLevelLoggingToConsole()
        {
            // Arrange
            Logger log = new Logger();
            StringWriter consoleOutput = new StringWriter();
            string testMessage = "Sample Info Message";

            // Act
            Console.SetOut(consoleOutput);
            log.Info(testMessage);
            string interceptedConsole = consoleOutput.ToString();
            consoleOutput.Flush();
            consoleOutput.Dispose();
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

            // Assert
            Assert.StartsWith("[info]", interceptedConsole);
            Assert.Contains(testMessage, interceptedConsole);
        }

        /// <summary>
        /// Test to ensure that the Debug Level Logger logs to the console and correctly identifies the Log Level.
        /// </summary>
        [Fact]
        public void CheckDebugLevelLoggingToConsole()
        {
            // Arrange
            Logger log = new Logger();
            StringWriter consoleOutput = new StringWriter();
            string testMessage = "Sample Debug Message";

            // Act
            Console.SetOut(consoleOutput);
            log.Debug(testMessage);
            string interceptedConsole = consoleOutput.ToString();
            consoleOutput.Flush();
            consoleOutput.Dispose();
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

            // Assert
            Assert.StartsWith("[debug]", interceptedConsole);
            Assert.Contains(testMessage, interceptedConsole);
        }

        /// <summary>
        /// Test to ensure that the Error Level Logger logs to the console and correctly identifies the Log Level.
        /// </summary>
        [Fact]
        public void CheckErrorLevelLoggingToConsole()
        {
            // Arrange
            Logger log = new Logger();
            StringWriter consoleOutput = new StringWriter();
            string testMessage = "Sample Error Message";

            // Act
            Console.SetOut(consoleOutput);
            log.Error(testMessage);
            string interceptedConsole = consoleOutput.ToString();
            consoleOutput.Flush();
            consoleOutput.Dispose();
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

            // Assert
            Assert.StartsWith("[error]", interceptedConsole);
            Assert.Contains(testMessage, interceptedConsole);
        }
    }
}