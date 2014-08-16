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
        /// Show Text
        /// </summary>
        bool Command101()
        {
            // If other text has been set to message_text
            if (InGame.Temp.MessageText != null) return false;
            // Pass the event id that opens the Message Window
            InGame.Temp.MessageWindowEventID = eventId;
            // Set message end waiting flag and callback
            isWaitingMessage = true;
            InGame.Temp.MessageProc = new ProcEmpty(ProcMessageWaiting);
            // Set message text on first line
            InGame.Temp.MessageText = list[index].StringParams[0] + "\n";
            int line_count = 1;
            #region Loop
            while (index+1<list.Length)
            {
                // If next event command text is on the second line or after
                if (list[index + 1].Code == 401)
                {
                    // Add the second line or after to message_text
                    InGame.Temp.MessageText += list[index + 1].StringParams[0] + "\n";
                    line_count += 1;
                }
                // If event command is not on the second line or after
                else
                {
                    // If next event command is show choices
                    if (list[index + 1].Code == 102)
                    {
                        // If choices fit on screen
                        if (list[index + 1].StringParams.Length < 5 - line_count)
                        {
                            // Advance index
                            index += 1;
                            // Choices setup
                            InGame.Temp.ChoiceStart = line_count;
                            SetupChoices(list[index].IntParams[0], list[index].StringParams);
                        }
                        // If next event command is input number
                        else
                        {
                            if (list[index + 1].Code == 103)
                            {
                                // If number input window fits on screen
                                if (line_count < 4)
                                {
                                    // Advance index
                                    index += 1;
                                    // Number input setup
                                    InGame.Temp.NumInputStart = line_count;
                                    InGame.Temp.NumInputVariableId = list[index].IntParams[0];
                                    InGame.Temp.NumInputDigitsMax = list[index].IntParams[1];
                                }
                            }
                        }
                    }
                    // Continue
                    return true;
                }
                // Advance index
                index += 1;
            }
            return true;
            #endregion
        }
    }
}