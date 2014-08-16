
namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// When Cancel
        /// </summary>
        bool Command403()
        {
            // If choices are cancelled
            if (branch[list[index].Indent].Val == 4)
            {
                // Delete branch data
                branch[list[index].Indent].Empty();
                // Continue
                return true;
            }
            // If it doen't meet the condition: command skip
            return CommandSkip();
        }
    }
}

