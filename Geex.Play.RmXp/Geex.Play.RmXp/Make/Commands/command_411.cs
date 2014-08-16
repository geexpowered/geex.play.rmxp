
namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Conditional branch - Else
        /// </summary>
        bool Command411()
        {
            // If determinant results are false
            if (branch[list[index].Indent].Result == false && !branch[list[index].Indent].IsEmpty)
            {
              // Delete branch data
                branch[list[index].Indent].Empty();
              // Continue
              return true;
            }
            // If it doesn't meet the conditions: command skip
            return CommandSkip();
        }
    }
}

