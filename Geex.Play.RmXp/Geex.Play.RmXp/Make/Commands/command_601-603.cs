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
        /// Battle Processing - If Win
        /// </summary>	    
		bool Command601()
        {
			// When battle results = win
			if (branch[list[index].Indent].Val == 0)
            {
				// Delete branch data
				branch[list[index].Indent].Empty();
				return true;
			}
			// If it doesn't meet conditions: command skip
			return CommandSkip();
		}
	    
        /// <summary>
        /// Battle Processing - If Escape
        /// </summary>
        bool Command602()
        {
			// If battle results = escape
			if (branch[list[index].Indent].Val == 1)
            {
				// Delete branch data
                branch[list[index].Indent].Empty();
				return true;
			}
			// If it doesn't meet conditions: command skip
			return CommandSkip();
		}
	    
        /// <summary>
        /// Battle Processing - If Win
        /// </summary>
        bool Command603()
        {
			// If battle results = lose
			if (branch[list[index].Indent].Val == 2)
            {
				// Delete branch data
                branch[list[index].Indent].Empty();
				return true;
			}
			// If it doesn't meet conditions: command skip
            return CommandSkip();
		} 
    }
}

