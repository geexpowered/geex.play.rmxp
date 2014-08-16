
namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Button Input Processing
        /// </summary>
        bool Command105()
        {
            // Set variable ID for button input
            buttonInputVariableId = intParams[0];
            // Advance index
            index += 1;
            return false;
        }
    }
}

