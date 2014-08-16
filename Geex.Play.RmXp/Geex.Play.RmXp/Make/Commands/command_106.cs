using Geex.Edit;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Wait
        /// </summary>
        bool Command106()
        {
            // Set wait count
            waitCount = (int)(intParams[0] * 2 * GameOptions.AdjustFrameRate);
            // Continue
            return true;
        }
    }
}

