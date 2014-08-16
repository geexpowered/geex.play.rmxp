
namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Conditional branch - Repeat Above
        /// </summary>        
        bool Command413()
        {
            // Get indent
            int indent = list[index].Indent;
            // Loop
            do
            {
                // Return index
                index -= 1;
            } while (list[index].Indent != indent);
            // If this event command is the same level as indent
            return true;
        }
    }
}

