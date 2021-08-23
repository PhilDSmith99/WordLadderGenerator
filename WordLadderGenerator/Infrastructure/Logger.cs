using Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure.Interfaces;
using System;

namespace Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure
{
    /// <summary>
    /// General Purpose Console Logger.
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// Details the active Console Foreground Colour so we can revert after a change of colour
        /// </summary>
        private ConsoleColor ForegroundColour { get; set; } = Console.ForegroundColor;

        /// <summary>
        /// Write the required message to the console as information waiting for a keypress if required
        /// </summary>
        /// <param name="message">The message that is to be logged</param>
        public void Info(string message)
        {
            Console.WriteLine($"[info] : {message}");
        }

        /// <summary>
        /// Write the required message to the console as debug waiting for a keypress if required
        /// </summary>
        /// <param name="message">The message that is to be logged</param>
        public void Debug(string message)
        {
            if (Console.BackgroundColor != ConsoleColor.Green)
                Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[debug]: {message}");
            Console.ForegroundColor = ForegroundColour;
        }

        /// <summary>
        /// Output the specified error message to the console highlighting it in red for visibility.
        /// Prompt the user to press a key to confirm and then exit the application returning an appropriate
        /// exit code.
        /// </summary>
        /// <param name="message">The error message that is to be displayed to the user</param>
        public void Error(string message)
        {
            if (Console.BackgroundColor != ConsoleColor.Red)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[error]: {message}");
            Console.ForegroundColor = ForegroundColour;
        }
    }
}
