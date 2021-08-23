namespace Synergenic.Windows.CLI.WordLadderGenerator.UI.Enums
{
    /// <summary>
    /// Enum that represents the Various Validation Statuses that may be returned 
    /// when the application in invoked.
    /// </summary>
    public enum ValidationStatusTypes
    {
        AllOK = 0,
        InvalidNumberOfArguments    = -1,
        DictionaryFileNotFound      = -2,
        StartWordInvalid            = -3,
        EndWordInvalid              = -4,
        StartAndEndWordsEqual       = -5,
        ResultsFileInvalid          = -6,
        DictionaryHasNoValidWords   = -7,
        StartWordNotInDictionary    = -8,
        EndWordNotInDictionary      = -9,
        EndWordFallsBeforeStartWord = -10
    }
}




