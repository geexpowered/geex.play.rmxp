namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Exit Event Processing
        /// </summary>
        bool Command115()
        {
            // End event
            CommandEnd();
            return true;
        }
    }
}

