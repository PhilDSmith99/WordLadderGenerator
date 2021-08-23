namespace Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure.Interfaces
{
    public interface ILogger
    {
        /// <summary>
        /// Write the required message to the console highlighting it as information
        /// </summary>
        /// <param name="message">The info message that is to be logged</param>
        void Info(string message);

        /// <summary>
        /// Write the required message to the console highlighting it as debug 
        /// </summary>
        /// <param name="message">The debug message that is to be logged</param>
        void Debug(string message);

        /// <summary>
        /// Output the specified error message to the console highlighting it as error
        /// exit code.
        /// </summary>
        /// <param name="message">The error message that is to be logged</param>
        void Error(string message);
    }
}
