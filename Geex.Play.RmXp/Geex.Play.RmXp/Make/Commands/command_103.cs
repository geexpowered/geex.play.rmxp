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
        /// Input Number
        /// </summary>
        bool Command103()
        {
            // If text has been set to message_text
            if (InGame.Temp.MessageText != null)
            {
                // End
                return false;
            }
            // Set message end waiting flag and callback
            isWaitingMessage = true;
            InGame.Temp.MessageProc = new ProcEmpty(ProcMessageWaiting);
            // Number input setup
            InGame.Temp.MessageText = "";
            InGame.Temp.NumInputStart = 0;
            InGame.Temp.NumInputVariableId = intParams[0];
            InGame.Temp.NumInputDigitsMax = intParams[1];
            // Continue
            return true;
        }
    }
}

