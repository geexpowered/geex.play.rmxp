
namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Jump to Label
        /// </summary>
        bool Command119()
        {
            // Get label Name
            string label_name = stringParams[0];
            // Initialize temporary variables
            int temp_index = 0;
            do
            {
                // If a fitting label was not found
                if (temp_index >= list.Length - 1) return true;
                // If this event command is a designated label Name
                if (list[temp_index].Code == 118 && list[temp_index].StringParams[0] == label_name)
                {
                    // Update index
                    index = temp_index;
                    return true;
                }
                // Advance index
                temp_index += 1;

            }
            while (true);
        }
    }
}

