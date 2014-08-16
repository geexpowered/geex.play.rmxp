
namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// When [**]
        /// </summary>
        /// <returns></returns>
        bool Command402()
        {
            // If fitting choices are selected
            if (branch != null)
            {
                if (branch[list[index].Indent].Val == intParams[0])
                {
                    // Delete branch data
                    branch[list[index].Indent].Empty();
                    // Continue
                    return true;
                }
            }
            // If it doesn't meet the condition: command skip
            return CommandSkip();
        }
    }
}

