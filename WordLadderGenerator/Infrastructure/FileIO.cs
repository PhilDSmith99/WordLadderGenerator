using Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure
{

    /// <summary>
    /// Provides general purpose File IO related methods to the application.
    /// </summary>
    public class FileIO
    {

        /// <summary>
        /// Holds details of the Object that will be used to perform File System functionality.
        /// The constructors specify the object via interface allowing us to pass different FileSystem Objects to the class.
        /// This is especially of use when running Unit Tests against the class as it allows us to pass in a Mocked File System.
        /// </summary>
        private IFileSystem FileSystem { get; set; }

        /// <summary>
        /// Holds details of the Object that will be used to perform Logging functionality (if supplied via the Constructor)
        /// </summary>
        private ILogger Log { get; set; }

        /// <summary>
        /// Override the default Constructor to create an instance of the Default File System Object and pass it to the
        /// overloaded constructor which records details of the IFileSystem Object that was specified. This allows us to
        /// pass different FileSystem Objects to the class - this is especially of use when running Unit Tests against
        /// the class as it allows us to pass in a Mocked File System.
        /// </summary>
        public FileIO() : this(new FileSystem()) { }

        /// <summary>
        /// Overload the Constructor to pass in and record the File System Object that will be used by the Class.
        /// </summary>
        /// <param name="fileSystem">The File System Object that will be used by the class</param>
        public FileIO(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        /// <summary>
        /// Overload the Constructor to pass a default FileSystem Object (vis its overloaded call) and pass and record 
        /// the Object that will be used to perform Logging functionality.
        /// Invoke the 
        /// </summary>
        /// <param name="logger"></param>
        public FileIO(ILogger logger) : this(new FileSystem())
        {
            Log = logger;
        }

        /// <summary>
        /// Check if the given file exists and is accessible in the filestore
        /// </summary>
        /// <param name="fileName">The name of the file that is to be checked</param>
        /// <returns>True if the file is present and accessible, false otherwise</returns>
        public bool Exists(string fileName)
        {
            return FileSystem.File.Exists(fileName);
        }

        /// <summary>
        /// Read the Dictionary File and perform initial optimsation by removing any entries that are not four letter words. 
        /// And Storing the words in a lower case list (to simplify future processing). 
        /// </summary>
        /// <param name="fileName">The name of the file that represents the Dictionary File</param>
        /// <returns>A cleansed Dictionary File</returns>
        public List<string> ReadDictionary(string fileName)
        {
            List<string> DictionaryFile = new List<string>();

            try
            {
                DictionaryFile = FileSystem.File.ReadLines(fileName)
                    .Where(l => l.Length == 4 && l.All(char.IsLetter))
                    .Select(w => w.ToLower())
                    .ToList();
            }
            catch (IOException e)
            {
                if (Log != null)
                    Log.Error(($"There was an error reading the Dictionary File. Exception Details: {e.Message}"));
                else
                    throw e;
            }

            return DictionaryFile;
        }

        /// <summary>
        /// Write the given information to the specified file and handle any associated IO Exceptions
        /// </summary>
        /// <param name="fileName">The file to which the information is to be written</param>
        /// <param name="information">The information that is to be written</param>
        public void Append(string fileName, IEnumerable<string> information)
        {
            try
            {
                FileSystem.File.AppendAllLines(fileName, information);
            }
            catch (IOException e)
            {
                if (Log != null)
                    Log.Error(($"There was an error writing to the file: {fileName}. Exception Details: {e.Message}"));
                else
                    throw (e);
            }
        }
    }
}
