using System;
using System.Collections.Generic;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Conditional branch - Break Loop
        /// </summary>
        bool Command113()
        {

            // Get indent
            int indent = list[index].Indent;
            // Copy index to temporary variables
            int temp_index = index;
            do
            {

                // Advance index
                temp_index += 1;
                // If a fitting loop was not found
                if (temp_index >= list.Length - 1) return true;
                // If this event command is [repeat above] and indent is shallow
                if (list[temp_index].Code == 413 && list[temp_index].Indent < indent)
                {
                    // Update index
                    index = temp_index;
                    // Continue
                    return true;
                }
            }
            while (true);
        }
    }
}

