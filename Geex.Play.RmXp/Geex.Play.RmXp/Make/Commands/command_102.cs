using System;
using System.Collections.Generic;
using Geex.Play.Rpg.Game;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Show Choices
        /// code = 102
        /// parameter = [0]=Choice Cancel type, [1]=choice1, [2]=choice2, [3]=choice3, [4]=choice4
        /// </summary>
        bool Command102()
        {
            // If text has been set to message_text
            if (InGame.Temp.MessageText != null) return false;
            // Set message end waiting flag and callback
            isWaitingMessage = true;
            InGame.Temp.MessageProc = new ProcEmpty(ProcMessageWaiting);
            // Choices setup
            InGame.Temp.MessageText = "";
            InGame.Temp.ChoiceStart = 0;
            SetupChoices(intParams[0],stringParams);
            // Continue
            return true;
        }

   }
}

